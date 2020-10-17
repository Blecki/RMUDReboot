using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace CILGenerationTest
{
    class AncoraTestHarness
    {
        static void Test(string[] args)
        {
            var assemblyName = new System.Reflection.AssemblyName("CILGenerationInternalTest");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");
            var typeBuilder = moduleBuilder.DefineType("Demo");
            var methodBuilder = typeBuilder.DefineMethod("Execute", MethodAttributes.Static | MethodAttributes.Public, typeof(void), System.Type.EmptyTypes);

            var ilBuilder = methodBuilder.GetILGenerator();
            ilBuilder.EmitWriteLine("Hello Ancora!");
            ilBuilder.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
            moduleBuilder.CreateGlobalFunctions();

            var dynamicType = assemblyBuilder.GetType("Demo");
            var execMethod = dynamicType.GetMethod("Execute");

            execMethod.Invoke(null, null);

            var tGrammar = new Ancora.TestGrammar();

            while (true)
            {
                var input = Console.ReadLine();
                var itr = new Ancora.StringIterator(input);

                var r = tGrammar.Root.Parse(itr);
                if (r.ResultType == Ancora.ResultType.Success && !r.StreamState.AtEnd && r.FailReason == null)
                    r.FailReason = new Ancora.Failure(tGrammar.Root, "Did not consume all input.");

                if (r.ResultType == Ancora.ResultType.Success && r.StreamState.AtEnd)
                {
                    Console.WriteLine("Parsed.");
                    EmitAst(r.Node);
                }
                else
                {
                    Console.WriteLine("Failed.");
                    if (r.FailReason != null)
                        Console.WriteLine(r.FailReason.Message);
                    else
                        Console.WriteLine("No fail reason specified.");
                }
            }


        }

        static void EmitAst(Ancora.AstNode Node, int Depth = 0)
        {
            Console.WriteLine(new String(' ', Depth) + Node.NodeType + " : " + (Node.Value == null ? "null" : Node.Value.ToString()));
            foreach (var child in Node.Children) EmitAst(child, Depth + 1);
        }
    }
}
