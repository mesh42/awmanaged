using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using NUnit.Framework;

namespace AwManaged.Tests.UnitTests
{
    [TestFixture]
    public class CompilerTests
    {
        [Test]
        public void CompileSimpleClassTest()
        {
            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "temp";
            parameters.GenerateInMemory = true;

            //var cls = new code

            var member = new CodeMemberProperty();
            member.HasGet = true;
            member.HasGet = true;
            member.Name = "test";
            member.Type = new CodeTypeReference(typeof(int));

            var memmber = new CodeTypeMember();
            memmber.Name = "bla";
            var r = memmber.ToString();

            var results = codeProvider.CompileAssemblyFromSource(parameters, "public class test{}");
        }
    }
}
