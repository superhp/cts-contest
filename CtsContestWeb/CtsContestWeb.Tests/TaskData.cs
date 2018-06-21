using System.Collections.Generic;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Tests
{
    public static class TaskData
    {
        public static TaskDto GetTestTask()
        {
            return new TaskDto
            {
                Inputs = new List<string>
                {
                    "1",
                    "2"
                },
                Outputs = new List<string>
                {
                    "2", "2"
                }
            };
        }
    }
}