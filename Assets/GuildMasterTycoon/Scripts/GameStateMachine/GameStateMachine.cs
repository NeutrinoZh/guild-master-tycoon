using MTK.StateMachine;

namespace GMT.GSM
{
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine() : base(new()
        {
            [typeof(BootstrapState)] = new BootstrapState()
        })
        {
        }
    }

}