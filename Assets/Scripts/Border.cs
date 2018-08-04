using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace forth
{
    public class Border
    {
        private Dictionary<Vector2, BorderPoint> points = new Dictionary<Vector2, BorderPoint>();
        private Constellation parent;

        public Dictionary<Vector2, BorderPoint> Points
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
            }
        }

        public Constellation Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public class BorderPoint
        {
            public Vector2 coordinates;
            public StarSystem parentSystem;
            public Border parentBorder;
            public bool isPositioned;
            public bool isProcessed;

            public List<BorderPoint> neighbours;

            public BorderPoint(Vector2 coordinates, StarSystem parent)
            {
                this.coordinates = coordinates;
                this.parentSystem = parent;
                this.parentBorder = parent.Constellation.Border;
                this.isPositioned = false;
                this.isProcessed = false;
                parent.Constellation.Border.Points.Add(coordinates, this);
            }


            public List<BorderPoint> GetNeigbours()
            {
                List<BorderPoint> result = new List<BorderPoint>();
                foreach (Vector2 direction in Utility.directions.Values)
                {
                    Vector2 neighbour = coordinates + direction;
                    if (parentBorder.Points.ContainsKey(neighbour))
                    {
                        result.Add(parentBorder.Points[neighbour]);
                    }
                }
                return result;
            }

            public void DeletePointFromNeighbours()
            {
                foreach(BorderPoint neighbour in this.neighbours)
                {
                    neighbour.neighbours.Remove(this);
                }
            }
        }

        public Border(Constellation parent)
        {
            Parent = parent;
        }

        public void RemoveProcessed()
        {
            List<Vector2> remove = new List<Vector2>();
            foreach(KeyValuePair<Vector2, BorderPoint> point in points)
            {
                if (point.Value.isProcessed)
                    remove.Add(point.Key);
            }
            foreach(Vector2 rm in remove)
            {
                points.Remove(rm);
            }
        }

        public void ConnectWithNeighbours()
        {
            foreach(BorderPoint point in Points.Values)
            {
                point.neighbours = point.GetNeigbours();
            }
        }

        public void SmoothBorder()
        {
            bool smoothing = true;
            while (smoothing)
            {
                smoothing = false;
                List<Vector2> pointsKeys = new List<Vector2>(Points.Keys);
                for (int i = pointsKeys.Count - 1; i >= 0; i--)
                {
                    Vector2 cd = pointsKeys[i];
                    if ((isTherePoint(cd, "N") && isTherePoint(cd, "E")) ||
                       (isTherePoint(cd, "N") && isTherePoint(cd, "W")) ||
                       (isTherePoint(cd, "S") && isTherePoint(cd, "E")) ||
                       (isTherePoint(cd, "S") && isTherePoint(cd, "W")))
                    {
                        bool canBeDeleted = true;
                        foreach (BorderPoint neighbour in Points[cd].neighbours)
                        {
                            if (neighbour.neighbours.Count <= 2)
                            {
                                canBeDeleted = false;
                                break;
                            }
                        }
                        if (canBeDeleted)
                        {
                            smoothing = true;
                            Points[cd].DeletePointFromNeighbours();
                            Points.Remove(cd);
                        }
                    }
                }
                DeleteBlinds();
            }
        }

        public void DeleteBlinds()
        {
            List<Vector2> pointsKeys = new List<Vector2>(Points.Keys);
            for (int i = pointsKeys.Count - 1; i >= 0; i--)
            {
                Vector2 cd = pointsKeys[i];
                if (Points[cd].neighbours.Count > 2)
                    continue;

                if ((isTherePoint(cd, "N") && isTherePoint(cd, "NE")) ||
                       (isTherePoint(cd, "N") && isTherePoint(cd, "NW")) ||
                       (isTherePoint(cd, "E") && isTherePoint(cd, "NE")) ||
                       (isTherePoint(cd, "E") && isTherePoint(cd, "SE")) ||
                       (isTherePoint(cd, "S") && isTherePoint(cd, "SE")) ||
                       (isTherePoint(cd, "S") && isTherePoint(cd, "SW")) ||
                       (isTherePoint(cd, "W") && isTherePoint(cd, "SW")) ||
                       (isTherePoint(cd, "W") && isTherePoint(cd, "NW")) ||
                       Points[cd].neighbours.Count < 2)
                {
                    Points[cd].DeletePointFromNeighbours();
                    Points.Remove(cd);
                }
            }
        }

        public bool isTherePoint(Vector2 coordinates, string direction)
        {
            return Points.ContainsKey(coordinates + Utility.directions[direction]);
        }

        public void SortBorderPointsList()
        {
            List<BorderPoint> dictionaryCopy = new List<BorderPoint>(Points.Values);
            List<BorderPoint> sortedBorder = new List<BorderPoint>();
            sortedBorder.Add(dictionaryCopy[0]);

            bool isSorted = false;
            while(!isSorted)
            {
                BorderPoint currentPoint = sortedBorder[sortedBorder.Count - 1];
                if (currentPoint.neighbours.Count == 1)
                    break;
                foreach (BorderPoint neighbour in currentPoint.neighbours)
                {
                    if (neighbour == sortedBorder[0] && sortedBorder.Count > Points.Count / 2)
                    {
                        isSorted = true;
                        //sortedBorder.Add(neighbour);
                        break;
                    }
                    if (sortedBorder.Contains(neighbour))
                        continue;
                    else
                    {
                        sortedBorder.Add(neighbour);
                        break;
                    }
                }
                if (currentPoint == sortedBorder[sortedBorder.Count - 1])
                    break;
            }

            Dictionary<Vector2, BorderPoint> sortedPoints = new Dictionary<Vector2, BorderPoint>();
            foreach (BorderPoint sorted in sortedBorder)
            {
                if(!sortedPoints.ContainsKey(sorted.coordinates))
                    sortedPoints.Add(sorted.coordinates, sorted);
            }
            points = sortedPoints;
        }
    }
}