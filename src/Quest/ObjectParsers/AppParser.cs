﻿using Quest.IO;
using Quest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quest.ObjectParsers
{
    public static class AppParser
    {
        public static bool IsAppInConfig(App app)
        {
            Config conf = Setup.GetConfig();
            if (conf.Applications == null || conf.Applications.Count == 0)
                return false;
            if (!conf.Applications.Any(a => a.Name == app.Name))
                return false;
            return true;
        }

        private static string CreateFeaturePath(App app)
        {
            Config conf = Setup.GetConfig();
            bool? hasFeature = conf.Applications?.FirstOrDefault(a => a.Name == app.Name)
                                    .Features?.Any(f => f.Name == app.Features.First().Name);

            if (hasFeature == null || hasFeature == false)
            {
                conf.Applications.FirstOrDefault(a => a.Name == app.Name)
                    .Features = new List<Feature>() { new Feature() { Name = app.Features.First().Name } };
                YamlHandler.Update(Setup.GetConfigPath(), conf);
            }
            return conf.Applications.FirstOrDefault(a => a.Name == app.Name).LocalPath;
        }

        public static App GetAppFromCommandLineArguments(string[] args)
        {
            int appI = CommandLineArguments.GetIndexOfFlag(args, "--app") + 1;
            int featI = CommandLineArguments.GetIndexOfFlag(args, "--feature") + 1;

            if (!CommandLineArguments.IsArgumentValid(args, appI, featI))
                throw new ArgumentException("Missing one or more required arguments. \n Run 'quest help [command]' for more information.");

            App app = new App() { Name = args[appI], Features = new List<Feature>() { new Feature() { Name = args[featI] }}};

            if (!IsAppInConfig(app))
                throw new ArgumentException($"App '{app.Name}' not found.\n Please, run 'quest help config add' to learn how to add a new app.");

            app.LocalPath = CreateFeaturePath(app);
            return app;
        }
    }
}
