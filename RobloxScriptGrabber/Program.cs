using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using RobloxFiles;

namespace RobloxScriptGrabber
{
    internal class Program
    {

        public static string RemoveInvalidFilePathCharacters(string filename, char replaceChar)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar.ToString());
        }

        public static string GetShortendScriptName(ModuleScript script)
        {
            return "M";
        }
        public static string GetShortendScriptName(Script script)
        {
            return script.ClassName == "Script" ? "S" : "C";
        }
        private static void Save(ModuleScript script,string newPath)
        {
            File.WriteAllBytes(Path.Combine(newPath,$"{GetShortendScriptName(script)}_{ RemoveInvalidFilePathCharacters(script.Name,'_') }[{script.ScriptGuid}].lua"), script.Source);
        }
        private static void Save(Script script,string newPath)
        {
            File.WriteAllBytes(Path.Combine(newPath,$"{GetShortendScriptName(script)}_{ RemoveInvalidFilePathCharacters(script.Name,'_') }[{script.ScriptGuid}].lua"), script.Source);
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

            var scScripts = file.GetDescendantsOfType<Script>();
            var moduleScripts = file.GetDescendantsOfType<ModuleScript>();
            
            foreach (Script script in scScripts)
            {
                Save(script,newPath);
            }
            foreach (ModuleScript script in moduleScripts)
            {
                Save(script,newPath);
            }
            Console.WriteLine($"Done. Saved {scScripts.Length + moduleScripts.Length} scripts.");
            
        }
    }
}