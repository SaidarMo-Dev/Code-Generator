using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CodeGenerator_BusinessLayer
{
    class Util
    {

        
        static string _solutionDirectory = "C:\\GenerateProjects\\CSharpProjects";

        public static string solutionDirectory
        {
            get { return _solutionDirectory; }
        }

        public static string CreateConsoleProject(string ProjectName)
        {

           

            if (!Directory.Exists(_solutionDirectory))
            {
                Directory.CreateDirectory(_solutionDirectory);


            }


            string path = Path.Combine(_solutionDirectory,ProjectName);


            if (Directory.Exists(path))
                return path;

 
            string command = "dotnet new console -n " + ProjectName;

            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _solutionDirectory


            };


            using (Process process = new Process())
            {
                process.StartInfo = processInfo;

                process.Start();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    return path;

                }
                else
                {
                    
                    return null;
                }

            }

        }
  
        public static string CreateProjectDirectory (string ProjectName)
        {

            if (!Directory.Exists(_solutionDirectory))
            {
                Directory.CreateDirectory(_solutionDirectory);

            }


            string path = Path.Combine(_solutionDirectory, ProjectName);


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }


            return path;



        }
    
    }
}
