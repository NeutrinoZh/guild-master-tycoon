using System;
using System.Collections.Generic;

using UnityEngine;

namespace MTK.Tween
{
    public class MyTween : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeInitialized()
        {
            if (_instance != null)
                return;

            var instanceObject = new GameObject("MyTweener");
            _instance = instanceObject.AddComponent<MyTween>();

            DontDestroyOnLoad(instanceObject);
        }

        private static MyTween _instance;
        private List<ITween> _tweens = new();

        public static void To<T>(Func<T> getter, Action<T> setter, float duration, T endValue)
        {
            _instance._tweens.Add(new Tween<T>(
                getter, setter, duration, endValue
            ));
        }

        private void Update()
        {
            var tweens = _tweens.ToArray();
            foreach (var tween in tweens)
                if (tween.Update())
                    _tweens.Remove(tween);
        }
    }
}
