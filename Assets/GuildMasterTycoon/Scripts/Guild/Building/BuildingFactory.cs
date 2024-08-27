using MTK.Services;
using UnityEngine;

namespace GMT.GamePlay
{
    public class BuildingFactory : IService
    {
        public Transform CreateBuilding(BuildingSO buildingSO, Transform department, int departmentId, int buildingId)
        {
            var building = Object.Instantiate(buildingSO.Prefab, department);

            building.GetComponent<BuildingPurchasable>().Init(
                departmentId,
                buildingId,
                buildingSO,
                ServiceContainer.Instance.Get<PlayerStats>(),
                ServiceContainer.Instance.Get<SavesManager>()
            );

            foreach (TableInjector injector in building.GetComponentsInChildren<TableInjector>())
                injector.Init(departmentId, buildingId, buildingSO);

            return building.transform;
        }
    }
}