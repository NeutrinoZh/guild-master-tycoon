using MTK.Services;

namespace GMT.GamePlay
{
    public class PlayerInput : IService
    {
        public GMTActions InputActions => _inputActions;
        private GMTActions _inputActions;

        public PlayerInput()
        {
            _inputActions = new GMTActions();
            _inputActions.Enable();
        }
    }
}