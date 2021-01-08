﻿using Quest;
using Quest.Commands;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuestTests
{
    public class DoneHandlerTests
    {
        [Fact]
        public async Task TestComplete_PassIfTodoIsInDoneFile()
        {
            string todo = "do this :";
            string expected = "* do this";
            string todoFile = "testComplete.md";
            await FileHandler.CreateTodoFileAsync(todoFile);
            DoHandler.Add(todo, todoFile);
            DoneHandler.Complete(todo, todoFile);
            string donePath = Path.Combine(Directory.GetCurrentDirectory(), "done.md");
            var actual = File.ReadAllText(donePath);
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), todoFile));
            File.Delete(donePath);
            Assert.Contains(expected, actual.Split(" :")[0]);
        }

        [Fact]
        public void TestListDone_PassIfReturns1()
        {
            int returnValue = DoneHandler.ListDone();
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task TestListDone_PassIfReturns0()
        {
            string todo = "to this";
            string todoFile = "testListDone.md";
            string doneFile = "done-testline.md";
            await FileHandler.CreateTodoFileAsync(todoFile);
            DoHandler.Add(todo, todoFile);
            DoneHandler.Complete(todo, todoFile, doneFile);
            int returnValue = DoneHandler.ListDone();
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), todoFile));
        }
    }
}
