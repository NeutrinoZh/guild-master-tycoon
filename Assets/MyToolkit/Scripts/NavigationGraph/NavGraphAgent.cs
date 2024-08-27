using System.Collections.Generic;
using UnityEngine;

namespace MTK
{
    public class NavGraphAgent : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private NavGraph _navGraph;
        private int _currentPointId;
        private int _targetPointId;

        private List<Vector2> _path = new();
        private int _currentTarget = 0;

        public void Init(NavGraph navGraph, int currentPointId, int targetPointId)
        {
            _navGraph = navGraph;
            _currentPointId = currentPointId;
            _targetPointId = targetPointId;
        }

        private void Start()
        {
            _path = _navGraph.FindPath(_currentPointId, _targetPointId);
        }

        private void Update()
        {
            if (_currentTarget >= _path.Count)
                return;

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
