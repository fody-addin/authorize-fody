using Authorize.Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Autorize.Fody.Tests.Spec
{
    public class ApiSpec
    {
        [Fact]
        public void ShouldAddAuthorizeAttribute()
        {
            var dll = "Authorize.Fody.Tests";
            var dir = new FileInfo(dll).Directory.FullName;
            var target = Path.Combine(dir, "Authorize.Fody.Tests.dll");
            var output = Path.Combine(dir, "Authorize.Fody.Tests.Output.dll");

            var moduleDefinition = ModuleDefinition.ReadModule(target);
            var weaver = new ModuleWeaver {
                ModuleDefinition = moduleDefinition,
                SolutionDirectoryPath = "../../../"
            };
            weaver.Execute();
            moduleDefinition.Write(output);
        }
    }
}
