using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using RobloxFiles;

namespace RobloxScriptGrabber
{
    internal class Program
    {

        public static string GetShortendScriptName(Script script)
        {
            return script.ClassName == "Script" ? "S" : "C";
        }
        
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("You are missing arguments");
                return;
            }
            if (! File.Exists (args[0]) || !Directory.Exists (args[1]))
            {
                Console.WriteLine("Path(s) should exist");
                return;
            }
            
            string ScriptPath = Path.Combine(args[1], "scripts");
            if (!Directory.Exists(ScriptPath))
                Directory.CreateDirectory(ScriptPath);

            var newPath = Path.Combine(ScriptPath, Path.GetFileName(args[0]));
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            
            RobloxFile file =RobloxFile.Open(args[0]);

            var scripts = file.GetDescendantsOfType<Script>();
            foreach (Script script in scripts)
            {
                File.WriteAllBytes(Path.Combine(newPath,$"{Program.GetShortendScriptName(script)}_{script.Name}[{script.ScriptGuid}].lua"), script.Source);
            }
            Console.WriteLine($"Done. Saved {scripts.Length} scripts.");
            
        }
    }
}