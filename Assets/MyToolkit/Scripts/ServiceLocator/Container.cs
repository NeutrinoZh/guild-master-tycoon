using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MTK.Services
{
    public class ServiceContainer : MonoBehaviour
    {

        private static ServiceContainer _globalInstance = null;
        private static ServiceContainer _sceneInstance = null;

        public static ServiceContainer Instance => _sceneInstance;
        private Dictionary<Type, IService> _services = new();


        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            if (_globalInstance)
                return;

            // Global Container
            var globalInstance = new GameObject("GlobalServicesContainer");
            _globalInstance = globalInstance.AddComponent<ServiceContainer>();
            DontDestroyOnLoad(_globalInstance);

            // Scene Container
            CreateSceneContainer();
            SceneManager.sceneLoaded += (Scene _, LoadSceneMode _) => CreateSceneContainer();
        }

        public static void CreateSceneContainer()
        {
            if (_sceneInstance)
                Destroy(_sceneInstance);

            var sceneInstance = new GameObject("SceneServicesContainer");
            _sceneInstance = sceneInstance.AddComponent<ServiceContainer>();
        }

        public void RegisterGlobal<T>(T service) where T : IService
        {
            _globalInstance._services.Add(typeof(T), service);
        }

        public void Register<T>(T service) where T : IService
        {
            _services.Add(typeof(T), service);

#if UNITY_EDITOR
            Debug.Log($"[MTK.Services] Register new service: {typeof(T)}");
#endif
        }

        public bool Contains<T>() where T : IService
        {
            return _services.ContainsKey(typeof(T));
        }

        public T Get<T>() where T : IService
        {
            if (Contains<T>())
                return (T)_services[typeof(T)];
            return (T)_globalInstance._services[typeof(T)];
        }
    }
}