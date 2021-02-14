﻿using Quest.Commands;
using System;
using System.Threading.Tasks;

namespace Quest
{
    public static class CommandHandler
    {
        public static async Task<bool> RunAsync(string[] args, string command)
        {
            if (command == "do")
                return await Do.HandleAsync(args);
            else if (command == "done")
                return await Done.HandleAsync(args);
            else if (command == "todo")
                return ToDo.List(ToDo.Handle(args)) == 0;
            else if (command == "undo")
                return Undo.Handle(args);
            else if (command == "dont")
                return await Dont.HandleAsync(args);
            else if (command == "version")
                Console.WriteLine("1.0.0");
            else if (command == "help")
                Help.HandleHelp(args);
            else if (command == "config")
                ConfigHandler.Handle(args);
            else
                Console.WriteLine($"Command \"{args[0]}\" is undefined.");
            return true;
        }
    }
}
