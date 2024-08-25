using UnityEngine;

namespace GMT.GamePlay
{
    public class Department : MonoBehaviour
    {
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
                var building = _buildingFactory.CreateBuilding(_departmentData.Building, transform);
                building.transform.localPosition = new Vector3(i * 3 * _departmentData.DirectionOfExpansion, 0, 0);
            }
        }

    }
}
