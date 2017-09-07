namespace CtsContestWeb.Communication
{
    public interface ICompiler
    {
        void GetLanguages();
        void Compile(string source, string[] inputs);
    }
}