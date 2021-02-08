﻿using Quest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quest.Commands
{
    public static class DoneHandler
    {
        public static int HandleDone(string[] args)
        {
            if (args.Length == 2)
                Complete(args[1]);
            else
                ListDone(HandleListDone(args));
            return 0;
        }

        public static void Complete(string todoText, string todoFile = "", string doneFile = "")
        {
            string donePath = string.IsNullOrEmpty(doneFile) ? Path.Combine(Directory.GetCurrentDirectory(), "done.md") : doneFile;
            if (!File.Exists(donePath))
                using (File.Create(donePath)) { };
            string todoPath = string.IsNullOrEmpty(todoFile) ? Path.Combine(Directory.GetCurrentDirectory(), "todo.md") : todoFile;
            List<string> lines = File.ReadAllLines(todoPath).ToList();
            string line = lines.Find(t => t.Contains(todoText));
            lines.Remove(line);
            File.WriteAllLines(todoPath, lines);
            File.AppendAllText(donePath, $"{line} - Completed at: {DateTime.Now}\n");
        }

        public static int ListDone(App app)
        {
            List<string> files = new List<string>();
            if (app == null)
                files = GetAllDones();
            else
            {
                string path = Path.Combine(app.LocalPath, ".quest");
                if (app.Features != null && app.Features.Count() > 0)
                    path = Path.Combine(path, app.Features.FirstOrDefault(e => true).Name);

                files = Directory.GetFiles(path, "done.md", SearchOption.AllDirectories).ToList();
            }
            foreach (string file in files)
            {
                List<string> content = File.ReadAllLines(file).ToList();
                if (!content.Any(l => l.Contains("*")))
                    continue;
                Console.WriteLine($"Feature: {new DirectoryInfo(file).Parent.Name}");
                foreach (string line in content)
                    Console.WriteLine(line);
            }

            return 0;
        }

        public static App HandleListDone(string[] args)
        {
            try
            {
                int appIndex = 0;
                int featureIndex = 0;

                if (args.Length == 1)
                    return null;
                if (ArgumentsHandler.HasFlag(args, "--app"))
                    appIndex = ArgumentsHandler.GetIndexOfFlag(args, "--app") + 1;
                if (ArgumentsHandler.HasFlag(args, "--feature") && !ArgumentsHandler.HasFlag(args, "--app"))
                    throw new ArgumentException("When using '--feature', the '--app' flag is required. \n Run 'quest help [command]' for more information.");
                if (ArgumentsHandler.HasFlag(args, "--feature"))
                    featureIndex = ArgumentsHandler.GetIndexOfFlag(args, "--feature") + 1;

                if (string.IsNullOrEmpty(args[appIndex]) || string.IsNullOrWhiteSpace(args[appIndex]))
                    throw new ArgumentException("Missing one or more required arguments. \n Run 'quest help [command]' for more information.");
                else if (ArgumentsHandler.HasFlag(args, "--feature") && string.IsNullOrWhiteSpace(args[featureIndex]) || string.IsNullOrEmpty(args[featureIndex]))
                    throw new ArgumentException("Missing one or more required arguments. \n Run 'quest help [command]' for more information.");

                var conf = Setup.GetConfig();
                if (conf.Applications == null || conf.Applications.Count == 0)
                    throw new Exception("Could not find any applications to list tasks. Please, run 'quest config list' to learn how to create an application for Quest.");

                App app = new App()
                {
                    Name = args[appIndex],
                    LocalPath = conf.Applications.FirstOrDefault(a => a.Name == args[appIndex]).LocalPath
                };

                if (featureIndex != 0)
                {
                    bool? hasFeature = conf.Applications?.FirstOrDefault(a => a.Name == args[appIndex])
                                           .Features?.Any(f => f.Name == args[featureIndex]);

                    if (hasFeature == null || hasFeature == false)
                    {
                        Console.WriteLine(@$"Feature ""{args[featureIndex]}"" was not found in application ""{args[appIndex]}""");
                        Console.WriteLine("Getting all features...");
                        return app;
                    }
                    app.Features = new List<Feature>() { new Feature() { Name = args[featureIndex] } };
                    return app;
                }
                return app;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<string> GetAllDones()
        {
            Config config = Setup.GetConfig();
            if (config.Applications == null || config.Applications.Count == 0)
                return null;
            List<string> files = new List<string>();
            List<App> allApps = config.Applications;
            foreach (App app in allApps)
            {
                var todoFiles = Directory.GetFiles(Path.Combine(app.LocalPath, ".quest"), "done.md", SearchOption.AllDirectories);
                foreach (string file in todoFiles)
                    files.Add(file);
            }
            return files;
        }
    }
}
