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
        private Coroutine _routine;

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
            if (_routine != null)
                StopCoroutine(_routine);

            if (_navControlPoints.IsItEndPoint(_navAgent.TargetPoint))
            {
                var pool = GetComponentInParent<AdventurersPool>();
                pool.ReturnAdventurer(transform);

                return;
            }

            if (_navControlPoints.IsItServingPoint(_navAgent.TargetPoint))
            {
                _routine = StartCoroutine(ServingRoutine());
            }

            if (_navControlPoints.IsItStayingPoint(_navAgent.TargetPoint))
            {
                _routine = StartCoroutine(StayingRoutine());
            }
        }

        private IEnumerator StayingRoutine()
        {
            while (!_navControlPoints.IsExistAvailableServingPoint())
                yield return new WaitForSeconds(1);

            _navControlPoints.ReturnStayPoint(_navAgent.TargetPoint);
            _navAgent.MoveTo(
                _navControlPoints.TakeRandomServePoint()
            );

            yield break;
        }

        private IEnumerator ServingRoutine()
        {
            yield return new WaitForSeconds(_servingTime);
            _playerStats.AddMoney(_revenue);

            _navControlPoints.ReturnServePoint(_navAgent.TargetPoint);
            _navAgent.MoveTo(_navControlPoints.GetRandomEndPoint());
        }
    }
}