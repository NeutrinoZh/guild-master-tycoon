using UnityEditor;
using UnityEngine;

namespace MTK
{
    [CustomEditor(typeof(NavGraph))]
    public class NavGraphEditor : Editor
    {
        private bool _isAddingPoints = false;
        private bool _isAddingConnections = false;
        private int _selectedFromIndex = -1;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(_isAddingPoints ? "Stop Adding Points" : "Start Adding Points"))
            {
                _isAddingConnections = false;
                _isAddingPoints = !_isAddingPoints;
                Tools.current = _isAddingPoints ? Tool.None : Tools.current;
            }

            if (GUILayout.Button(_isAddingConnections ? "Stop Adding Connections" : "Start Adding Connections"))
            {
                _isAddingPoints = false;
                _isAddingConnections = !_isAddingConnections;
                Tools.current = _isAddingConnections ? Tool.None : Tools.current;
            }
        }

        private void OnSceneGUI()
        {
            NavGraph navGraph = (NavGraph)target;

            Handles.BeginGUI();

            if (_isAddingPoints)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                {
                    Undo.RecordObject(navGraph, "Add Point");

                    var point = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(0);
                    navGraph.points.Add(point);

                    EditorUtility.SetDirty(navGraph);
                    Event.current.Use();
                }
            }

            if (_isAddingConnections)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                {
                    Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                    Vector2 mousePos2D = (Vector2)mousePosition;

                    int nearestPointIndex = -1;
                    float minDistance = float.MaxValue;
                    for (int i = 0; i < navGraph.points.Count; i++)
                    {
                        float distance = (mousePos2D - navGraph.points[i]).sqrMagnitude;
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestPointIndex = i;
                        }
                    }

                    if (nearestPointIndex != -1)
                    {
                        if (_selectedFromIndex == -1)
                        {
                            _selectedFromIndex = nearestPointIndex;
                        }
                        else
                        {
                            navGraph.AddConnection(_selectedFromIndex, nearestPointIndex);
                            _selectedFromIndex = -1;
                        }

                        Event.current.Use();
                    }
                }
            }

            Handles.EndGUI();

            foreach (var connection in navGraph.connections)
            {
                Vector2 from = navGraph.points[connection.fromId];
                Vector2 to = navGraph.points[connection.toId];
                Handles.DrawLine(from, to);
            }

            for (int i = 0; i < navGraph.points.Count; i++)
            {
                Handles.Label(navGraph.points[i], $"Point  {i + 1}");
                Handles.SphereHandleCap(0, navGraph.points[i], Quaternion.identity, 0.1f, EventType.Repaint);
            }
        }
    }
}
