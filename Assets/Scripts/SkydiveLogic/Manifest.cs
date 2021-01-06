using System.Collections.Generic;
using System.Linq;

namespace SkydiveLogic
{
    public class Manifest
    {
        List<Aircraft> aircrafts;

        Queue<Load> AvailableLoads = new Queue<Load>();
    }
}
