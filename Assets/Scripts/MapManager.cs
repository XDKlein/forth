﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace forth {
    [Serializable]
    public class MapManager {
        public MapType map;

        private List<StarSystem> starSystems = new List<StarSystem>();
        private List<StarSystemConnection> starSystemConnections = new List<StarSystemConnection>();
        private List<Constellation> constellations = new List<Constellation>();

        public List<StarSystem> StarSystems
        {
            get
            {
                return starSystems;
            }

            set
            {
                starSystems = value;
            }
        }

        public List<StarSystemConnection> StarSystemConnections
        {
            get
            {
                return starSystemConnections;
            }

            set
            {
                starSystemConnections = value;
            }
        }

        public List<Constellation> Constellations
        {
            get
            {
                return constellations;
            }

            set
            {
                constellations = value;
            }
        }

        ///<summary>
        ///This is a description of my function.
        ///</summary>
        public void GenerateMap()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            PlaceStarSystems();
            watch.Stop();
            Debug.Log("Place Star Systems: " + watch.ElapsedMilliseconds / 1000);

            watch = System.Diagnostics.Stopwatch.StartNew();
            ConnectStarSystems();
            watch.Stop();
            Debug.Log("Connect Star Systems: " + watch.ElapsedMilliseconds / 1000);

            watch = System.Diagnostics.Stopwatch.StartNew();
            CreateConstellations();
            watch.Stop();
            Debug.Log("Create Constellations: " + watch.ElapsedMilliseconds / 1000);

            watch = System.Diagnostics.Stopwatch.StartNew();
            DrawConstellationsBorders();
            watch.Stop();
            Debug.Log("Draw Constellations Borders: " + watch.ElapsedMilliseconds / 1000);
        }

        void PlaceStarSystems()
        {
            List<Vector2> positions = new List<Vector2>();
            int starSystemCount = (int)UnityEngine.Random.Range(map.minSystems, map.maxSystems);

            for (int count = 0; count < starSystemCount;)
            {
                Vector2 position = new Vector2((int)UnityEngine.Random.Range((map.size.x / 2 - 2) * -1f, (map.size.x / 2 - 2)),
                                               (int)UnityEngine.Random.Range((map.size.y / 2 - 2) * -1f, (map.size.x / 2 - 2)));
                if (!isPositionSuitable(positions, position))
                    continue;

                if (UnityEngine.Random.Range(0f, 1f) >= GetTemplateGreyscale(position.x, position.y))
                    continue;

                positions.Add(position);
                StarSystem system = CreateStarSystem(position);
                StarSystems.Add(system);
                count++;
            }
        }

        StarSystem CreateStarSystem(Vector2 position)
        {
            SystemType systemType = map.systemTypes.Find(GetSystemType).systemType;
            if (systemType == null)
                systemType = map.systemTypes.Find((x) => x.systemType.useDefault).systemType;

            GameObject systemObject = systemType.gameObjects.Find((x) => UnityEngine.Random.Range(0f, 1f) <= x.probability).gameObject;
            if (systemObject == null)
                systemObject = systemType.gameObjects[0].gameObject;
            systemObject = GameObject.Instantiate(systemObject);
            float size = UnityEngine.Random.Range(systemType.minSizeMultiplier, systemType.maxSizeMultiplier);
            systemObject.transform.GetChild(0).localScale = new Vector3(size, 1, size);

            string systemName = (starSystems.Count == 0 && systemType.names.Count > 0) ? systemType.names[0] : "System";
            foreach (StarSystem system in starSystems)
            {
                systemName = systemType.names.Find((x) => (x != system.Name));
                if (systemName == null)
                    systemName = "System"; //TODO: replace with default names
            }

            List<Planet> planets = new List<Planet>();
            for(int i = 0; i < UnityEngine.Random.Range(systemType.minPlanets, systemType.maxPlanets); i++)
            {
                PlanetType planet = systemType.planets.Find((x) => UnityEngine.Random.Range(0f, 1f) <= x.probability).planet;
                if (planet == null)
                    planet = systemType.planets[0].planet;
                planets.Add(new Planet(planet.name));
            }

            return new StarSystem(systemName, systemObject, planets, position);
        }

        bool GetSystemType(SystemTypeAndProbability systemTypes)
        {
            if(UnityEngine.Random.Range(0f, 1f) <= systemTypes.probability)
            {
                if(!systemTypes.systemType.useDefault)
                    foreach(StarSystem system in starSystems)
                    {
                        if (systemTypes.systemType.names.Contains(system.Name))
                            return false;
                    }
                return true;
            }
            return false;
        }

        float GetTemplateGreyscale(float x, float y)
        {
            float mapToTextureRatio = map.size.x / map.template.width;
            return map.template.GetPixel((int)((x + map.size.x / 2) / mapToTextureRatio), (int)((y + map.size.y / 2) / mapToTextureRatio)).grayscale;
        }

        void ConnectStarSystems()
        {
            foreach(StarSystem origin in StarSystems)
            {
                StarSystemConnection systemConnection = origin.ConnectToClosest(StarSystems);
                if (systemConnection != null)
                    StarSystemConnections.Add(systemConnection);
            }
            //connectBlindSystems();
        }

        void connectBlindSystems()
        {
            foreach(StarSystem blindSystem in StarSystems)
            {
                if(blindSystem.Connections.Count == 1)
                {
                    StarSystemConnection systemConnection = blindSystem.ConnectToClosest(StarSystems);
                    if (systemConnection != null)
                        StarSystemConnections.Add(systemConnection);
                }
            }
        }

        void CreateConstellations()
        {
            List<StarSystem> systems = new List<StarSystem>(starSystems);
            for (int i = 0; i < systems.Count;)
            {
                List<StarSystem> constellation = GetConstellation(systems[i], new List<StarSystem>());
                foreach(StarSystem constellationSystem in constellation)
                {
                    if(systems.Contains(constellationSystem))
                        systems.Remove(constellationSystem);
                }
                constellations.Add(new Constellation("Constellation " + i, constellation));
            }
        }

        List<StarSystem> GetConstellation(StarSystem system, List<StarSystem> systems)
        {
            systems.Add(system);
            List<StarSystem> systemNeighbours = system.GetConnectedNeighbours();
            if (systemNeighbours?.Count != null )
            {
                foreach (StarSystem neighbour in systemNeighbours)
                {
                    if (systems.Contains(neighbour))
                        continue;
                    List<StarSystem> subresult = GetConstellation(neighbour, new List<StarSystem>(systems));
                    foreach(StarSystem subresultSystem in subresult)
                    {
                        if (systems.Contains(subresultSystem))
                            continue;
                        systems.Add(subresultSystem);
                    }
                }
            }
            return systems;
        }

        void DrawConstellationsBorders()
        {
            List<Border.BorderPoint> points = new List<Border.BorderPoint>();
            Dictionary<Vector2, Border.BorderPoint> processedPoints = new Dictionary<Vector2, Border.BorderPoint>();

            foreach (Constellation constellation in constellations)
            {
                foreach (StarSystem system in constellation.Systems)
                {
                    Border.BorderPoint point = new Border.BorderPoint(system.Position, system);
                    points.Add(point);
                    processedPoints.Add(point.coordinates, point);
                }
            }

            bool isDrawingBorder = true;
            while (isDrawingBorder)
            {
                isDrawingBorder = false;
                List<Border.BorderPoint> copyPoints = new List<Border.BorderPoint>(points);
                copyPoints.RemoveAll(x => x.isProcessed || x.isPositioned);

                foreach (Border.BorderPoint point in copyPoints)
                {
                    isDrawingBorder = true;
                    bool commitPoint = false;
                    foreach (Vector2 direction in Utility.directions.Values)
                    {
                        commitPoint = commitPoint || MovePointTo(direction, point, processedPoints, points);
                    }

                    if (commitPoint)
                    {
                        point.isPositioned = true;
                    }
                    else
                    {
                        point.isProcessed = true;
                    }
                }
            }
            foreach (Constellation constellation in constellations)
            {
                constellation.Border.RemoveProcessed();
                constellation.Border.ConnectWithNeighbours();
                constellation.Border.SmoothBorder();
                constellation.oldBorder = new List<Vector2>(constellation.Border.Points.Keys);
                constellation.Border.SortBorderPointsList();
            }
        }
        bool MovePointTo(Vector2 direction, Border.BorderPoint point, Dictionary<Vector2, Border.BorderPoint> processedPoints, List<Border.BorderPoint> points)
        {
            bool commitPoint = false;

            Vector2 nextPosition = point.coordinates + direction;
            if (Mathf.Abs(nextPosition.x) > map.size.x / 2 || Mathf.Abs(nextPosition.y) > map.size.y / 2)
                commitPoint = true;
            else if (GetTemplateGreyscale(point.coordinates.x, point.coordinates.y) <= 0.05f)
                commitPoint = true;
            else if (!processedPoints.ContainsKey(nextPosition))
            {
                processedPoints.Add(nextPosition, point);
                points.Add(new Border.BorderPoint(nextPosition, point.parentSystem));
            }
            else if (processedPoints[nextPosition].parentSystem.Constellation != point.parentSystem.Constellation)
                commitPoint = true;
            return commitPoint;
        }

        //IDEAS: ConnectionLength (vector2 difference) as alternatives choose parameter; Storing shortest lengths in every node to every node;
        List<StarSystem> GetPath(StarSystem from, StarSystem to, List<StarSystem> path)
        {
            path.Add(from);
            List<StarSystem> originNeighbours = from.GetConnectedNeighbours();

            foreach (StarSystem destination in originNeighbours)
            {
                if (path.IndexOf(destination) >= 0)
                    continue;
                if(destination == to)
                {
                    path.Add(to);
                    return path;
                }
                List<StarSystem> subresult = GetPath(destination, to, new List<StarSystem>(path));
                if(subresult != null)
                {
                    if (path.IndexOf(to) > 0)
                    {
                        if (subresult.Count < path.Count)
                        {
                            path = subresult;
                        }
                    }
                    else
                        path = subresult;
                }
            }
            if (path.IndexOf(to) > 0)
                return path;
            return null;
        }

        bool isPositionSuitable(List<Vector2> positions, Vector2 position)
        {
            foreach(Vector2 pos in positions)
            {
                if (pos.x == position.x || pos.x - 1f == position.x || pos.x + 1f == position.x || pos.x - 2f == position.x || pos.x + 2f == position.x)
                    if (pos.y == position.y || pos.y - 1f == position.y || pos.y + 1f == position.y || pos.y - 2f == position.y || pos.y + 2f == position.y)
                        return false;
            }
            return true;
        }
    }
}