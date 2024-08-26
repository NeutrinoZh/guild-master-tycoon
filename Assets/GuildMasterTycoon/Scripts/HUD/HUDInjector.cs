using GMT.GamePlay;
using MTK.Services;
using UnityEngine;

namespace GMT.UI
{
    [RequireComponent(typeof(HUD))]
    public class HUDInjector : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<HUD>().Init(
                ServiceContainer.Instance.Get<PlayerStats>()
            );

            Destroy(this);
        }
    }
}