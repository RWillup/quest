﻿using Quest.Commands;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace QuestTests.CommandsTests
{
    public class ListTasksTests
    {
        private readonly string donePath;
        public ListTasksTests()
        {
            donePath = Path.Combine(Environment.CurrentDirectory, ".quest", "listtasks-tests", "todo.md");
            Environment.SetEnvironmentVariable("QUEST_PATH", Environment.CurrentDirectory);            
        }

        [Fact]
        public async Task TestHandle_PassIfReturnsTrue()
        {
            string[] doArgs = { "do", "TestHandle_PassIfReturnsTrue", "-a", "unit-test", "-f", "listtasks-tests" };
            _ = await Do.HandleAsync(doArgs);
            doArgs[0] = "done";
            _ = await Done.HandleAsync(doArgs);
            string[] args = { "done" };
            bool success = ListTasks.Handle(args, true);
            Assert.True(success);
        }                
    }
}
