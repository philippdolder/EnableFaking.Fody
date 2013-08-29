namespace EnableFaking.Fody
{
    using System;
    using System.IO;
    using System.Reflection;

    using Mono.Cecil;

    public class WeaverHelper
    {
        public static Assembly WeaveAssembly()
        {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            var assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
        assemblyPath = assemblyPath.Replace("Debug", "Release");
#endif

            var newAssembly = assemblyPath.Replace(".dll", "2.dll");
            File.Copy(assemblyPath, newAssembly, true);

            var moduleDefinition = ModuleDefinition.ReadModule(newAssembly);
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition
            };

            weavingTask.Execute();
            moduleDefinition.Write(newAssembly);

            return Assembly.LoadFile(newAssembly);
        }
    }
}