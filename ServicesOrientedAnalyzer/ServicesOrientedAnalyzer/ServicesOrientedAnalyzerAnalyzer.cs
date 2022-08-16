using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ServicesOrientedAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ServicesOrientedAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "POCO";

        public const string ClassWithData = nameof(ClassWithData);
        public const string ClassWithFieldMessage = "Class '{0}' contains a field or property named '{1}'";

        public const string ClassWithFieldDescription =
            "These are restricted inside of classes. To define models use records. This helps keep business logic and data seperate.";

        public const string DerivedClasses = nameof(DerivedClasses);

        public const string DerivedClassesWithFieldMessage =
            "Class '{0}' contains a '{1}' with virtual or override keywords.";

        public const string DerivedClassesFieldDescription =
            "Instead of using derived classes instead use services. See the Decorator pattern for more help. Services can be swapped at runtime where derived classes can not providing better flexibility.";

        private static readonly DiagnosticDescriptor ClassWithFieldRule = new DiagnosticDescriptor(ClassWithData,
            ClassWithData, ClassWithFieldMessage,
            Category, DiagnosticSeverity.Error, true, ClassWithFieldDescription);

        private static readonly DiagnosticDescriptor DerivedClassesFieldRule = new DiagnosticDescriptor(DerivedClasses,
            DerivedClasses, DerivedClassesWithFieldMessage,
            Category, DiagnosticSeverity.Error, true, DerivedClassesFieldDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ClassWithFieldRule, DerivedClassesFieldRule);

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

            if (namedTypeSymbol.TypeKind == TypeKind.Class)
            {
                //if (namedTypeSymbol.IsStatic)

                var members = namedTypeSymbol.GetMembers();

                var field = members.FirstOrDefault(m =>
                    (m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property) && m.CanBeReferencedByName);
                if (field != null)
                    context.ReportDiagnostic(Diagnostic.Create(ClassWithFieldRule, namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name, field.Name));

                if (field.IsVirtual || field.IsOverride)
                    context.ReportDiagnostic(Diagnostic.Create(DerivedClassesFieldRule, namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name, field.Name));
            }
        }
    }
}