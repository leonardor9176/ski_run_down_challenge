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
        private List<int>[] adjacencyTable;
        private int[] heights;
        private List<int> finalPoints = new List<int>();
        public bool finishAtZero = true; // Allows you to choose 0 as the possible value of the final height
        private int[] totSteps;
        private int[] previousPoint;
        private List<int> topPoints = new List<int>();
        private List<int> finalRoute = new List<int>();

        public Map(string file)
        {
            StreamReader read = new StreamReader(@file);
            string lineRead = read.ReadLine();
            int[] currentLine = lineToInt(lineRead);

            this.n = currentLine[0];
            this.m = currentLine[0];
            this.adjacencyTable = new List<int>[n * m];
            this.heights = new int[n * m];
            this.totSteps = new int[n * m];
            this.previousPoint = new int[n * m];

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
                    this.adjacencyTable[(linesRead * this.n) + i] = new List<int>();
                    this.heights[(linesRead * this.n) + i] = currentLine[i];
                    if (linesRead == 0 || i == 0 || i == (currentLine.Length - 1) || linesRead == (this.n - 1))
                    {
                        if (currentLine[i] == 1)
                        {
                            finalPoints.Add((linesRead * this.n) + i);
                        }
                        if (this.finishAtZero)
                        {
                            if (currentLine[i] == 0)
                            {
                                finalPoints.Add((linesRead * this.n) + i);
                            }
                        }
                    }

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

        private void iterateAdjacents(List<int> initials, int[] totSteps, int[] previousPoint, List<int> topPoints, ref int maxSteps)
        {
            foreach (var node1 in initials)
            {
                if (totSteps[node1] == 0)
                {
                    totSteps[node1]++;
                }

                if (maxSteps < totSteps[node1])
                {
                    maxSteps = totSteps[node1];
                    topPoints.Clear();
                    topPoints.Add(node1);
                }
                else if (maxSteps == totSteps[node1])
                {
                    if (!topPoints.Contains(node1))
                    {
                        topPoints.Add(node1);
                    }
                }

                if (this.adjacencyTable[node1].Count != 0)
                {
                    foreach (var node2 in this.adjacencyTable[node1])
                    {
                        if (totSteps[node2] < (totSteps[node1] + 1))
                        {
                            previousPoint[node2] = node1;
                            totSteps[node2] = totSteps[node1] + 1;
                        }
                        iterateAdjacents(this.adjacencyTable[node1], this.totSteps, this.previousPoint, this.topPoints, ref maxSteps);
                    }
                }
            }
        }
        private void planRoute()
        {
            int maxSteps = 0;
            iterateAdjacents(this.finalPoints, this.totSteps, this.previousPoint, this.topPoints, ref maxSteps);

            createRoute();
        }
        private void createRoute()
        {
            List<int> route = new List<int>();
            int next;
            int bestDistance = 0;
            for (int i = 0; i < this.topPoints.Count; i++)
            {
                next = this.previousPoint[this.topPoints[i]];
                route.Add(this.topPoints[i]);
                while (next != 0)
                {
                    route.Add(next);
                    next = this.previousPoint[next];
                }
                if (bestDistance < (this.heights[route[0]] - this.heights[route[route.Count - 1]]))
                {
                    this.finalRoute.Clear();
                    foreach (var item in route)
                    {
                        this.finalRoute.Add(item);
                    }
                    bestDistance = this.heights[route[0]] - this.heights[route[route.Count - 1]];
                }
                route.Clear();
            }
        }

        public void setFinishAtZero(bool finishAtZero)
        {
            this.finishAtZero = finishAtZero;
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
            return this.adjacencyTable;
        }
        public List<int> getRoute()
        {
            List<int> route = new List<int>();
            for (int i = 0; i < this.finalRoute.Count; i++)
            {
                route.Add(this.heights[this.finalRoute[i]]);
            }

            return route;
        }
    }
}