using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    public class AdventurersPoolInjector : MonoBehaviour
    {
        public void Awake()
        {
            GetComponent<AdventurersPool>().Init(
                ServiceContainer.Instance.Get<NavControlPoints>(),
                ServiceContainer.Instance.Get<NavGraphsManager>().AdventuresNavGraph()
            );

            Destroy(this);
        }
    }
}