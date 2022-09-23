using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ServicesOrientedAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ServicesOrientedAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private const string ClassCategory = "Class";
        private const string RecordCategory = "Record";

        private const string HelpLink = "https://github.com/holm0563/ImperativeArchitecture";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ClassWithDataRule, DerivedClassesFieldRule, NotPublicRule,
                ClassMethodMissingInterfaceRule, RecordWithMethodRule,
                NotPublicInRecordRule, InterfaceInRecordRule, ConstructorNotDiRule);

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
                            if (member is IMethodSymbol method && (!method.IsExtensionMethod ||
                                                                   !method.Parameters.All(p =>
                                                                       p.Type.Name.Contains("IServiceCollection"))))
                                allPass = false;

                        if (allPass) return;
                    }

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
                    context.ReportDiagnostic(Diagnostic.Create(ConstructorNotDiRule,
                        restrictedConstructor.Locations[0],
                        namedTypeSymbol.Name, restrictedConstructor.Name));

                if (
                    namedTypeSymbol.Constructors.Count(c => !c.IsImplicitlyDeclared) > 1)
                    context.ReportDiagnostic(Diagnostic.Create(ConstructorNotDiRule,
                        namedTypeSymbol.Constructors[0]
                            .Locations[0],
                        namedTypeSymbol.Name, namedTypeSymbol.Constructors[0]
                            .Name));
            }
        }

        #region Not Public

        public const string NotPublic = nameof(NotPublic);
        public const string NotPublicMessage = "Symbol '{0}' is not public";

        public const string NotPublicDescription =
            "It is recommended to have most code public. Situations where you do not want to be open to extensibility should be very rare or live in another project outside your shared library.";

        private static readonly DiagnosticDescriptor NotPublicRule = new DiagnosticDescriptor(NotPublic,
            NotPublic, NotPublicMessage,
            ClassCategory, DiagnosticSeverity.Warning, true, NotPublicDescription, HelpLink);

        #endregion

        #region Not Public in Record

        public const string NotPublicInRecord = nameof(NotPublicInRecord);
        public const string NotPublicInRecordMessage = "Record '{0}' contains a non public member named '{1}'";

        public const string NotPublicInRecordDescription =
            "Records are restricted to be plain old class objects. These can not have any logic inside of them. This helps keep business logic and data separate.";

        private static readonly DiagnosticDescriptor NotPublicInRecordRule = new DiagnosticDescriptor(NotPublicInRecord,
            NotPublicInRecord, NotPublicInRecordMessage,
            RecordCategory, DiagnosticSeverity.Error, true, NotPublicInRecordDescription, HelpLink);

        #endregion

        #region Interface in Record

        public const string InterfaceInRecord = nameof(InterfaceInRecord);

        public const string InterfaceInRecordMessage =
            "Record '{0}' contains a constructor with an interface parameter named '{1}'";

        public const string InterfaceInRecordDescription =
            "Records are restricted to be plain old class objects. These do not have logic or dependency injection. This helps keep business logic and data separate.";

        private static readonly DiagnosticDescriptor InterfaceInRecordRule = new DiagnosticDescriptor(InterfaceInRecord,
            InterfaceInRecord, InterfaceInRecordMessage,
            RecordCategory, DiagnosticSeverity.Error, true, InterfaceInRecordDescription, HelpLink);

        #endregion

        #region Constructor not dependency injected

        public const string ConstructorNotDi = nameof(ConstructorNotDi);

        public const string ConstructorNotDiMessage =
            "Class '{0}' contains a constructor with a non interface parameter named '{1}'";

        public const string ConstructorNotDiDescription =
            "Classes should be dependency injected. This gets overly complex if they have multiple constructors or constructors with other values.";

        private static readonly DiagnosticDescriptor ConstructorNotDiRule = new DiagnosticDescriptor(ConstructorNotDi,
            ConstructorNotDi, ConstructorNotDiMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ConstructorNotDiDescription, HelpLink);

        #endregion

        #region Class With Data

        public const string ClassWithData = nameof(ClassWithData);
        public const string ClassWithDataMessage = "Class '{0}' contains a field or property named '{1}'";

        public const string ClassWithDataDescription =
            "Non private fields and properties are restricted inside of classes. To define models use records. This helps keep business logic and data separate.";

        private static readonly DiagnosticDescriptor ClassWithDataRule = new DiagnosticDescriptor(ClassWithData,
            ClassWithData, ClassWithDataMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ClassWithDataDescription, HelpLink);

        #endregion

        #region Class Missing Interface

        public const string ClassMethodMissingInterface = nameof(ClassMethodMissingInterface);

        public const string ClassMethodMissingInterfaceMessage =
            "Class '{0}' method '{1}' is missing a corresponding interface";

        public const string ClassMethodMissingInterfaceDescription =
            "All classes methods must use interfaces so that they are open to extension by other libraries.";

        private static readonly DiagnosticDescriptor ClassMethodMissingInterfaceRule = new DiagnosticDescriptor(
            ClassMethodMissingInterface,
            ClassMethodMissingInterface, ClassMethodMissingInterfaceMessage,
            ClassCategory, DiagnosticSeverity.Error, true, ClassMethodMissingInterfaceDescription, HelpLink);

        #endregion

        #region Derived Classes

        public const string DerivedClasses = nameof(DerivedClasses);

        public const string DerivedClassesMessage =
            "Class '{0}' contains virtual, static, or override keywords";

        public const string DerivedClassesFieldDescription =
            "Instead of using derived classes use services. See the decorator pattern for more help. Services can be injected at runtime where derived and static classes can not providing better flexibility.";

        private static readonly DiagnosticDescriptor DerivedClassesFieldRule = new DiagnosticDescriptor(DerivedClasses,
            DerivedClasses, DerivedClassesMessage,
            ClassCategory, DiagnosticSeverity.Error, true, DerivedClassesFieldDescription, HelpLink);

        #endregion

        #region Record With Method

        public const string RecordWithMethod = nameof(RecordWithMethod);
        public const string RecordWithMethodMessage = "Record '{0}' contains a method named '{1}'";

        public const string RecordWithMethodDescription =
            "Methods are restricted inside of records. To define business logic use classes. This helps keep business logic and data separate.";

        private static readonly DiagnosticDescriptor RecordWithMethodRule = new DiagnosticDescriptor(RecordWithMethod,
            RecordWithMethod, RecordWithMethodMessage,
            RecordCategory, DiagnosticSeverity.Error, true, RecordWithMethodDescription, HelpLink);

        #endregion
    }
}