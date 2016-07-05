using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Authorize.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }
        public string SolutionDirectoryPath { get; set; }
        public string ProjectDirectoryPath { get; set; }

        // Will log an MessageImportance.High message to MSBuild. OPTIONAL
        public Action<string> LogInfo { get; set; }

        private Tuple<bool, string> GetDll(string root)
        {
            var packageDir = Path.Combine(root, "packages");
            var packageInfo = new DirectoryInfo(packageDir);

            if (packageInfo.Exists)
            {
                var webApi = packageInfo.GetDirectories("Microsoft.AspNet.WebApi.Core.*").LastOrDefault().FullName;
                var path = Path.Combine(webApi, @"lib\net45", "System.Web.Http.dll");
                return new Tuple<bool, string>(true, path);
            }

            return new Tuple<bool, string>(false, "");
        }

        private string DllPath()
        {
            LogInfo($" >> Solution Path | {SolutionDirectoryPath}");
            LogInfo($" >> Project Path | {ProjectDirectoryPath}");
            LogInfo($" >> Current Path | ${new DirectoryInfo("./").FullName}");

            var solution = GetDll(SolutionDirectoryPath);
            if (solution.Item1) return solution.Item2;

            var project = GetDll(ProjectDirectoryPath);
            if (project.Item1) return project.Item2;

            var dir = new DirectoryInfo("./");
            while (dir.Parent != null)
            {
                LogInfo($" >> Current Dir | {dir.FullName}");
                var current = GetDll(dir.FullName);
                if (current.Item1) return current.Item2;

                dir = dir.Parent;
            }

            return "";
        }

        public void Execute()
        {
            AddAttribute(ModuleDefinition);
        }

        private bool IsPostOrGet(CustomAttribute att, string targetName)
        {
            return att.AttributeType.FullName == targetName;
        }

        private bool IsPostOrGet(IEnumerable<CustomAttribute> attrs)
        {
            var get = "System.Web.Http.HttpGetAttribute";
            var post = "System.Web.Http.HttpPostAttribute";
            return attrs.Where(x => IsPostOrGet(x, get)).ToList().Count() > 0 ||
                   attrs.Where(x => IsPostOrGet(x, post)).ToList().Count() > 0;
        }

        private void AddAttribute(ModuleDefinition module)
        {
            var dllPath = DllPath();
            var attributeName = "System.Web.Http.AuthorizeAttribute";

            var asm = AssemblyDefinition.ReadAssembly(dllPath);
            var attr = asm.MainModule.GetType(attributeName);

            var ctor = attr.Methods.First(x => x.IsConstructor);
            var ctorReference = module.ImportReference(ctor);

            module.Types.ToList().ForEach(type =>
            {
                var targetMethods = type.Methods.Where(x => IsPostOrGet(x.CustomAttributes));
                targetMethods.ToList().ForEach(method =>
                {
                    if (method.ReturnType.FullName != "System.Net.Http.HttpResponseMessage")
                    {
                        var exist = method.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == attributeName);
                        if (exist == null)
                        {
                            method.CustomAttributes.Add(new CustomAttribute(ctorReference));
                        }
                    }
                });
            });
        }
    }
}
