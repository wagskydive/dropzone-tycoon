using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsLogic
{
    public class StatsLibrary
    {
        List<string> statNames;

        public StatsLibrary()
        {
            statNames.Add("Health");
            statNames.Add("Stamina");
            statNames.Add("Meelee Damage");
            statNames.Add("Ranges Damage");
            statNames.Add("Defense");
        }

    }
}
