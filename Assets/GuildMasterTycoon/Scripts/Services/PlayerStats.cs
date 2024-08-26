using System;

using MTK.Services;

namespace GMT.GamePlay
{
    public class PlayerStats : IService
    {
        public event Action<PlayerStats> OnBalanceChanged;
        public int Balance => _savesManager.Money;

        public PlayerStats(SavesManager savesManager)
        {
            _savesManager = savesManager;
        }

        public void AddMoney(int number)
        {
            _savesManager.Money += number;
            OnBalanceChanged?.Invoke(this);
        }

        public bool TrySubtract(int number)
        {
            if (_savesManager.Money - number >= 0)
            {
                _savesManager.Money -= number;
                OnBalanceChanged?.Invoke(this);

                return true;
            }

            return false;
        }

        private SavesManager _savesManager;
    }
}