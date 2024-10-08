using System.Collections;

using UnityEngine;

using MTK;
using MTK.Services;
using MTK.StateMachine;

using GMT.GamePlay;

namespace GMT.GamePlay
{
    public class ServingState : State
    {
        private Transform _transform;
        private Adventurer _adventurer;

        private NavGraphAgent _navAgent;
        private NavControlPoints _navControlPoints;
        private TableManager _tableManager;

        private PlayerStats _playerStats;

        public ServingState(Transform transform)
        {
            _transform = transform;

            _adventurer = _transform.GetComponent<Adventurer>();
            _navAgent = _transform.GetComponent<NavGraphAgent>();

            _navControlPoints = ServiceContainer.Instance.Get<NavControlPoints>();
            _playerStats = ServiceContainer.Instance.Get<PlayerStats>();
            _tableManager = ServiceContainer.Instance.Get<TableManager>();
        }

        public override void OnEnter()
        {
            _adventurer.StartSingleRoutine(ServingRoutine());
        }

        private IEnumerator ServingRoutine()
        {
            _tableManager
                .GetTableByPoint(_navAgent.TargetPoint)
                .GetComponent<TableUI>()
                .ServeAnimate(_adventurer.ServingTime);

            yield return new WaitForSeconds(_adventurer.ServingTime);

            _playerStats.AddMoney(_adventurer.Revenue);

            _navControlPoints.ReturnServePoint(_navAgent.TargetPoint);
            _navAgent.MoveTo(_navControlPoints.GetRandomEndPoint());

            _stateMachine.Enter<WalkingState>();
        }
    }
}