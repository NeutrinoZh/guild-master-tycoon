using MTK;
using MTK.Services;
using UnityEngine;

namespace GMT.GamePlay
{
    [DefaultExecutionOrder(-50)]
    public class NavGraphsManager : MonoBehaviour, IService
    {
        [SerializeField] private NavGraph _adventuresNavGraph;

        public NavGraph AdventuresNavGraph()
        {
            return _adventuresNavGraph;
        }

        private void Awake()
        {
            ServiceContainer.Instance.Register(this);
        }
    }
}