using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    public class DepartmentInjector : MonoBehaviour
    {
        [SerializeField] private DepartmentSO _departmentData;

        private void Awake()
        {
            var department = gameObject.AddComponent<Department>();
            department.Init(
                _departmentData,
                ServiceContainer.Instance.Get<BuildingFactory>()
            );

            Destroy(this);
        }
    }
}