using MTK.Services;
using UnityEngine;

namespace GMT.GamePlay
{
    public class BuildingFactory : IService
    {
        public Transform CreateBuilding(BuildingSO buildingSO, Transform department)
        {
            var building = Object.Instantiate(buildingSO.Prefab, department);

            return building.transform;
        }
    }
}