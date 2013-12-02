namespace DbUpdateApp
{
    class Program
    {
        static void Main()
        {
        }
    }


    public interface IVersion
    {
        string GetVersion();
        void SaveVersion(string version);

    }
}
