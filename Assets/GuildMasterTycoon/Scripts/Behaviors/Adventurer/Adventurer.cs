using UnityEngine;

using MTK;

using GMT.GamePlay;
using System.Collections;

namespace GMT
{
    public class Adventurer : MonoBehaviour
    {
        [SerializeField] int _revenue;
        [SerializeField] int _servingTime;

        private NavGraphAgent _navAgent;
        private NavControlPoints _navControlPoints;
        private PlayerStats _playerStats;

        [SerializeField] private int _state = 0;

        public void Init(PlayerStats playerStats, NavControlPoints navControlPoints)
        {
            _playerStats = playerStats;
            _navControlPoints = navControlPoints;
        }

        private void Awake()
        {
            _navAgent = GetComponent<NavGraphAgent>();
            _navAgent.OnPathFinished += OnPathFinishedHandler;
        }

        private void OnPathFinishedHandler()
        {
            if (_navControlPoints.IsItEndPoint(_navAgent.CurrentPoint))
            {
                _state = 0;

                var pool = GetComponentInParent<AdventurersPool>();
                pool.ReturnAdventurer(transform);

                return;
            }

            if (_navControlPoints.IsItServingPoint(_navAgent.CurrentPoint))
            {
                _state = 1;
                StartCoroutine(ServingRoutine());
            }

            if (_navControlPoints.IsItStayingPoint(_navAgent.CurrentPoint))
            {
                _state = 2;
                StartCoroutine(StayingRoutine());
            }
        }

        private IEnumerator StayingRoutine()
        {
            while (!_navControlPoints.IsExistAvailableServingPoint())
                yield return new WaitForSeconds(1);

            _navControlPoints.ReturnStayPoint(_navAgent.CurrentPoint);
            _navAgent.MoveTo(
                _navControlPoints.TakeRandomServePoint()
            );

            _state = 3;

            yield break;
        }

        private IEnumerator ServingRoutine()
        {
            yield return new WaitForSeconds(_servingTime);
            _playerStats.AddMoney(_revenue);

            _navControlPoints.ReturnServePoint(_navAgent.CurrentPoint);
            _navAgent.MoveTo(_navControlPoints.GetRandomEndPoint());

            _state = 4;
        }
    }
}