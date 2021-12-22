using System;
using System.Collections;
using System.Collections.Generic;

namespace ski_run_down_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set test = false to see results for matrix 1000x100 in file map.txt
            bool test = false;
            string file;
            if (test)
            {
                file = "../4x4.txt";
            }
            else
            {
                file = "../map.txt";
            }

            Map myMap = new Map(file);

            //Show Dimentions
            showDimentions(myMap);

            //Show map
            // ShowMap(myMap);

            //Show adjasency table
            // showAdjacencyTable(myMap);

            //Route
            showResults(myMap);

        }
        static void showDimentions(Map myMap)
        {
            Console.WriteLine();
            Console.WriteLine("Dimentions");
            Console.WriteLine("[{0}]", string.Join(", ", myMap.getDimentions()));
        }
        static void ShowMap(Map myMap)
        {
            Console.WriteLine();
            Console.WriteLine("Map of Heights");
            int[,] heightsMap = myMap.exportHeightsMap();
            for (int i = 0; i < heightsMap.GetLength(0); i++)
            {
                for (int j = 0; j < heightsMap.GetLength(1); j++)
                {
                    Console.Write(heightsMap[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void showAdjacencyTable(Map myMap)
        {
            List<int>[] adjacencyTable;
            adjacencyTable = myMap.getAdjacencyTable();
            Console.WriteLine();
            Console.WriteLine("Adjacenci Nodes");
            for (int i = 0; i < adjacencyTable.Length; i++)
            {
                Console.Write(i + ": ");
                foreach (var item in adjacencyTable[i])
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
            }
        }
        static void showResults(Map myMap)
        {
            List<int>[] routes;
            routes = myMap.getRoute();

            Console.WriteLine();
            Console.WriteLine("TopPnt\tBotPnt\tSteps\tDist\tRoute");
            foreach (var path in routes)
            {
                int distance = path[0] - path[path.Count - 1];

                Console.Write(path[0] + "\t");
                Console.Write(path[path.Count - 1] + "\t");
                Console.Write(path.Count + "\t");
                Console.Write(distance + "\t");

                foreach (var item in path)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
