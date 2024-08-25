using System;
using System.Collections.Generic;
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
    }
}
