using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MTK.Services
{
    public class Container : MonoBehaviour
    {
        private static GameObject _globalInstance = null;
        private static GameObject _sceneInstance = null;

        private Dictionary<Type, IService> _services = new();

        private void Awake()
        {
            SceneManager.sceneLoaded += (Scene _, LoadSceneMode _) =>
            {
                Debug.Log("Scene Loaded");
                _sceneInstance = new GameObject("SceneServicesContainer");
                _sceneInstance.AddComponent<Container>();
            };
        }

        public void Reset()
        {
            _services.Clear();
        }

        public void Register<T>(T service) where T : IService
        {
            _services.Add(typeof(T), service);

#if UNITY_EDITOR
            Debug.Log($"[MTK.Services] Register new service: {typeof(T)}");
#endif
        }

        public T Get<T>() where T : IService
        {
            return (T)_services[typeof(T)];
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            if (_globalInstance)
                return;

            _globalInstance = new GameObject("GlobalServicesContainer");
            _globalInstance.AddComponent<Container>();

            DontDestroyOnLoad(_globalInstance);
        }
    }
}