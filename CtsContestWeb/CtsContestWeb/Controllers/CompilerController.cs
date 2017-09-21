using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtsContestWeb.Communication;
using CtsContestWeb.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class CompilerController : Controller
    {
        public ICompiler Compiler { get; }

        public CompilerController(ICompiler compiler)
        {
            Compiler = compiler;
        }

        [HttpGet("[action]")]
        public async Task<LanguageDto> GetLanguages()
        {
            var languages = await Compiler.GetLanguages();

            return languages;
        }
    }
}
