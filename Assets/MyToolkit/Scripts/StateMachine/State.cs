namespace MTK.StateMachine
{
    public abstract class State
    {
        private StateMachine _stateMachine;

        public void Init(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    };
}