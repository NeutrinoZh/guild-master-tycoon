using System;
using UnityEngine;

namespace MTK.Tween
{
    internal class Tween<T> : ITween
    {
        private float _startTime;
        private float _duration;
        private T _startValue;
        private T _endValue;
        private Func<T> _getter;
        private Action<T> _setter;

        internal Tween(Func<T> getter, Action<T> setter, float duration, T endValue)
        {
            _getter = getter;
            _setter = setter;
            _duration = duration;
            _endValue = endValue;

            _startValue = _getter();
            _startTime = Time.time;
        }

        bool ITween.Update()
        {
            var step = (Time.time - _startTime) / _duration;

            if (step >= 1)
            {
                _setter?.Invoke(_endValue);
                return true;
            }
            else
            {
                _setter?.Invoke(
                    (dynamic)_startValue + ((dynamic)_endValue - (dynamic)_startValue) * step
                );
                return false;
            }
        }
    };
}