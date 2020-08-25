﻿using Quest.Console;
using Quest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Quest.Arguments
{
    public static class FeatHandler
    {
        public static Feature GetFeature(string[] args)
        {
            try
            {
                List<ValidationResult> re = new List<ValidationResult>();
                Feature feature = new Feature();
                feature.Name = "rob";
                var vc = new ValidationContext(feature);
                var valid = Validator.TryValidateObject(feature, vc, re, true);

                List<string> argsList = args.ToList();
                if (!args.Contains("create"))
                    throw new ArgumentException("A subcommand must be used with the command 'feat'");
                int createIndex = argsList.IndexOf("create");
                if (createIndex + 1 >= args.Length || string.IsNullOrEmpty(args[createIndex + 1]))
                    throw new ArgumentException("A name must be provided after the subcommand 'create'");
                if (!args.Contains("--path"))
                    throw new ArgumentException("The '--path' flag must be used");
                int pathIndex = argsList.IndexOf("--path");
                if (pathIndex + 1 >= args.Length || string.IsNullOrEmpty(args[pathIndex + 1]))
                    throw new ArgumentException("An argument must be provided for the '--path' flag");
                if (!args.Contains("--desc"))
                    throw new ArgumentException("The '--desc' flag must be used");
                int descIndex = argsList.IndexOf("--desc");
                if (descIndex + 1 >= args.Length || string.IsNullOrEmpty(args[descIndex + 1]))
                    throw new ArgumentException("An argument must be provided for the '--desc' flag");
                feature.Name = args[createIndex + 1];
                feature.Description = args[descIndex + 1];
                feature.Path = args[pathIndex + 1];
                return feature;
            }
            catch (ArgumentException ex)
            {
                FeatCommandUi.ShowError(ex.Message);
                return null;
            }
        }
    }
}
