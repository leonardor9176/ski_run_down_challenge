using System;
using System.IO;
using System.Collections.Generic;

namespace ski_run_down_challenge
{
    public class Map
    {
        private int n;
        private int m;
        private List<int>[] adjacencyTable;
        private int[] heights;

        public bool finishAtZero = true; // Allows you to choose 0 as the possible value of the final height
        // private int[] start = new int[2];

        public Map(string file)
        {
            StreamReader read = new StreamReader(@file);
            string lineRead = read.ReadLine();
            int[] currentLine = lineToInt(lineRead);

            this.n = currentLine[0];
            this.m = currentLine[0];
            this.adjacencyTable = new List<int>[n * m];
            this.heights = new int[n * m];

            int[] prevLine = new int[m];
            int[] nextLine = new int[m];

            int linesRead = 0;

            while (linesRead < this.n)
            {
                if (linesRead == 0)
                {
                    lineRead = read.ReadLine();
                    currentLine = lineToInt(lineRead);
                    lineRead = read.ReadLine();
                    nextLine = lineToInt(lineRead);
                }
                else if (linesRead == (this.n - 1))
                {
                    prevLine = currentLine;
                    currentLine = nextLine;
                }
                else
                {
                    prevLine = currentLine;
                    currentLine = nextLine;
                    lineRead = read.ReadLine();
                    nextLine = lineToInt(lineRead);
                }

                for (int i = 0; i < currentLine.Length; i++)
                {
                    this.adjacencyTable[(linesRead * this.n) + i] = new List<int>();
                    this.heights[(linesRead * this.n) + i] = currentLine[i];

                    // Not for first line
                    if (linesRead > 0)
                    {
                        if (currentLine[i] < prevLine[i])
                        {
                            this.adjacencyTable[(linesRead * this.n) + i].Add(((linesRead - 1) * this.n) + (i));
                        }
                    }

                    // All the lines
                    if (i > 0)
                    {
                        if (currentLine[i] < currentLine[i - 1])
                        {
                            this.adjacencyTable[(linesRead * this.n) + i].Add((linesRead * this.n) + (i - 1));
                        }
                    }
                    if (i < (currentLine.Length - 1))
                    {
                        if (currentLine[i] < currentLine[i + 1])
                        {
                            this.adjacencyTable[(linesRead * this.n) + i].Add((linesRead * this.n) + (i + 1));
                        }
                    }

                    // Not for final line
                    if (linesRead < (this.n - 1))
                    {
                        if (currentLine[i] < nextLine[i])
                        {
                            this.adjacencyTable[(linesRead * this.n) + i].Add(((linesRead + 1) * this.n) + (i));
                        }
                    }
                }
                linesRead++;
            }
            read.Close();
        }

        private int[] lineToInt(string line)
        {
            string[] lineString = line.Split(' ');
            int[] lineInt = new int[lineString.Length];
            for (int i = 0; i < lineString.Length; i++)
            {
                lineInt[i] = Int32.Parse(lineString[i]);
            }
            return lineInt;
        }
        public void showAdjacency()
        {
            for (int i = 0; i < adjacencyTable.Length; i++)
            {
                Console.Write("node " + i + ": ");
                for (int j = 0; j < adjacencyTable[i].Count; j++)
                {
                    Console.Write(adjacencyTable[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void showMap()
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.m; j++)
                {
                    Console.Write(this.heights[(i * this.n) + j] + " ");
                }
                Console.WriteLine();

            }
            Console.WriteLine();
        }
    }
}
