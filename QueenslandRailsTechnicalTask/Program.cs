
using QueenslandRailsTechnicalTask.Helpers;
using QueenslandRailsTechnicalTask.Model;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {

        if (args.Length < 1)
        {
            Console.WriteLine("text file not provided");
        }
        else 
        {
            var route = CSVHelper.LoadFile(args[0]);
            var sequences = CSVHelper.AnalyzeRoute(route);
            CSVHelper.AnalyzeCollection(sequences);
        }
        Console.ReadKey();
    }
}


