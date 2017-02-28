using System.Collections.Generic;
using System.Linq;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AopCodeGeneration
{
    public sealed class AopProxy
    {

        //internal sealed class Rewriter : CSharpSyntaxRewriter
        //{
        //    private readonly string _interfaceName;
        //    private readonly INamespaceSymbol _nameSpace;

        //    public Rewriter(string interfaceName, INamespaceSymbol nameSpace)
        //    {
        //        _interfaceName = interfaceName;
        //        _nameSpace = nameSpace;
        //    }

        //    public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        //    {
        //        var newNode = (NamespaceDeclarationSyntax) base.VisitNamespaceDeclaration(node);
        //        return newNode.WithName(GetNameSpaceName());
        //    }

        //    public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        //    {
        //        var newNode = (ClassDeclarationSyntax) base.VisitClassDeclaration(node);
        //        return newNode.WithIdentifier(Identifier(GetAopProxyClassName(_interfaceName)));
        //    }

        //    public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        //    {
        //        var newNode = (ConstructorDeclarationSyntax) base.VisitConstructorDeclaration(node);
        //        return newNode.WithIdentifier(Identifier(GetAopProxyClassName(_interfaceName)));
        //    }

        //    public override SyntaxNode VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        //    {
        //        var newNode = (SimpleBaseTypeSyntax) base.VisitSimpleBaseType(node);
        //        return newNode.WithType(IdentifierName(_interfaceName));
        //    }

        //    public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        //    {
        //        var newNode = (VariableDeclarationSyntax) base.VisitVariableDeclaration(node);
        //        return newNode.Parent is FieldDeclarationSyntax
        //               && ((IdentifierNameSyntax) newNode.Type).Identifier.Text == nameof(ITemplateInterface)
        //            ? newNode.WithType(IdentifierName(_interfaceName))
        //            : newNode;
        //    }

        //    public override SyntaxNode VisitParameter(ParameterSyntax node)
        //    {
        //        var newNode = (ParameterSyntax) base.VisitParameter(node);
        //        return newNode.Parent.Parent is ConstructorDeclarationSyntax
        //               && ((IdentifierNameSyntax) newNode.Type).Identifier.Text == nameof(ITemplateInterface)
        //            ? newNode
        //                .WithType(IdentifierName(_interfaceName))
        //                .WithLeadingTrivia(node.GetLeadingTrivia())
        //            : newNode;
        //    }

        //    private static string GetAopProxyClassName(string interfaceName)
        //    {
        //        var className = interfaceName.StartsWith("I")
        //            ? interfaceName.Substring(1, interfaceName.Length - 1)
        //            : interfaceName;

        //        return className + "AopProxy";
        //    }

        //    private NameSyntax GetNameSpaceName()
        //    {
        //        NameSyntax nameSpace = IdentifierName(_nameSpace.Name);
        //        var tmpNameSpace = _nameSpace.ContainingNamespace;
        //        while (!tmpNameSpace.IsGlobalNamespace)
        //        {
        //            nameSpace = QualifiedName(IdentifierName(tmpNameSpace.Name), (SimpleNameSyntax) nameSpace);
        //            tmpNameSpace = tmpNameSpace.ContainingNamespace;
        //        }
        //        return nameSpace;
        //    }
        //}

        //public SyntaxTree Generate(Document document, INamedTypeSymbol namedTypeSymbol)
        //{
        //    if (namedTypeSymbol.TypeKind != TypeKind.Interface)
        //    {
        //        return null;
        //    }

        //    var rootNode = ParseSyntaxTree(Resources.TemplateInterfaceAopProxy).GetRoot();

        //    var rewriter = new Rewriter(namedTypeSymbol.Name, namedTypeSymbol.ContainingNamespace);

        //    rootNode = rewriter.Visit(rootNode);


        //    //CompilationUnitSyntax cu = CompilationUnit();
        //    ////TO DO partial classes
        //    //cu = cu.AddUsings(
        //    //    initialCompilationUnit
        //    //        .Usings
        //    //        .Select(u => UsingDirective(IdentifierName(u.Name.ToString())))
        //    //        .ToArray());



        //    //cu = cu.AddMembers(templateClassDeclarationSyntax);

        //    var cw = new AdhocWorkspace();
        //    SyntaxNode formattedNode = Formatter.Format(rootNode, cw)
        //        .WithLeadingTrivia(CarriageReturnLineFeed);

        //    return formattedNode.SyntaxTree;
        //}

        public ClassDeclarationSyntax Generate(InterfaceDeclarationSyntax interfaceDeclarationSyntax, SemanticModel semanticModel)
        {
            return new AopProxyFactory().Create(interfaceDeclarationSyntax, semanticModel);
        }

        internal class AopProxyFactory
        {
            public ClassDeclarationSyntax Create(InterfaceDeclarationSyntax interfaceDeclarationSyntax, SemanticModel semanticModel)
            {
                return ClassDeclaration(
                    GetAopProxyClassName(interfaceDeclarationSyntax.Identifier.Text))
                    .WithModifiers(ConcatWithSealed(interfaceDeclarationSyntax.Modifiers))
                    .WithBaseList(SingleBaseType(IdentifierName(interfaceDeclarationSyntax.Identifier)))
                    .WithMembers(GenerateMembers(interfaceDeclarationSyntax, semanticModel));
            }

            private static string GetAopProxyClassName(string interfaceName)
            {
                var className = interfaceName.StartsWith("I")
                    ? interfaceName.Substring(1, interfaceName.Length - 1)
                    : interfaceName;

                return className + "AopProxy";
            }

            private BaseListSyntax SingleBaseType(TypeSyntax typeSyntax)
            {
                return BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(typeSyntax)));
            }

            private SyntaxTokenList ConcatWithSealed(SyntaxTokenList source)
            {
                return TokenList(source.Concat(new[] {Token(SyntaxKind.SealedKeyword)}));
            }

            private SyntaxList<MemberDeclarationSyntax> GenerateMembers(
                InterfaceDeclarationSyntax interfaceDeclarationSyntax,
                SemanticModel semanticModel)
            {
                var members = List<MemberDeclarationSyntax>();

                members = members.Add(GeneratePrivateReadonlyField(nameof(InterceptionHandler)));
                members = members.Add(
                    GeneratePrivateReadonlyField(interfaceDeclarationSyntax.Identifier.Text, "_realInstance"));
                members = members.Add(GenerateConstructor(interfaceDeclarationSyntax.Identifier.Text));

                foreach (ISymbol interfaceMember in GetInterfaceMembers(interfaceDeclarationSyntax, semanticModel))
                {
                    if (interfaceMember is IMethodSymbol)
                    {
                        members = members.Add(GenerateWrappingMethod((IMethodSymbol) interfaceMember, interfaceDeclarationSyntax));
                        members = members.Add(GeneratePassingMethod((IMethodSymbol) interfaceMember));
                    }
                    else if (interfaceMember is IPropertySymbol)
                    {

                    }
                    else if (interfaceMember is IEventSymbol)
                    {

                    }
                }

                return members;
            }

            private static MemberDeclarationSyntax GeneratePrivateReadonlyField(string typeName)
            {
                return GeneratePrivateReadonlyField(typeName, PrivateFieldName(typeName));
            }

            private static MemberDeclarationSyntax GeneratePrivateReadonlyField(string typeName, string variableName)
            {
                return FieldDeclaration(
                    VariableDeclaration(IdentifierName(typeName))
                        .WithVariables(SingleVariableOfType(variableName)))
                    .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)));
            }

            private static SeparatedSyntaxList<VariableDeclaratorSyntax> SingleVariableOfType(string variableName)
            {
                return SingletonSeparatedList(VariableDeclarator(Identifier(variableName)));
            }

            private static string PrivateFieldName(string typeName)
            {
                return $"_{PrivateVariableName(typeName)}";
            }

            private static string PrivateVariableName(string typeName)
            {
                return $"{char.ToLower(typeName[0])}{typeName.Substring(1, typeName.Length - 1)}";
            }

            private static string ParameterName(string typeName)
            {
                return $"{char.ToLower(typeName[0])}{typeName.Substring(1, typeName.Length - 1)}";
            }

            private MemberDeclarationSyntax GenerateConstructor(string typeName)
            {
                var parameters = ParameterList().AddParameters(
                    GenerateParameter(typeName, "realInstance"),
                    GenerateParameter(nameof(InterceptionHandler), PrivateVariableName(nameof(InterceptionHandler))));

                return ConstructorDeclaration(Identifier(GetAopProxyClassName(typeName)))
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(parameters)
                    .WithBody(
                        Block(
                            GenerateAssignment("_realInstance", "realInstance"),
                            GenerateAssignment(
                                PrivateFieldName(nameof(InterceptionHandler)),
                                PrivateVariableName(nameof(InterceptionHandler)))));
            }

            private static ParameterSyntax GenerateParameter(string typeName, string parameterName)
            {
                return Parameter(Identifier(parameterName)).WithType(IdentifierName(typeName));
            }

            private static ExpressionStatementSyntax GenerateAssignment(string left, string right)
            {
                return ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(left),
                        IdentifierName(right)));
            }

            private static List<ISymbol> GetInterfaceMembers(
                InterfaceDeclarationSyntax interfaceDeclarationSyntax,
                SemanticModel semanticModel)
            {
                INamedTypeSymbol symbolInfo = semanticModel.GetDeclaredSymbol(interfaceDeclarationSyntax);

                var members = new List<ISymbol>(symbolInfo.GetMembers());

                foreach (var baseInterface in GetBaseInterfaces(symbolInfo).Distinct())
                {
                    members.AddRange(baseInterface.GetMembers());
                }

                return members;
            }

            private static List<INamedTypeSymbol> GetBaseInterfaces(INamedTypeSymbol interfaceSymbol)
            {
                var members = new List<INamedTypeSymbol>();
                foreach (INamedTypeSymbol baseInterfaceSymbol in interfaceSymbol.AllInterfaces)
                {
                    members.Add(baseInterfaceSymbol);
                    members.AddRange(GetBaseInterfaces(baseInterfaceSymbol));
                }
                return members;
            }

            private static MemberDeclarationSyntax GenerateWrappingMethod(IMethodSymbol methodSymbol, InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                var method = MethodDeclaration(GenerateReturnType(methodSymbol), Identifier(methodSymbol.Name))
                    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                    .WithBody(GenerateMethodBody(methodSymbol, interfaceDeclarationSyntax));

                if (methodSymbol.Parameters.Length > 0)
                {
                    method = method.WithParameterList(GenerateParameters(methodSymbol));
                }

                return method;
            }

            private static TypeSyntax GenerateReturnType(IMethodSymbol methodSymbol)
            {
                if (methodSymbol.ReturnsVoid)
                {
                    return PredefinedType(Token(SyntaxKind.VoidKeyword));
                }

                return IdentifierName(methodSymbol.ReturnType.Name);
            }

            private static ParameterListSyntax GenerateParameters(IMethodSymbol methodSymbol)
            {
                return ParameterList().AddParameters(
                    methodSymbol.Parameters
                        .Select(p => GenerateParameter(p.Type.Name, p.Name))
                        .ToArray());
            }

            private static BlockSyntax GenerateMethodBody(
                IMethodSymbol methodSymbol,
                InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                return Block(
                    GenerateVariableDeclaration(
                        PrivateVariableName(nameof(InvocationInfo)),
                        GenerateInvocation(
                            nameof(InvocationInfo),
                            nameof(InvocationInfo.Create),
                            GetInvocationInfoCreateArguments(
                                methodSymbol,
                                interfaceDeclarationSyntax))),
                    ExpressionStatement(
                        GenerateInvocation(
                            PrivateFieldName(nameof(InterceptionHandler)),
                            nameof(InterceptionHandler.Invoke),
                            GenerateLocalArgument(PrivateVariableName(nameof(InvocationInfo))))));
            }

            private static ExpressionSyntax GenerateInvocation(
                string memberName,
                string methodName,
                ArgumentListSyntax arguments)
            {
                return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(memberName),
                        IdentifierName(methodName)),
                    arguments);
            }

            private static LocalDeclarationStatementSyntax GenerateVariableDeclaration(
                string variableName,
                ExpressionSyntax initializer)
            {
                return LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName("var"))
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                    Identifier(variableName))
                                    .WithInitializer(
                                        EqualsValueClause(initializer)))));
            }

            private static ArgumentListSyntax GetInvocationInfoCreateArguments(
                IMethodSymbol methodSymbol,
                InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                return ArgumentList(
                    SeparatedList<ArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Argument(
                                TypeOfExpression(
                                    IdentifierName(GetAopProxyClassName(interfaceDeclarationSyntax.Identifier.Text)))),
                            Token(SyntaxKind.CommaToken),
                            Argument(
                                InvocationExpression(IdentifierName("nameof"), GenerateLocalArgument(methodSymbol.Name))),
                            Token(SyntaxKind.CommaToken),
                            Argument(IdentifierName(methodSymbol.Name))
                        }));
            }

            private static ArgumentListSyntax GenerateLocalArgument(string identifierName)
            {
                return ArgumentList(
                    SingletonSeparatedList(
                        Argument(
                            IdentifierName(identifierName))));
            }

            private static MemberDeclarationSyntax GeneratePassingMethod(IMethodSymbol methodSymbol)
            {
                var parameters = ParameterList().AddParameters(
                    GenerateParameter(nameof(InvocationInfo), ParameterName(nameof(InvocationInfo))));

                return MethodDeclaration(IdentifierName(nameof(InvocationInfo)), Identifier(methodSymbol.Name))
                    .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword)))
                    .WithParameterList(parameters)
                    .WithBody(
                        Block(
                            GenerateInvocation("_realInstance", methodSymbol.Name),
                            ReturnStatement(IdentifierName(ParameterName(nameof(InvocationInfo))))));
            }

            private static ExpressionStatementSyntax GenerateInvocation(string memberName, string methodName)
            {
                return ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(memberName),
                            IdentifierName(methodName))));
            }
        }
    }
}
