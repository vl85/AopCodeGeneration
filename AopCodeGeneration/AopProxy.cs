using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AopCodeGeneration
{
    public sealed class AopProxy
    {
        public SyntaxTree Generate(SyntaxTree syntaxTree)
        {
            var node = (CompilationUnitSyntax) syntaxTree.GetRoot();

            var cuSyntax = CompilationUnit();

            var usings = new UsingVisitor().VisitAndCreate(node);
            if (usings != null && usings.Length > 0)
            {
                cuSyntax = cuSyntax.WithUsings(List(usings));
            }

            var namespaceSyntax = new NamespaceVisitor().VisitAndCreate(node);

            var classSyntax = new InterfaceVisitor().VisitAndCreate(node);

            if (classSyntax != null)
            {
                if (namespaceSyntax != null)
                {
                    namespaceSyntax = namespaceSyntax
                        .WithMembers(
                            List<MemberDeclarationSyntax>(
                                new[]
                                {
                                    classSyntax
                                }));
                }
                else
                {
                    cuSyntax = cuSyntax.WithMembers(
                    List<MemberDeclarationSyntax>(
                        new[]
                        {
                            classSyntax
                        }));
                }
            }

            if (namespaceSyntax != null)
            {
                cuSyntax = cuSyntax.WithMembers(
                    List<MemberDeclarationSyntax>(
                        new[]
                        {
                            namespaceSyntax
                        }));
            }

            var cw = new AdhocWorkspace();
            SyntaxNode formattedNode = Formatter.Format(cuSyntax, cw)
                .WithLeadingTrivia(CarriageReturnLineFeed);

            return formattedNode.SyntaxTree;
        }
    }

    internal sealed class UsingVisitor : CSharpSyntaxWalker
    {
        private readonly List<UsingDirectiveSyntax> _result = new List<UsingDirectiveSyntax>();

        public UsingDirectiveSyntax[] VisitAndCreate(SyntaxNode node)
        {
            Visit(node);

            return _result.ToArray();
        }
        
        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            _result.Add(UsingDirective(node.Name));

            base.VisitUsingDirective(node);
        }
    }

    internal sealed class NamespaceVisitor : CSharpSyntaxWalker
    {
        private NamespaceDeclarationSyntax _result;

        public NamespaceDeclarationSyntax VisitAndCreate(SyntaxNode node)
        {
            Visit(node);

            return _result;
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            _result = NamespaceDeclaration(node.Name);

            base.VisitNamespaceDeclaration(node);
        }
    }

    internal sealed class InterfaceVisitor : CSharpSyntaxWalker
    {
        private ClassDeclarationSyntax _result;

        public ClassDeclarationSyntax VisitAndCreate(SyntaxNode node)
        {
            Visit(node);

            return _result;
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var classNamePart = EnsureNoI(node.Identifier.Text);

            GenerateClassSyntax(classNamePart);
            GenerateInheritedMembers(node);

            base.VisitInterfaceDeclaration(node);
        }

        private void GenerateInheritedMembers(InterfaceDeclarationSyntax node)
        {
            foreach (BaseTypeSyntax baseTypeSyntax in node.BaseList.Types)
            {
                //foreach (var VARIABLE in baseTypeSyntax.Type)
                //{
                    
                //}
            }
        }

        private void GenerateClassSyntax(string classNamePart)
        {
            _result = (ClassDeclarationSyntax) ParseSyntaxTree(
                string.Format(
                    @"
public sealed class {0}AopProxy : I{0}
{{
    private readonly InterceptionHandler _interceptionHandler;
    private readonly I{0} _realInstance;

    public {0}AopProxy(
        I{0} realInstance,
        InterceptionHandler interceptionHandler)
    {{
        _realInstance = realInstance;
        _interceptionHandler = interceptionHandler;
    }}
}}
",
                    classNamePart)).GetRoot().ChildNodes().First();
        }

        private string EnsureNoI(string source)
        {
            if (string.IsNullOrWhiteSpace(source)
                || source.Length < 2)
            {
                return source;
            }

            return source.StartsWith("I")
                ? source.Substring(1, source.Length - 1)
                : source;
        }
    }
}
