using System;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class CodeSkeletonManager : ICodeSkeletonManager
    {
        private readonly IConfiguration _iconfiguration;
        private readonly ISolutionRepository _solutionRepository;
        private readonly ICompiler _compiler;
        private readonly ITaskManager _taskManager;

        public CodeSkeletonManager(IConfiguration iconfiguration, ISolutionRepository solutionRepository, ICompiler compiler, ITaskManager taskManager)
        {
            _iconfiguration = iconfiguration;
            _solutionRepository = solutionRepository;
            _compiler = compiler;
            _taskManager = taskManager;
        }

        public async Task<CodeSkeletonDto> GetCodeSkeleton(string userEmail, int taskId, string language)
        {
            var solution = _solutionRepository.GetSolution(userEmail, taskId);

            if (solution != null)
            {
                var languages = await _compiler.GetLanguages();
                var languageCode = languages.Codes.FirstOrDefault(c => c.Value == solution.Language).Key;
                return new CodeSkeletonDto
                {
                    Language = languageCode,
                    Skeleton = solution.Source
                };
            }

            var umbracoApiUrl = _iconfiguration["UmbracoApiUrl"];
            var client = new RestClient(umbracoApiUrl);

            var request = new RestRequest("codeskeleton/get/{language}", Method.GET);
            request.AddUrlSegment("language", language);

            TaskCompletionSource<GenericCodeSkeletonDto> taskCompletion = new TaskCompletionSource<GenericCodeSkeletonDto>();
            client.ExecuteAsync<GenericCodeSkeletonDto>(request, response =>
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException("Error getting code skeleton");
                taskCompletion.SetResult(response.Data);
            });

            var codeSkeletonDto = await taskCompletion.Task;

            var task = await _taskManager.GetTaskById(taskId);

            var skeleton = GenerateCodeSkeletonForTask(task, codeSkeletonDto);

            return new CodeSkeletonDto
            {
                Language = language.ToLower(),
                Skeleton = skeleton
            };
        }

        public string GenerateCodeSkeletonForTask(TaskDto task, GenericCodeSkeletonDto genericSkeleton)
        {
            var skeleton = GenerateReadSkeleton(task, genericSkeleton);

            return skeleton;
        }

        private string GenerateReadSkeleton(TaskDto task, GenericCodeSkeletonDto genericSkeleton)
        {
            string skeleton;

            if (task.InputType.Equals("Standart"))
            {
                skeleton = AddReadLine(genericSkeleton);
            }
            else if (task.InputType.Equals("String"))
            {
                skeleton = AddReadLine(genericSkeleton);
            }
            else if (task.InputType.Equals("Line of integers"))
            {
                skeleton = AddReadLineOfIntegers(genericSkeleton);
            }
            else if (task.InputType.Equals("Integer"))
            {
                skeleton = AddReadInteger(genericSkeleton);
            }
            else if (task.InputType.Equals("Two lines (first integer, second list of integers)"))
            {
                skeleton = AddTwoLinesRead(genericSkeleton);
            }
            else if (task.InputType.Equals("First line says how many lines of integers"))
            {
                skeleton = AddMultiReadLines(genericSkeleton);
            }
            else
            {
                skeleton = AddReadLine(genericSkeleton);
            }
            return skeleton;
        }

        private string AddTwoLinesRead(GenericCodeSkeletonDto skeleton)
        {
            var readLines = skeleton.ReadInteger;
            readLines += "\n" + skeleton.ReadLineOfIntegers;
            var withReadLines = skeleton.Skeleton.Replace("{read}", readLines);

            return withReadLines;
        }

        private string AddReadInteger(GenericCodeSkeletonDto skeleton)
        {
            var readLines = skeleton.ReadInteger;
            var withReadLines = skeleton.Skeleton.Replace("{read}", readLines);

            return withReadLines;
        }

        private string AddMultiReadLines(GenericCodeSkeletonDto skeleton)
        {
            var readLines = skeleton.ReadInteger;
            readLines += "\n" + skeleton.ReadInputIntegerNumberOfLinesOfIntegers;

            var withReadLines = skeleton.Skeleton.Replace("{read}", readLines);

            return withReadLines;
        }

        private string AddReadLineOfIntegers(GenericCodeSkeletonDto skeleton)
        {
            var readLines = skeleton.ReadLineOfIntegers;
            var withReadLines = skeleton.Skeleton.Replace("{read}", readLines);

            return withReadLines;
        }

        private string AddReadLine(GenericCodeSkeletonDto skeleton)
        {
            var readLines = skeleton.ReadLine;
            var withReadLines = skeleton.Skeleton.Replace("{read}", readLines);

            return withReadLines;
        }
    }
}
