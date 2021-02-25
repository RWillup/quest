﻿using static System.Console;

namespace Quest.IO.StdOut
{
    public static class ConfigCreationDialog
    {
        public static bool GetUserConfirmation()
        {
            string answer;
            do
            {                
                WriteLine("Quest's global config not found.");
                WriteLine("Would you like to create it now?");
                WriteLine("[Y] Yes\n[N] No");
                answer = ReadLine();
                answer = answer.ToLower();
            } while (answer != "yes" && answer != "y" && answer != "n" && answer != "no");
            if (answer == "no" || answer == "n")
                return false;
            return true;
        }

        public static string GetUsername()
        {
            WriteLine("Please, what is your GitHub username?");
            return ReadLine();
        }
    }
}
