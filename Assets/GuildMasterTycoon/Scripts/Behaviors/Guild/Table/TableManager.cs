using System.Collections.Generic;

using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    [DefaultExecutionOrder(-100)]
    public class TableManager : IService
    {
        private Dictionary<int, Transform> _tables = new();

        public Transform GetTableByPoint(int pointId)
        {
            return _tables[pointId];
        }

        public void MergeTable(List<PointTablePair> pairs, int offset)
        {
            foreach (var pair in pairs)
                _tables.Add(pair.point + offset, pair.table);
        }
    }
}