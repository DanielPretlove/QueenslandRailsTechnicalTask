using CsvHelper;
using QueenslandRailsTechnicalTask.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenslandRailsTechnicalTask.Helpers
{
    public class CSVHelper
    {
        public static List<TrainStations> LoadFile(string path)
        {
            using var reader = new StreamReader(path);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<TrainStations>().ToList();

            for(int i = 0; i < records.Count; i++)
            {
                Console.WriteLine(records[i].Name + ", " + records[i].Stop);
            }
            Console.WriteLine("\n");

            return records;
        }

        public static List<RouteSequence> AnalyzeCollection(List<RouteSequence> records)
        {
            List<TrainStations> allStations = new List<TrainStations>();
            foreach (RouteSequence routeSequence in records)
            {
                allStations.AddRange(routeSequence.Stations);
            }

            string firstStation = allStations.First(x => x.Stop == true).Name;
            string lastStation = allStations.Last(x => x.Stop == true).Name;

            var firstExpress = records.Where(x => x.Express == true).First().Express == true;
            var firstExpressStations = records.Where(x => x.Express == true).First().Stations.Any(x => x.Stop == true) == true;

            if (records.All(n => n.Express == false))
            {
                Console.WriteLine("This train stops at all stations");
            }

            else if (allStations.Count(x => x.Stop == true) == 2)
            {
                Console.WriteLine($"This train stops at {firstStation} and {lastStation} only");
            }

            else if (allStations.Count(x => x.Stop == false) == 1)
            {
                Console.WriteLine($"This train stops at all stations except {allStations.FirstOrDefault(s => s.Stop == false).Name}");
            }

            // checks if the first express and the second express that returns true and the checks if any stops within the true express
            // station are true for both the first and second true express conditions
            else if (firstExpress == true
                && records.Where(x => x.Express == true).Skip(1).FirstOrDefault() != null 
                && firstExpressStations == true 
                && records.Where(x => x.Express == true).Skip(1).FirstOrDefault().Stations.Any(x => x.Stop == true) != null)
            {
                Console.WriteLine($"This train runs express from {records.Where(x => x.Express == true).First().Stations.First().Name} " +
                    $"to {records.Where(x => x.Express == true).First().Stations.Last().Name}, stopping only at " +
                    $"{records.Where(x => x.Express == true).First().Stations.Where(x => x.Stop == true).Skip(1).First().Name} then runs express from " +
                    $"{records.Where(x => x.Express == true).ElementAt(1).Stations.First().Name} to " +
                    $"{records.Where(x => x.Express == true).ElementAt(1).Stations.Last().Name}");
            }

            else if (firstExpress == true && firstExpressStations == true && records.Where(x => x.Express == true).First().Stations.Count(x => x.Stop == true) > 2)
            {
                Console.WriteLine($"This train runs express from {records.Where(x => x.Express == true).First().Stations.First().Name} " +
                    $"to {records.Where(x => x.Express == true).First().Stations.Last().Name} stopping only at" +
                    $" {records.Where(x => x.Express == true).First().Stations.Where(x => x.Stop == true).Skip(1).First().Name}");
            }
            else if (firstExpress == true)
            {
                Console.WriteLine($"This train runs express from" +
                    $" {records.Where(x => x.Express == true).First().Stations.First().Name}" +
                    $" to {records.Where(x => x.Express == true).First().Stations.Last().Name}");
            }

            else
            {
                Console.WriteLine("none are the conditions doe not meet the requirements");
            }
           
            return records;
        }

        public static List<RouteSequence> AnalyzeRoute(List<TrainStations> stations)
        {
            TrainStations currentStation;
            TrainStations? previousStation = null;
            TrainStations? nextStation = null;
            List<TrainStations> currentSequence = new();
            List<RouteSequence> routeSequences = new();

            for (int i = 0; i < stations.Count; i++)
            {
                currentStation = stations[i];
                if (i > 0)
                    previousStation = stations[i - 1];
                if (i < stations.Count - 1)
                    nextStation = stations[i + 1];
                else
                    nextStation = null;

                // When at start of route, or 2 adjacent stations are Stops, this marks the end of an "express sequence"
                // When current Station is a stop but next one is express, this marks the start of an "express sequence"
                if (((previousStation == null || previousStation.Stop) && currentStation.Stop && currentSequence.Any(x => x.Stop == false)) ||
                   (!currentSequence.Any(x => x.Stop == false) && nextStation != null && !nextStation.Stop && currentStation.Stop))
                {
                    routeSequences.Add(new RouteSequence(currentSequence, currentSequence.Any(x => x.Stop == false)));
                    currentSequence = new();
                }

                currentSequence.Add(currentStation);

                if (nextStation == null)
                    routeSequences.Add(new RouteSequence(currentSequence, currentSequence.Any(x => x.Stop == false)));
            }
            return routeSequences;
        }
    }
}
