using System;
using System.IO;
using System.Collections;

namespace ski_run_down_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "../4x4.txt";
            
            Map myMap = new Map(file);
            
            myMap.showMap();
            myMap.showAdjacency();
            

        }

    }
}
