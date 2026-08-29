using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using System.Xml.Linq;
namespace MyAnlyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassNameHaveToBeUpperCaseAnlyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            "BI0001",
            "Class name must start with uppercase",
            "Class name '{0}' must start with an uppercase letter",
            "Naming",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction((symbolAnalysisContext) => AnalyzeClassNameIsUpperCase(symbolAnalysisContext,context),
                SyntaxKind.ClassDeclaration
            );
        }

        private void AnalyzeClassNameIsUpperCase(SyntaxNodeAnalysisContext syntaxContext, AnalysisContext context) 
        {
            var node = syntaxContext.Node;
            if (node is ClassDeclarationSyntax classDeclaration)
            {
                var className = classDeclaration.Identifier.Text;

                if (string.IsNullOrEmpty(className))
                    return;

                if (!char.IsUpper(className[0]))
                {
                    var location = classDeclaration.Identifier.GetLocation();
                    syntaxContext.ReportDiagnostic(
                        Diagnostic.Create(
                            Rule,
                            location,
                            className
                    ));
                }
            }
        }

    }
}
