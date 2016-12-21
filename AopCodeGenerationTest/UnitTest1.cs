using System;
using System.IO;
using AopCodeGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AopCodeGenerationTest
{
    [TestClass]
    public class UnitTest1
    {
        private const string SamplesPath = @"..\..\..\aopcodegeneration\samples\";

        [TestMethod]
        public void Generate_NameSpace_IsGenerated()
        {
            var namespaceOnly =
                @"
namespace AopCodeGeneration.Samples
{
}";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(namespaceOnly);
            SyntaxTree result = new AopProxy().Generate(tree);

            Assert.AreEqual(namespaceOnly, result.GetText().ToString());
        }

        [TestMethod]
        public void Generate_Interface_IsGenerated()
        {
            var interfaceOnly =
                @"
public interface IIgnoredInterface
{
}";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(interfaceOnly);
            SyntaxTree result = new AopProxy().Generate(tree);

            var expectedClass = @"
public sealed class IgnoredInterfaceAopProxy : IIgnoredInterface
{
    private readonly InterceptionHandler _interceptionHandler;
    private readonly IIgnoredInterface _realInstance;

    public IgnoredInterfaceAopProxy(
        IIgnoredInterface realInstance,
        InterceptionHandler interceptionHandler)
    {
        _realInstance = realInstance;
        _interceptionHandler = interceptionHandler;
    }
}";
            
            Assert.AreEqual(expectedClass, result.GetText().ToString());
        }

        [TestMethod]
        public void Generate_Usings_IsGenerated()
        {
            var interfaceOnly =
                @"
using System;

";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(interfaceOnly);
            SyntaxTree result = new AopProxy().Generate(tree);

            Assert.AreEqual(interfaceOnly, result.GetText().ToString());
        }
        
        /// <summary>
        /// Empty interface generates *AopProxy implementing the interface with constructor with interface and InterceptionHandler arguments
        /// </summary>
        [TestMethod]
        public void Generate_InterfaceWithInterface_IsGenerated()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(SamplesPath, "iignoredinterface.cs")));

            SyntaxTree result = new AopProxy().Generate(tree);

            var expected = File.ReadAllText(Path.Combine(SamplesPath, "IgnoredInterfaceAopProxy"));
            Assert.AreEqual(expected, result.GetText().ToString());
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
