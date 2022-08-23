using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ServicesOrientedAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ServicesOrientedAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private const string ClassCategory = "Class";
        private const string RecordCategory = "Record";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ClassWithDataRule, DerivedClassesFieldRule, NotPublicRule,
                ClassMissingInterfaceRule, ClassMethodMissingInterfaceRule, RecordWithMethodRule,
                NotPublicInRecordRule, InterfaceInRecordRule, ConstructorNotDIRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.DeclaredAccessibility != Accessibility.Public)
            {
                context.ReportDiagnostic(Diagnostic.Create(NotPublicRule,
                    namedTypeSymbol.Locations[0],
                    namedTypeSymbol.Name));

                return;
            }

            var members = namedTypeSymbol.GetMembers();

            if (namedTypeSymbol.IsRecord)
            {
                var method = members.FirstOrDefault(m =>
                    m.Kind == SymbolKind.Method && m.CanBeReferencedByName && !m.IsImplicitlyDeclared);
                if (method != null)
                    context.ReportDiagnostic(Diagnostic.Create(RecordWithMethodRule,
                        method.Locations[0],
                        namedTypeSymbol.Name, method.Name));

                var field = members.FirstOrDefault(m =>
                    (m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property) && m.CanBeReferencedByName &&
                    !m.IsImplicitlyDeclared &&
                    m.DeclaredAccessibility != Accessibility.Public);
                if (field != null)
                    context.ReportDiagnostic(Diagnostic.Create(NotPublicInRecordRule, field.Locations[0],
                        namedTypeSymbol.Name, field.Name));

                var restrictedConstructor = namedTypeSymbol.Constructors.FirstOrDefault(c =>
                    c.Parameters.Any(t => t.Type.TypeKind == TypeKind.Interface));

                if (restrictedConstructor != null)
                    context.ReportDiagnostic(Diagnostic.Create(InterfaceInRecordRule,
                        restrictedConstructor.Locations[0],
                        namedTypeSymbol.Name, restrictedConstructor.Name));
            }
            else if (namedTypeSymbol.TypeKind == TypeKind.Class)
            {
                if (namedTypeSymbol.IsStatic)
                    if (members.All(m => m.Kind == SymbolKind.Method && m.IsStatic))
                    {
                        var allPass = true;

                        foreach (var member in members)
                            if (member is IMethodSymbol method && method.IsExtensionMethod &&
                                method.Parameters.All(p => p.Type.GetType() == typeof(IServiceCollection)))
                                allPass = false;

                        if (allPass) return;
                    }

                if (!namedTypeSymbol.Interfaces.Any())
                    context.ReportDiagnostic(Diagnostic.Create(ClassMissingInterfaceRule,
                        namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name));

                if (namedTypeSymbol.IsStatic || namedTypeSymbol.IsAbstract)
                    context.ReportDiagnostic(Diagnostic.Create(DerivedClassesFieldRule,
                        namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name));

                var field = members.FirstOrDefault(m =>
                    (m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property) && m.CanBeReferencedByName &&
                    m.DeclaredAccessibility != Accessibility.Private);
                if (field != null)
                    context.ReportDiagnostic(Diagnostic.Create(ClassWithDataRule, field.Locations[0],
                        namedTypeSymbol.Name, field.Name));

                var methods = members.Where(m => m.Kind == SymbolKind.Method && m.CanBeReferencedByName);
                foreach (var method in methods)
                    if (!namedTypeSymbol.Interfaces.Any(i => i.MemberNames.Contains(method.Name)))
                        context.ReportDiagnostic(Diagnostic.Create(ClassMethodMissingInterfaceRule,
                            method.Locations[0],
                            namedTypeSymbol.Name, method.Name));

                var restrictedConstructor = namedTypeSymbol.Constructors.FirstOrDefault(c =>
                    c.Parameters.Any(t => t.Type.TypeKind != TypeKind.Interface));

                if (restrictedConstructor != null)
                    context.ReportDiagnostic(Diagnostic.Create(ConstructorNotDIRule,
                        restrictedConstructor.Locations[0],
                        namedTypeSymbol.Name, restrictedConstructor.Name));

                if (
                    namedTypeSymbol.Constructors.Count(c => !c.IsImplicitlyDeclared) > 1)
                    context.ReportDiagnostic(Diagnostic.Create(ConstructorNotDIRule,
                        namedTypeSymbol.Constructors[0]
                            .Locations[0],
                        namedTypeSymbol.Name, namedTypeSymbol.Constructors[0]
                            .Name));
            }
        }

        #region Not Public

        public const string NotPublic = nameof(NotPublic);
        public const string NotPublicMessage = "Symbol '{0}' is not public.";

        public const string NotPublicDescription =
            "Recommend having everything public. Situations where you want to not be open to extensibility should be very rare.";

        private static readonly DiagnosticDescriptor NotPublicRule = new DiagnosticDescriptor(NotPublic,
            NotPublic, NotPublicMessage,
            ClassCategory, DiagnosticSeverity.Warning, true, NotPublicDescription);

        #endregion

        #region Not Public in Record

        public const string NotPublicInRecord = nameof(NotPublicInRecord);
        public const string NotPublicInRecordMessage = "Record '{0}' contains a non public member named '{1}'";

        public const string NotPublicInRecordDescription =
            "Records are restricted to be plain old class objects. These do not have any logic inside of them. This helps keep business logic and data seperate.";

        private static readonly DiagnosticDescriptor NotPublicInRecordRule = new DiagnosticDescriptor(NotPublicInRecord,
            NotPublicInRecord, NotPublicInRecordMessage,
            RecordCategory, DiagnosticSeverity.Error, true, NotPublicInRecordDescription);

        #endregion

        #region Interface in Record

        public const string InterfaceInRecord = nameof(InterfaceInRecord);

        public const string InterfaceInRecordMessage =
            "Record '{0}' contains a constructor with an interface parameter named '{1}'";

        public const string InterfaceInRecordDescription =
            "Records are restricted to be plain old class objects. These do not have logic or dependency injection. This helps keep business logic and data seperate.";

        private static readonly DiagnosticDescriptor InterfaceInRecordRule = new DiagnosticDescriptor(InterfaceInRecord,
            InterfaceInRecord, InterfaceInRecordMessage,
            RecordCategory, DiagnosticSeverity.Error, true, InterfaceInRecordDescription);

        #endregion

        #region Constructor not dependency injected

        public const string ConstructorNotDI = nameof(ConstructorNotDI);

        public const string ConstructorNotDIMessage =
            "Class '{0}' contains a constructor with a non interface parameter named '{1}'";

        public const string ConstructorNotDIDescription =
            "Classes should be dependency injected. This gets overly complex if they have multiple constructors or constructors with other values.";

        private static readonly DiagnosticDescriptor ConstructorNotDIRule = new DiagnosticDescriptor(ConstructorNotDI,
            ConstructorNotDI, ConstructorNotDIMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ConstructorNotDIDescription);

        #endregion

        #region Class With Data

        public const string ClassWithData = nameof(ClassWithData);
        public const string ClassWithDataMessage = "Class '{0}' contains a field or property named '{1}'";

        public const string ClassWithDataDescription =
            "These are restricted inside of classes. To define models use records. This helps keep business logic and data seperate.";

        private static readonly DiagnosticDescriptor ClassWithDataRule = new DiagnosticDescriptor(ClassWithData,
            ClassWithData, ClassWithDataMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ClassWithDataDescription);

        #endregion

        #region Class Missing Interface

        public const string ClassMissingInterface = nameof(ClassMissingInterface);
        public const string ClassMissingInterfaceMessage = "Class '{0}' is missing an interface attribute";

        public const string ClassMissingInterfaceDescription =
            "All classes must use interfaces so that they are open to extension by other libraries.";

        private static readonly DiagnosticDescriptor ClassMissingInterfaceRule = new DiagnosticDescriptor(
            ClassMissingInterface,
            ClassMissingInterface, ClassMissingInterfaceMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ClassMissingInterfaceDescription);

        public const string ClassMethodMissingInterface = nameof(ClassMethodMissingInterface);

        public const string ClassMethodMissingInterfaceMessage =
            "Class '{0}' method '{1}' is missing an interface attribute";

        public const string ClassMethodMissingInterfaceDescription =
            "All classes must use interfaces so that they are open to extension by other libraries.";

        private static readonly DiagnosticDescriptor ClassMethodMissingInterfaceRule = new DiagnosticDescriptor(
            ClassMethodMissingInterface,
            ClassMethodMissingInterface, ClassMethodMissingInterfaceMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ClassMethodMissingInterfaceDescription);

        #endregion

        #region Derived Classes

        public const string DerivedClasses = nameof(DerivedClasses);
        public const string DerivedClassesMethod = nameof(DerivedClassesMethod);

        public const string DerivedClassesMessage =
            "Class '{0}' contains virtual, static, or override keywords.";

        public const string DerivedClassesFieldDescription =
            "Instead of using derived classes use services. See the Decorator pattern for more help. Services can be swapped at runtime where derived and static classes can not providing better flexibility.";

        private static readonly DiagnosticDescriptor DerivedClassesFieldRule = new DiagnosticDescriptor(DerivedClasses,
            DerivedClasses, DerivedClassesMessage,
            ClassCategory, DiagnosticSeverity.Error, true, DerivedClassesFieldDescription);

        #endregion

        #region Record With Method

        public const string RecordWithMethod = nameof(RecordWithMethod);
        public const string RecordWithMethodMessage = "Record '{0}' contains a method named '{1}'";

        public const string RecordWithMethodDescription =
            "These are restricted inside of records. To define business logic use classes. This helps keep business logic and data seperate.";

        private static readonly DiagnosticDescriptor RecordWithMethodRule = new DiagnosticDescriptor(RecordWithMethod,
            RecordWithMethod, RecordWithMethodMessage,
            RecordCategory, DiagnosticSeverity.Error, true, RecordWithMethodDescription);

        #endregion
    }
}