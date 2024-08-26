using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMT.GamePlay
{
    public class BuildingPurchasable : MonoBehaviour
    {
        private int _departmentId;
        private int _buildingId;
        private BuildingSO _buildingSO;

        private Transform _canvas;
        private TextMeshProUGUI _textMeshPrice;
        private Button _buttonBuy;

        private PlayerStats _playerStats;
        private SavesManager _savesManager;

        public void Init(int departmentId, int buildingId, BuildingSO buildingSO, PlayerStats playerStats, SavesManager savesManager)
        {
            _departmentId = departmentId;
            _buildingId = buildingId;
            _buildingSO = buildingSO;
            _playerStats = playerStats;
            _savesManager = savesManager;
        }

        private void Awake()
        {
            _canvas = transform.Find("Locked").Find("Canvas");
            _textMeshPrice = _canvas.Find("Price").GetComponent<TextMeshProUGUI>();
            _buttonBuy = _canvas.Find("BuyButton").GetComponent<Button>();
        }

        private void Start()
        {
            _textMeshPrice.text = $"$ {_buildingSO.Price}";
            _buttonBuy.onClick.AddListener(BuyButtonHandler);
        }

        private void BuyButtonHandler()
        {
            if (!_playerStats.TrySubtract(_buildingSO.Price))
                return;

            _savesManager.PurchaseBuilding(_departmentId, _buildingId);
            _canvas.gameObject.SetActive(false);
        }
    }
}