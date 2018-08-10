using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.JScript;

namespace Scene3DLib
{
    public class AppJSAutomator
    {
        public object app { get; set; }

        public AppJSAutomator(object app)
        {
            this.app = app;
        }

        public string onStartUp(string fn)
        {
            string Source = System.IO.File.ReadAllText(fn);

            var provider = new JScriptCodeProvider();
            var compiler = provider.CreateCompiler();
            var parameters = new CompilerParameters { GenerateInMemory = true };
            var results = compiler.CompileAssemblyFromSource(parameters, Source);
            var assembly = results.CompiledAssembly;
            dynamic instance = Activator.CreateInstance(assembly.GetType("StartupConfiguration"));
            var result = instance.getStartup3DFile(this.app);

            string sr = result.ToString();
            return sr;
        }
    }
}
