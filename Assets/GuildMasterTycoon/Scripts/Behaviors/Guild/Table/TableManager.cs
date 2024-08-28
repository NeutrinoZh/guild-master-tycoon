using System.Collections.Generic;

using UnityEngine;

using MTK.Services;

namespace GMT.GamePlay
{
    [DefaultExecutionOrder(-50)]
    public class TableManager : MonoBehaviour, IService
    {
        private Dictionary<int, Transform> _tables;

        private void Awake()
        {
            ServiceContainer.Instance.Register(this);
        }
    }
}