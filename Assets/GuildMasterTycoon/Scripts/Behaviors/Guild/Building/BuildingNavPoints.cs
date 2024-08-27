using System.Collections.Generic;

using UnityEngine;

using MTK.Services;
using MTK;

namespace GMT.GamePlay
{
    public class BuildingNavPoints : MonoBehaviour, IService
    {
        [SerializeField] private List<int> _stayPoints;
        [SerializeField] private List<int> _servePoints;

        private NavControlPoints _navControlPoints;
        private NavGraph _localNavGraph;
        private NavGraphsManager _navGraphsManager;

        private int _departmentId;
        private int _buildingId;

        private SavesManager _savesManager;
        private BuildingPurchasable _buildingPurchasable;

        private bool _isAddedToGlobalGraph;

        public void Init(int departmentId, int buildingId, SavesManager savesManager, NavControlPoints navControlPoints, NavGraphsManager navGraphsManager)
        {
            _departmentId = departmentId;
            _buildingId = buildingId;
            _savesManager = savesManager;
            _navControlPoints = navControlPoints;
            _navGraphsManager = navGraphsManager;
        }

        private void Awake()
        {
            _buildingPurchasable = GetComponent<BuildingPurchasable>();
            _localNavGraph = GetComponent<NavGraph>();
            _isAddedToGlobalGraph = false;
        }

        private void Start()
        {
            _savesManager.OnBuildingPurchase += PurchasedHandler;
            _savesManager.OnWorkerPurchase += WorkerPurchasedHandler;

            PurchasedHandler();
            for (int i = 0; i < 3; ++i)
                WorkerPurchasedHandler(_departmentId, _buildingId, i);
        }

        private void WorkerPurchasedHandler(int departmentId, int buildingId, int workerId)
        {
            if (departmentId != _departmentId || buildingId != _buildingId)
                return;

            if (!_savesManager.IsWorkerPurchased(departmentId, buildingId, workerId))
                return;

            _navControlPoints.AddServePoints(new() { _servePoints[workerId] });
        }

        private void PurchasedHandler()
        {
            if (_isAddedToGlobalGraph)
                return;

            if (!_buildingPurchasable.IsBuildingPurchased())
                return;

            var globalGraph = _navGraphsManager.AdventuresNavGraph();

            for (int i = 0; i < _stayPoints.Count; ++i)
                _stayPoints[i] += globalGraph.points.Count;

            for (int i = 0; i < _servePoints.Count; ++i)
                _servePoints[i] += globalGraph.points.Count;

            _navControlPoints.AddStayPoints(_stayPoints);

            globalGraph.Merge(_localNavGraph);
            _isAddedToGlobalGraph = true;
        }
    }
}