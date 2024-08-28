using UnityEngine;

using MTK;
using MTK.Services;
using MTK.StateMachine;

using GMT.GamePlay;

namespace GMT.GamePlay
{
    public class WalkingState : State
    {
        private Transform _transform;
        private Adventurer _adventurer;

        private NavGraphAgent _navAgent;
        private NavControlPoints _navControlPoints;

        public WalkingState(Transform transform) {
            _transform = transform;
            _adventurer = _transform.GetComponent<Adventurer>();

            _navAgent = _transform.GetComponent<NavGraphAgent>();
            _navControlPoints = ServiceContainer.Instance.Get<NavControlPoints>();
        }

        public override void OnEnter()
        {   
            _navAgent.OnPathFinished += OnPathFinishedHandler;   
        }

        public override void OnExit()
        {
            _navAgent.OnPathFinished -= OnPathFinishedHandler;
        }

        private void OnPathFinishedHandler()
        {
            if (_navControlPoints.IsItEndPoint(_navAgent.TargetPoint))
            {
               _adventurer.Release();
                return;
            }

            if (_navControlPoints.IsItServingPoint(_navAgent.TargetPoint))
                _stateMachine.Enter<ServingState>();

            if (_navControlPoints.IsItStayingPoint(_navAgent.TargetPoint))
                _stateMachine.Enter<StayingState>();
        }
    }
}