﻿//MdStart
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonStaticLiterals = CommonBase.StaticLiterals;

namespace SolutionCopier.ConApp
{
    internal partial class Program
    {
        static Program()
        {
            ClassConstructing();
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                        Environment.OSVersion.Platform == PlatformID.MacOSX)
                       ? Environment.GetEnvironmentVariable("HOME")
                       : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = GetCurrentSolutionPath();
            TargetPath = Directory.GetParent(SourcePath).FullName;
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private static string HomePath { get; set; }
        private static string UserPath { get; set; }
        private static string SourcePath { get; set; }
        private static string TargetPath { get; set; }
        private static void Main(/*string[] args*/)
        {
            Console.WriteLine(nameof(SolutionCopier));

            var input = string.Empty;
            var sourceSolutionName = GetCurrentSolutionName();
            var targetSolutionName = "TargetSolution";
            var sourceProjects = CommonStaticLiterals.SolutionProjects
                                               .Concat(CommonStaticLiterals.GeneratorProjects
                                                                     .Select(e => $"{e}"))
                                               .Concat(CommonStaticLiterals.ProjectExtensions
                                                                     .Select(e => $"{sourceSolutionName}{e}"));

            while (input.Equals("x") == false)
            {
                Console.Clear();
                Console.WriteLine("Solution copier!");
                Console.WriteLine("================");
                Console.WriteLine();
                Console.WriteLine($"Copy '{sourceSolutionName}' from: {SourcePath}");
                Console.WriteLine($"Copy to '{targetSolutionName}':   {Path.Combine(TargetPath, targetSolutionName)}");
                Console.WriteLine();
                Console.WriteLine("[1] Change target path");
                Console.WriteLine("[2] Change target solution name");
                Console.WriteLine("[3] Start copy process");
                Console.WriteLine("[x|X] Exit");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Choose: ");
                input = Console.ReadLine().ToLower();

                if (input.Equals("1"))
                {
                    Console.Write("Enter target path: ");
                    TargetPath = Console.ReadLine();
                }
                else if (input.Equals("2"))
                {
                    Console.Write("Enter target solution name: ");
                    targetSolutionName = Console.ReadLine();
                }
                else if (input.Equals("3"))
                {
                    var sc = new Copier();

                    PrintBusyProgress();
                    sc.Copy(SourcePath, Path.Combine(TargetPath, targetSolutionName), sourceProjects);
                    runBusyProgress = false;
                }
                Console.ResetColor();
            }
        }
        private static bool canBusyPrint = true;
        private static bool runBusyProgress = false;
        private static void PrintBusyProgress()
        {
            Console.WriteLine();
            runBusyProgress = true;
            Task.Factory.StartNew(async () =>
            {
                while (runBusyProgress)
                {
                    if (canBusyPrint)
                    {
                        Console.Write(".");
                    }
                    await Task.Delay(250).ConfigureAwait(false);
                }
            });
        }
        private static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(SolutionCopier)}", StringComparison.CurrentCultureIgnoreCase);
            var result = AppContext.BaseDirectory[..endPos];

            while (result.EndsWith("/"))
            {
                result = result[0..^1];
            }
            while (result.EndsWith("\\"))
            {
                result = result[0..^1];
            }
            return result;
        }
        private static string GetCurrentSolutionName()
        {
            var solutionPath = GetCurrentSolutionPath();

            return GetSolutionNameByFile(solutionPath);
        }
        private static string GetSolutionNameByPath(string solutionPath)
        {
            return solutionPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                               .Where(e => string.IsNullOrEmpty(e) == false)
                               .Last();
        }
        private static string GetSolutionNameByFile(string solutionPath)
        {
            var fileInfo = new DirectoryInfo(solutionPath).GetFiles()
                                                          .SingleOrDefault(f => f.Extension.Equals(".sln", StringComparison.CurrentCultureIgnoreCase));

            return fileInfo != null ? Path.GetFileNameWithoutExtension(fileInfo.Name) : string.Empty;
        }
    }
}
//MdEnd