﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Quest.Commands
{
    public static class DontHandler
    {
        public static int Remove(string[] args, string todoPath = "")
        {
            if (args.Length < 2)
            {
                WriteLine("The 'dont' command requires at least one argument.");
                return 1;
            }
            string todo = args[1];
            todoPath = string.IsNullOrEmpty(todoPath) ? Path.Combine(Directory.GetCurrentDirectory(), "todo.md") : todoPath;
            if (!File.Exists(todoPath))
            {
                WriteLine("'todo.md' file not found.");
                return 1;
            }
            List<string> todoContent = File.ReadAllLines(todoPath).ToList();
            if (todoContent.Count == 0)
            {
                WriteLine("No active tasks found.");
                return 1;
            }
            todoContent.Remove(todoContent.FirstOrDefault(t => t.Contains(todo)));
            File.WriteAllLines(todoPath, todoContent);
            return 0;
        }
    }
}
