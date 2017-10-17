using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Logic
{
    public interface ISolutionLogic
    {
        Task<CompileDto> CheckSolution(int taskId, string source, int language, string userEmail);
        void SaveSolution(TaskDto task, string source, string userEmail, int language, bool isCorrect = true);
    }
}
