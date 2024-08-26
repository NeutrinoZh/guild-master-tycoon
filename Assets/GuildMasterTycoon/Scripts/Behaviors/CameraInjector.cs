using MTK.Services;
using UnityEngine;

namespace GMT.GamePlay
{
    public class CameraInjector : MonoBehaviour
    {
        private void Awake()
        {
            var cameraController = gameObject.AddComponent<CameraController>();
            cameraController.Init(
                ServiceContainer.Instance.Get<PlayerInput>()
            );

            Destroy(this);
        }
    }
}