using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    [DefaultExecutionOrder(-50)]
    public class DepartmentInjector : MonoBehaviour
    {
        [SerializeField] private DepartmentSO _departmentData;

        private void Awake()
        {
            var department = GetComponent<Department>();
            department.Init(
                _departmentData,
                ServiceContainer.Instance.Get<BuildingFactory>()
            );

            Destroy(this);
        }
    }
}