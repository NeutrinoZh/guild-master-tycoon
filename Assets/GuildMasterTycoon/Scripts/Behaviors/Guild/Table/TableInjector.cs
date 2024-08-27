using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    public class TableInjector : MonoBehaviour
    {
        public void Init(int departmentId, int buildingId, BuildingSO buildingSO)
        {
            GetComponent<TablePurchasable>().Init(
                departmentId,
                buildingId,
                buildingSO,
                ServiceContainer.Instance.Get<PlayerStats>(),
                ServiceContainer.Instance.Get<SavesManager>()
            );

            Destroy(this);
        }
    }
}