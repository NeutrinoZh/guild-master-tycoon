using GMT.GSM;
using UnityEngine;

namespace GMT
{
    public class Game : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine();
        }

        private void Update()
        {
            _gameStateMachine.Update();
        }
    }
}