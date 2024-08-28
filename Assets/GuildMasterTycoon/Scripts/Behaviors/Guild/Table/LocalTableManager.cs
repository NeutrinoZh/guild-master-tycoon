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

    public class LocalTableManager : MonoBehaviour
    {
        [SerializeField] private List<PointTablePair> pointTablePairs = new();
        private TableManager _tableManager;
        private SavesManager _savesManager;

        public void Init(TableManager tableManager, SavesManager savesManager)
        {
            _tableManager = tableManager;
            _savesManager = savesManager;
        }

        private void Start()
        {

        }

    }
}