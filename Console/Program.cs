using DataModel;
using System.Data.SqlClient;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Authentication auth = new Authentication();
            System.Console.WriteLine(auth.EmailIsUnique("jeroen.denotter@icloud.com"));
        }
    }
}


