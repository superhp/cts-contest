using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Communication
{
    public class CompilerLanguages
    {
        public LanguageDto GetLanguages()
        {
            var languages = new LanguageDto();

            //languages.Codes.Add("c", 1);
            languages.Codes.Add("cpp", 2);
            //languages.Codes.Add("java", 43);
            //languages.Codes.Add("scala", 15);
            //languages.Codes.Add("swift", 51);
            //languages.Codes.Add("csharp", 9);
            //languages.Codes.Add("go", 21);
            //languages.Codes.Add("haskell", 12);
            //languages.Codes.Add("erlang", 16);
            //languages.Codes.Add("perl", 6);
            //languages.Codes.Add("python", 5);
            //languages.Codes.Add("python3", 30);
            //languages.Codes.Add("ruby", 8);
            //languages.Codes.Add("php", 7);
            //languages.Codes.Add("r", 24);
            //languages.Codes.Add("cobol", 36);
            //languages.Codes.Add("fsharp", 33);
            //languages.Codes.Add("d", 22);
            //languages.Codes.Add("clojure", 13);

            //languages.Names.Add("c", "C");
            languages.Names.Add("cpp", "C++");
            //languages.Names.Add("java", "Java");
            //languages.Names.Add("scala", "Scala");
            //languages.Names.Add("swift", "Swift");
            //languages.Names.Add("csharp", "C#");
            //languages.Names.Add("go", "GO");
            //languages.Names.Add("haskell", "Haskell");
            //languages.Names.Add("erlang", "Erlang");
            //languages.Names.Add("perl", "Perl");
            //languages.Names.Add("python", "Python");
            //languages.Names.Add("python3", "Python 3");
            //languages.Names.Add("ruby", "Ruby");
            //languages.Names.Add("php", "PHP");
            //languages.Names.Add("r", "R");
            //languages.Names.Add("cobol", "COBOL");
            //languages.Names.Add("fsharp", "F#");
            //languages.Names.Add("d", "D");
            //languages.Names.Add("clojure", "Clojure");

            return languages;
        }
    }
}
