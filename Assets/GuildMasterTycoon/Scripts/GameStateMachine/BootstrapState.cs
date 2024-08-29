using MTK.Services;
using MTK.StateMachine;

using GMT.GamePlay;

namespace GMT.GSM
{
    public class BootstrapState : State
    {
        public override void OnEnter()
        {
            ServiceContainer.Init();

            ServiceContainer.Instance.Register(new SavesManager());
            ServiceContainer.Instance.Register(new PlayerStats(
                ServiceContainer.Instance.Get<SavesManager>()
            ));
            ServiceContainer.Instance.Register(new BuildingFactory());
            ServiceContainer.Instance.Register(new PlayerInput());
            ServiceContainer.Instance.Register(new TableManager());
        }
    }
}