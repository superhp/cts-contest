using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Dto;
using Microsoft.Extensions.Configuration;

namespace CtsContestWeb.Communication
{
    public class PuzzleManager : IPuzzleManager
    {
        private readonly IConfiguration _configuration;
        private readonly List<PuzzleDto> _puzzles;
        private readonly string _puzzleBaseUrlPattern;

        public PuzzleManager(IConfiguration configuration)
        {
            _puzzleBaseUrlPattern = configuration["PuzzleBaseUrlPattern"];
            _puzzles = configuration.GetSection("Puzzles")
                                    .GetChildren()
                                    .Select(child => new PuzzleDto()
                                    {
                                        Identifier = child.GetValue<string>("Id"),
                                        Value = child.GetValue<int>("Value")
                                    })
                                    .ToList();
        }

        public async Task<PuzzleDto> GetPuzzle(int id)
        {
            if (id < 0 || id >= _puzzles.Count()) throw new IndexOutOfRangeException("No puzzle with the specified ID exists");
            return await Task.Run(() => EnrichPuzzleDto(_puzzles[id], id));
        }

        public async Task<IEnumerable<PuzzleDto>> GetPuzzles(string userEmail = null)
        {
            // TODO: mark IsSolved with DB call
            return await Task.Run(() => _puzzles.Select(EnrichPuzzleDto));
        }

        private PuzzleDto EnrichPuzzleDto(PuzzleDto puzzle, int index)
        {
            return new PuzzleDto()
            {
                Id = index,
                Identifier = puzzle.Identifier,
                Value = puzzle.Value,
                BaseUrl = string.Format(_puzzleBaseUrlPattern, puzzle.Identifier)
            };
        }
    }
}
