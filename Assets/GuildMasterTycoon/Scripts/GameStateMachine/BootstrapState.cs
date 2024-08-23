using GMT.GamePlay;

using MTK.Services;
using MTK.StateMachine;

namespace GMT.GSM
{
    public class BootstrapState : State
    {
        public override void OnEnter()
        {
            ServiceContainer.Init();
            ServiceContainer.Instance.Register(new BuildingFactory());
        }
    }
}