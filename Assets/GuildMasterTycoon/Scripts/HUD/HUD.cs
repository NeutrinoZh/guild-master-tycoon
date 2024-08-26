using GMT.GamePlay;
using TMPro;
using UnityEngine;

namespace GMT.UI
{
    public class HUD : MonoBehaviour
    {
        private TextMeshProUGUI _adventurersTextMesh;
        private TextMeshProUGUI _moneyTextMesh;

        private string _adventurersTextPattern;
        private string _moneyTextPattern;

        private PlayerStats _playerStats;

        public void Init(PlayerStats playerStats)
        {
            _playerStats = playerStats;
            _playerStats.OnBalanceChanged += UpdateHUD;

            UpdateHUD(playerStats);
        }

        private void Awake()
        {
            var statisticsGroup = transform.Find("StatisticsGroup");

            _adventurersTextMesh = statisticsGroup.Find("Adventurers").GetComponent<TextMeshProUGUI>();
            _moneyTextMesh = statisticsGroup.Find("Money").GetComponent<TextMeshProUGUI>();

            _adventurersTextPattern = _adventurersTextMesh.text;
            _moneyTextPattern = _moneyTextMesh.text;
        }

        private void UpdateHUD(PlayerStats stats)
        {
            _moneyTextMesh.text = _moneyTextPattern.Replace("{value}", stats.Balance.ToString());
        }
    }
}