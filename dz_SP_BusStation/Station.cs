using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dz_SP_BusStation
{
    public class Station
    {
        public int CountPeople { get; set; }

        public int AddPeaple()
        {
            int count = new Random().Next(0, 25);
            CountPeople += count;
            return count;
            
        }
    }
}
