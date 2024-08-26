using MTK.Services;
using System;

namespace GMT.GamePlay
{
    public class PlayerStats : IService
    {
        public event Action<PlayerStats> OnBalanceChanged;

        public int Balance => _money;

        public void AddMoney(int number)
        {
            _money += number;
            OnBalanceChanged?.Invoke(this);
        }

        public bool TrySubtract(int number)
        {
            if (_money - number >= 0)
            {
                _money -= number;
                OnBalanceChanged?.Invoke(this);

                return true;
            }

            return false;
        }

        private int _money = 0;

    }
}