using System;
using System.IO;
using System.Collections.Generic;
// using System.Collections;

namespace ski_run_down_challenge
{
    public class Map
    {
        private int n;
        private int m;
        private List<int>[] adjacencyList;
        private int[] heights;
        // private List<int> finalPoints = new List<int>();
        // private int[] totSteps;
        // private int[] previousPoint;
        // private List<int> topPoints = new List<int>();
        List<int>[] longestPaths = new List<int>[1];

        public Map(string file)
        {
            StreamReader read = new StreamReader(@file);
            string lineRead = read.ReadLine();
            int[] currentLine = lineToInt(lineRead);

            this.n = currentLine[0];
            this.m = currentLine[0];
            this.adjacencyList = new List<int>[n * m];
            this.heights = new int[n * m];
            // this.totSteps = new int[n * m];
            // this.previousPoint = new int[n * m];

            int[] prevLine = new int[m];
            int[] nextLine = new int[m];

            int linesRead = 0;
            int[] minHeightByEdge = new int[4];

            while (linesRead < this.n)
            {
                if (linesRead == 0)
                {
                    lineRead = read.ReadLine();
                    currentLine = lineToInt(lineRead);
                    lineRead = read.ReadLine();
                    nextLine = lineToInt(lineRead);

                    minHeightByEdge[0] = currentLine[0];
                    minHeightByEdge[1] = currentLine[0];
                    minHeightByEdge[2] = currentLine[currentLine.Length - 1];
                }
                else if (linesRead == (this.n - 1))
                {
                    prevLine = currentLine;
                    currentLine = nextLine;

                    minHeightByEdge[3] = currentLine[0];
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
                    this.adjacencyList[(linesRead * this.n) + i] = new List<int>();
                    this.heights[(linesRead * this.n) + i] = currentLine[i];

                    // Not for first line
                    if (linesRead > 0)
                    {
                        if (currentLine[i] > prevLine[i])
                        {
                            this.adjacencyList[(linesRead * this.n) + i].Add(((linesRead - 1) * this.n) + (i));
                        }
                    }

                    // All the lines
                    if (i > 0)
                    {
                        if (currentLine[i] > currentLine[i - 1])
                        {
                            this.adjacencyList[(linesRead * this.n) + i].Add((linesRead * this.n) + (i - 1));
                        }
                    }
                    if (i < (currentLine.Length - 1))
                    {
                        if (currentLine[i] > currentLine[i + 1])
                        {
                            this.adjacencyList[(linesRead * this.n) + i].Add((linesRead * this.n) + (i + 1));
                        }
                    }

                    // Not for final line
                    if (linesRead < (this.n - 1))
                    {
                        if (currentLine[i] > nextLine[i])
                        {
                            this.adjacencyList[(linesRead * this.n) + i].Add(((linesRead + 1) * this.n) + (i));
                        }
                    }
                }
                linesRead++;
            }
            read.Close();
            planRoute();
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

        private void planRoute()
        {
            int[] totSteps = new int[n * m];
            int[] previousNode = new int[n * m];
            List<int> currentPath = new List<int>();
            this.longestPaths[0] = new List<int>();

            Array.Fill<int>(previousNode, -1);

            for (int i = 0; i < adjacencyList.Length; i++)
            {
                currentPath.Clear();
                if (previousNode[i] == -1)
                {
                    iterateAdjacents(i, ref totSteps, ref previousNode, ref currentPath);
                }
            }
        }

        private void iterateAdjacents(int node1, ref int[] totSteps, ref int[] previousNode, ref List<int> currentPath)
        {
            totSteps[node1]++;
            currentPath.Add(node1);

            if (this.adjacencyList[node1].Count != 0)
            {
                foreach (var node2 in this.adjacencyList[node1])
                {
                    if (previousNode[node2] == -1)
                    {
                        previousNode[node2] = node1;
                        totSteps[node2] = totSteps[node1];
                        iterateAdjacents(node2, ref totSteps, ref previousNode, ref currentPath);
                    }
                    else if (totSteps[node2] < (totSteps[node1] + 1))
                    {
                        previousNode[node2] = node1;
                        totSteps[node2] = totSteps[node1];
                        iterateAdjacents(node2, ref totSteps, ref previousNode, ref currentPath);

                    }
                    else if (totSteps[node2] == (totSteps[node1] + 1))
                    {
                        if (this.heights[previousNode[node2]] <= this.heights[node1])
                        {
                            previousNode[node2] = node1;
                            totSteps[node2] = totSteps[node1];
                            iterateAdjacents(node2, ref totSteps, ref previousNode, ref currentPath);
                        }
                    }
                }
            }

            if (this.longestPaths[0].Count < currentPath.Count)
            {
                Array.Clear(this.longestPaths, 0, this.longestPaths.Length);
                Array.Resize(ref this.longestPaths, 1);
                this.longestPaths[0] = new List<int>();
                this.longestPaths[0].AddRange(currentPath);
            }
            else if (this.longestPaths[0].Count == currentPath.Count)
            {
                int downhillCurrent = this.heights[currentPath[0]] - this.heights[currentPath[currentPath.Count - 1]];
                int downhillLonguest = this.heights[this.longestPaths[0][0]] - this.heights[this.longestPaths[0][this.longestPaths[0].Count - 1]];

                if (downhillCurrent == downhillLonguest)
                {
                    Array.Resize(ref this.longestPaths, this.longestPaths.Length + 1);
                    this.longestPaths[this.longestPaths.Length - 1] = new List<int>();
                    this.longestPaths[this.longestPaths.Length - 1].AddRange(currentPath);
                }
                if (downhillCurrent > downhillLonguest)
                {
                    Array.Clear(this.longestPaths, 0, this.longestPaths.Length);
                    Array.Resize(ref this.longestPaths, 1);
                    this.longestPaths[0] = new List<int>();
                    this.longestPaths[0].AddRange(currentPath);
                }
            }
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        public int[] getDimentions()
        {
            int[] dimentions = { this.n, this.m };
            return dimentions;
        }
        public int[,] exportHeightsMap()
        {
            int[,] map = new int[this.n, this.m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    map[i, j] = heights[(i * this.n) + j];
                }
            }
            return map;
        }
        public List<int>[] getAdjacencyTable()
        {
            return this.adjacencyList;
        }
        public List<int>[] getRoute()
        {
            List<int>[] routes = new List<int>[this.longestPaths.Length];

            for (int i = 0; i < this.longestPaths.Length; i++)
            {
                routes[i] = new List<int>();
                for (int j = 0; j < this.longestPaths[i].Count; j++)
                {
                    routes[i].Add(this.heights[this.longestPaths[i][j]]);
                }
            }
            return routes;
        }
    }
}