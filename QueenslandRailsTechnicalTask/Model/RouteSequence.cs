using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenslandRailsTechnicalTask.Model
{
    public class RouteSequence
    {
        public List<TrainStations> Stations { get; set; }
        public bool Express { get; set; }

        public RouteSequence(List<TrainStations> stations, bool express) 
        {
            Stations = stations;
            Express = express;
        }
    }
}
