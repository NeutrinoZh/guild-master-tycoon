using GMT.GamePlay;
using MTK.Services;
using UnityEngine;

namespace GMT.UI
{
    [RequireComponent(typeof(HUD))]
    [DefaultExecutionOrder(-100)]
    public class HUDInjector : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<HUD>().Init(
                ServiceContainer.Instance.Get<PlayerStats>()
            );

            GetComponentInChildren<Tutorial>().Init(
                ServiceContainer.Instance.Get<SavesManager>()
            );

            Destroy(this);
        }
    }
}