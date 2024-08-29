using UnityEngine;

using MTK.Services;

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

            building.GetComponent<BuildingNavPoints>().Init(
                departmentId,
                buildingId,
                ServiceContainer.Instance.Get<SavesManager>(),
                ServiceContainer.Instance.Get<NavControlPoints>(),
                ServiceContainer.Instance.Get<NavGraphsManager>()
            );

            foreach (TableInjector injector in building.GetComponentsInChildren<TableInjector>())
                injector.Init(departmentId, buildingId, buildingSO);

            building.GetComponent<LocalTableManager>().Init(
                ServiceContainer.Instance.Get<TableManager>()
            );

            return building.transform;
        }
    }
}