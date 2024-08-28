using UnityEngine;

using MTK.StateMachine;

namespace GMT.GamePlay
{
    public class AdventurerSM : StateMachine
    {
        public AdventurerSM(Transform transform) : base(new()
        {
            [typeof(WalkingState)] = new WalkingState(transform),
            [typeof(ServingState)] = new ServingState(transform),
            [typeof(StayingState)] = new StayingState(transform)
        })
        {
        }
    }
}