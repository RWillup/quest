﻿using Quest.Arguments;

namespace Quest
{
    class Program
    {   
        static int Main(string[] args)
        {
            return ArgumentManager.HandleArgs(args);
        }
    }
}
