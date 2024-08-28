using UnityEngine;

namespace GMT.GamePlay
{
    public class Department : MonoBehaviour
    {
        [SerializeField] private int _directionOfExpansion;
        [SerializeField] private int _departmentId;

        private DepartmentSO _departmentData;
        private BuildingFactory _buildingFactory;

        public void Init(DepartmentSO departmentData, BuildingFactory buildingFactory)
        {
            _departmentData = departmentData;
            _buildingFactory = buildingFactory;
        }

        private void Start()
        {
            for (int i = 0; i < _departmentData.NumBuildings; ++i)
            {
                var buildingSO = _departmentData.Building;
                var building = _buildingFactory.CreateBuilding(buildingSO, transform, _departmentId, i);
                building.transform.localPosition = new Vector3(i * buildingSO.Width * _directionOfExpansion, 0, 0);
            }

            if (_departmentData.NumBuildings > 0)
            {
                var walls = transform.GetChild(_departmentData.NumBuildings - 1).GetComponent<BuildingWalls>();
                if (_directionOfExpansion == 1)
                    walls.CloseRight();
                else
                    walls.CloseLeft();
            }
        }

    }
}
