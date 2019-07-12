using System;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Db;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace CtsContestWeb.Communication
{
    public class CodeSkeletonRepository : ICodeSkeletonRepository
    {
        private readonly IConfiguration _iconfiguration;
        private readonly ISolutionRepository _solutionRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ApplicationDbContext _dbContext;

        public CodeSkeletonRepository(IConfiguration iconfiguration, ISolutionRepository solutionRepository, ITaskRepository taskRepository,
            ApplicationDbContext dbContext)
        {
            _iconfiguration = iconfiguration;
            _solutionRepository = solutionRepository;
            _taskRepository = taskRepository;
            _dbContext = dbContext;
        }

        public async Task<CodeSkeletonDto> GetCodeSkeleton(LanguageDto languages, string userEmail, int taskId, string language)
        {
            if (userEmail != null)
            {
                var solution = _solutionRepository.GetSolution(userEmail, taskId);

                if (solution != null)
                {
                    var languageCode = languages.Codes.FirstOrDefault(c => c.Value == solution.Language).Key;

                    if (languageCode.Equals(language.ToLower()) || language.Equals("undefined"))
                        return new CodeSkeletonDto
                        {
                            Language = languageCode.Replace(" ", string.Empty),
                            Skeleton = solution.Source
                        };
                }
            }

            if (language.Equals("undefined"))
                language = "Java";

            var codeSkeletonEntity = _dbContext.CodeSkeletons.FirstOrDefault(x => x.Language.Equals(language, StringComparison.CurrentCultureIgnoreCase));
            var genericCodeSkeleton = new GenericCodeSkeletonDto();
            if (codeSkeletonEntity == null)
            {
                genericCodeSkeleton.Language = language;
                genericCodeSkeleton.Skeleton = string.Empty;
            }
            else
            {
                genericCodeSkeleton.Language = language;
                genericCodeSkeleton.Skeleton = codeSkeletonEntity.Skeleton;
                genericCodeSkeleton.ReadInputIntegerNumberOfLinesOfIntegers = codeSkeletonEntity.ReadInputIntegerNumberOfLinesOfIntegers.Replace("\\n", "\n");
                genericCodeSkeleton.ReadLine = codeSkeletonEntity.ReadLine.Replace("\\n", "\n");
                genericCodeSkeleton.ReadInteger = codeSkeletonEntity.ReadInteger.Replace("\\n", "\n");
                genericCodeSkeleton.WriteLine = codeSkeletonEntity.WriteLine;
                genericCodeSkeleton.ReadLineOfIntegers = codeSkeletonEntity.ReadLineOfIntegers.Replace("\\n", "\n");
            }

            var task = await _taskRepository.GetTaskInputType(taskId);

            var skeleton = GenerateReadSkeleton(task, genericCodeSkeleton);

            return new CodeSkeletonDto
            {
                Language = language.ToLower().Replace(" ", string.Empty),
                Skeleton = skeleton.Replace("\\\n", "\\n")
            };
        }

        private string GenerateReadSkeleton(string taskInputType, GenericCodeSkeletonDto genericSkeleton)
        {
            string skeleton;

            if (taskInputType.Equals("Standart"))
            {
                skeleton = AddReadLine(genericSkeleton);
            }
            else if (taskInputType.Equals("String"))
            {
                skeleton = AddReadLine(genericSkeleton);
            }
            else if (taskInputType.Equals("Line of integers"))
            {
                skeleton = AddReadLineOfIntegers(genericSkeleton);
            }
            else if (taskInputType.Equals("Integer"))
            {
                skeleton = AddReadInteger(genericSkeleton);
            }
            else if (taskInputType.Equals("Two lines (first integer, second list of integers)"))
            {
                skeleton = AddTwoLinesRead(genericSkeleton);
            }
            else if (taskInputType.Equals("First line says how many lines of integers"))
            {
                skeleton = AddMultiReadLines(genericSkeleton);
            }
            else
            {
                skeleton = genericSkeleton.Skeleton.Replace("{read}", "");
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
