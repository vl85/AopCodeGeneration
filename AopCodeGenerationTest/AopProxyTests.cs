using System.Linq;
using System.Text;
using AopCodeGeneration;
using AopCodeGeneration.Samples;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AopCodeGenerationTest
{
    [TestClass]
    public class AopProxyTests
    {
        private const string AopCodeGenerationSolutionFilePath = @"..\..\..\AopCodeGeneration.sln";

        [TestMethod]
        public void Generate_BaseInterface_Done()
        {
            var document = GetDocument(GetFileName(nameof(IIgnoredInterface)));
            var interfaceDeclarationSyntax = GetInterface(document, nameof(IIgnoredInterface));

            var actualClassDeclarationSyntax = new AopProxy().Generate(
                interfaceDeclarationSyntax,
                document.GetSemanticModelAsync().Result);

            var expectedTree = GetClass(Resources.IgnoredInterfaceAopProxy);

            Assert.IsTrue(
                expectedTree.IsEquivalentTo(actualClassDeclarationSyntax, false),
                GetDifference(expectedTree, actualClassDeclarationSyntax));
        }

        [TestMethod]
        public void Generate_BaseAndCurrent_Done()
        {
            var document = GetDocument(GetFileName(nameof(IAppConfigPeriodicRefreshHandler)));
            var interfaceDeclarationSyntax = GetInterface(document, nameof(IAppConfigPeriodicRefreshHandler));

            var actualClassDeclarationSyntax = new AopProxy().Generate(
                interfaceDeclarationSyntax,
                document.GetSemanticModelAsync().Result);

            var expectedTree = GetClass(Resources.AppConfigPeriodicRefreshHandlerAopProxy);

            Assert.IsTrue(
                expectedTree.IsEquivalentTo(actualClassDeclarationSyntax),
                GetDifference(expectedTree, actualClassDeclarationSyntax));
        }

        private static string GetFileName(string interfaceName)
        {
            return $"{interfaceName}.cs";
        }

        private static Document GetDocument(string name)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(AopCodeGenerationSolutionFilePath).Result;
            var project = solution.Projects.First(p => p.Name == "AopCodeGeneration");
            return project.Documents.First(d => d.Name == name);
        }

        private static InterfaceDeclarationSyntax GetInterface(Document document, string name)
        {
            return document.GetSyntaxRootAsync().Result
                .DescendantNodes()
                .OfType<InterfaceDeclarationSyntax>()
                .Single(ids => ids.Identifier.Text == name);
        }

        private static ClassDeclarationSyntax GetClass(string text)
        {
            var expectedRootNode = SyntaxFactory.ParseSyntaxTree(text).GetRoot();
            return expectedRootNode.DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
        }

        private static string GetDifference(SyntaxNode first, SyntaxNode last)
        {
            return first.SyntaxTree.GetChanges(last.SyntaxTree)
                .Aggregate(new StringBuilder(), (s, tc) => s.AppendLine(tc.ToString()))
                .ToString();
        }

        private static string GetText(SyntaxNode syntaxNode)
        {
            //TODO wrap invocation arguments. Chop if long.
            return Formatter.Format(syntaxNode, new AdhocWorkspace())
                .WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                .GetText()
                .ToString();
        }

        /*
         * Method with parameter passes through
         * Method with result is returning result
         * Method with two parameters passes through
         * Method with parameter and result passes parameter and returns result
         * Property with getter is returning result
         * Property with setter is passing value
         * Property with getter and setter returns result and passes value
         * Event with EventHandler passes add and remove
         * Event with Action passes add and remove
         * Event with EventHandler<string> passes add and remove
         * Generic method with generic parameter passes through
         * Generic method with generic result returns it
         * Generic method with two generic parameters passes through
         * Generic method with generic parameter and generic result passes through and returns
         * Generic method with generic and non-generic parameters passes through
         * Method with ref parameter passes through and returns result
         * Method with out parameter returns result
         * Method with params parameter passes through
         * Generic method with parameter of generic type instantiated with generic argument passes through
         * Generic method with result of generic type instantiated with generic arguemtn passes through
         * Method with an overload passes through
         * Generic method with generic argument restrictions passes through and returns result
         * Method of generic interface passes parameter and returns result
         * Method of generic interface passes generic parameter and returns result
         * Generic method of generic interface passes parameter and returns result
         * Generic method of generic interface passes generic parameter and returns result
         * Internal interface creates internal AopProxy with internal constructor
         * Internal method creates internal proxy method
        */
    }
}
