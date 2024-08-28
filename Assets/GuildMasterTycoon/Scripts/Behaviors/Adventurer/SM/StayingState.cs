using System.Collections;

using UnityEngine;

using MTK;
using MTK.Services;
using MTK.StateMachine;

using GMT.GamePlay;

namespace GMT.GamePlay
{
    public class StayingState : State
    {
        private Transform _transform;
        private Adventurer _adventurer;
        
        private NavGraphAgent _navAgent;
        private NavControlPoints _navControlPoints;

        public StayingState(Transform transform) {
            _transform = transform;
            _adventurer = _transform.GetComponent<Adventurer>();
            
            _navAgent = _transform.GetComponent<NavGraphAgent>();
            _navControlPoints = ServiceContainer.Instance.Get<NavControlPoints>();
        }

        public override void OnEnter()
        {   
            _adventurer.StartSingleRoutine(StayingRoutine());
        }

        private IEnumerator StayingRoutine()
        {
           while (!_navControlPoints.IsExistAvailableServingPoint())
                yield return null;

            _navControlPoints.ReturnStayPoint(_navAgent.TargetPoint);
            _navAgent.MoveTo(
                _navControlPoints.TakeRandomServePoint()
            );
        
            _stateMachine.Enter<WalkingState>();

            yield break;
        }
    }
}