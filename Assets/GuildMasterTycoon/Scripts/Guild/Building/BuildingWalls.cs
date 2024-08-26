using UnityEngine;

namespace GMT.GamePlay
{
    public class BuildingWalls : MonoBehaviour
    {
        private GameObject _rightOpen;
        private GameObject _rightClose;
        private GameObject _leftOpen;
        private GameObject _leftClose;

        private void Awake()
        {
            var walls = transform.Find("Unlocked").Find("Walls");

            _rightOpen = walls.Find("Right-Open").gameObject;
            _rightClose = walls.Find("Right-Close").gameObject;
            _leftOpen = walls.Find("Left-Open").gameObject;
            _leftClose = walls.Find("Left-Close").gameObject;

            OpenLeft();
            OpenRight();
        }

        public void OpenLeft()
        {
            _leftOpen.gameObject.SetActive(true);
            _leftClose.gameObject.SetActive(false);
        }

        public void OpenRight()
        {
            _rightOpen.gameObject.SetActive(true);
            _rightClose.gameObject.SetActive(false);
        }

        public void CloseLeft()
        {
            _leftOpen.gameObject.SetActive(false);
            _leftClose.gameObject.SetActive(true);
        }

        public void CloseRight()
        {
            _rightOpen.gameObject.SetActive(false);
            _rightClose.gameObject.SetActive(true);
        }
    }
}