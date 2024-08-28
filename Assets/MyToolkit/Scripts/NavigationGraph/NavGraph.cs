using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

namespace MTK
{
    [Serializable]
    public struct Connection
    {
        public int fromId;
        public int toId;
    }

    public class NavGraph : MonoBehaviour
    {
        public List<Vector2> points = new();
        public List<Connection> connections = new();
        public List<int> externalPoints = new();

        public void Merge(NavGraph navGraph)
        {
            var localConnections = navGraph.connections.ToArray();
            for (int i = 0; i < localConnections.Length; ++i)
            {
                var connection = localConnections[i];
                connection.fromId += points.Count;
                connection.toId += points.Count;

                connections.Add(connection);
            }

            float minSqrDistance = float.MaxValue;
            int pointI = -1, pointJ = -1;

            foreach (var exPointGlobal in externalPoints)
                foreach (var exPointLocal in navGraph.externalPoints)
                {
                    float sqrDistance = (points[exPointGlobal] - (navGraph.points[exPointLocal] + (Vector2)navGraph.transform.position)).sqrMagnitude;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        pointI = exPointGlobal;
                        pointJ = exPointLocal + points.Count;
                    }
                }

            foreach (var externalPoint in navGraph.externalPoints)
                externalPoints.Add(externalPoint + points.Count);

            foreach (var point in navGraph.points)
            {
                var copyPoint = point + (Vector2)navGraph.transform.position;
                points.Add(copyPoint);
            }

            if (pointI != -1 && pointJ != -1)
                AddConnection(pointI, pointJ);
        }

        public void AddConnection(int fromId, int toId)
        {
            connections.Add(new Connection()
            {
                fromId = fromId,
                toId = toId
            });

            connections.Add(new Connection()
            {
                fromId = toId,
                toId = fromId
            });
        }

        public List<Connection> GetConnectionsFromPoint(int pointId)
        {
            return connections.Where(c => c.fromId == pointId).ToList();
        }

        public List<Vector2> FindPath(int fromId, int toId)
        {
            var costs = new Dictionary<int, float>();

            void buildCostTable(int pointId)
            {
                var connections = GetConnectionsFromPoint(pointId);
                foreach (var connection in connections)
                {
                    if (costs.ContainsKey(connection.toId))
                        continue;

                    costs[connection.toId] = costs[pointId] + (points[connection.toId] - points[pointId]).sqrMagnitude;
                    buildCostTable(connection.toId);
                }
            }

            costs[toId] = 0;
            buildCostTable(toId);

            var path = new List<Vector2>();

            void buildPath(int pointId)
            {
                path.Add(points[pointId]);

                if (pointId == toId)
                    return;

                var nearestPoint = points[pointId];
                var nearestCost = float.MaxValue;
                var nearestPointId = pointId;

                foreach (var toIdLocal in GetConnectionsFromPoint(pointId).Select(c => c.toId))
                {
                    if (path.Contains(points[toIdLocal]))
                        continue;

                    if (costs[toIdLocal] < nearestCost)
                    {
                        nearestCost = costs[toIdLocal];
                        nearestPoint = points[toIdLocal];
                        nearestPointId = toIdLocal;
                    }
                }

                if (nearestPoint != points[pointId])
                    buildPath(nearestPointId);
            }

            buildPath(fromId);

            return path;
        }
    }
}
