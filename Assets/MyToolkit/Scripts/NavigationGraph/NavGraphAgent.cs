using System.Collections.Generic;
using System;

using UnityEngine;

namespace MTK
{
    public class NavGraphAgent : MonoBehaviour
    {
        public event Action OnPathFinished;

        [SerializeField] private float _speed;

        private NavGraph _navGraph;
        private int _currentPointId;
        private int _targetPointId;

        private List<Vector2> _path = new();
        private int _currentTarget = 0;

        private bool _isPathFinished = false;

        public int TargetPoint => _targetPointId;

        public void Init(NavGraph navGraph, int currentPointId)
        {
            _navGraph = navGraph;

            _currentPointId = currentPointId;
            _targetPointId = currentPointId;
        }

        public void MoveTo(int targetPointId)
        {
            _currentPointId = _targetPointId;
            _targetPointId = targetPointId;
            _isPathFinished = false;
            _currentTarget = 0;
            _path = _navGraph.FindPath(_currentPointId, _targetPointId);
        }

        private void Update()
        {
            if (_currentTarget >= _path.Count)
            {
                if (!_isPathFinished)
                {
                    _currentPointId = _targetPointId;
                    _isPathFinished = true;

                    OnPathFinished?.Invoke();
                }

                return;
            }

            var vector = (Vector3)_path[_currentTarget] - transform.position;

            var sqrDistance = vector.sqrMagnitude;
            if (sqrDistance < 0.1f)
            {
                _currentTarget += 1;
                return;
            }

            transform.position += Time.deltaTime * _speed * vector.normalized;
        }
    }
}
