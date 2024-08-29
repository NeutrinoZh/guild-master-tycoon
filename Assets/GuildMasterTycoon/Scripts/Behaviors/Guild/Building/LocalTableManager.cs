using System;
using System.Collections.Generic;

using UnityEngine;

namespace GMT.GamePlay
{
    [Serializable]
    public struct PointTablePair
    {
        public int point;
        public Transform table;
    }

    [DefaultExecutionOrder(-1)]
    public class LocalTableManager : MonoBehaviour
    {
        [SerializeField] private List<PointTablePair> pointTablePairs = new();

        private TableManager _tableManager;
        private BuildingNavPoints _buildingNav;

        public void Init(TableManager tableManager)
        {
            _tableManager = tableManager;
            _buildingNav = GetComponent<BuildingNavPoints>();
        }

        private void Start()
        {
            _buildingNav.OnBeforeMerge += MergePointsToGlobal;
        }

        private void MergePointsToGlobal(int offset)
        {
            _tableManager.MergeTable(pointTablePairs, offset);
        }
    }
}