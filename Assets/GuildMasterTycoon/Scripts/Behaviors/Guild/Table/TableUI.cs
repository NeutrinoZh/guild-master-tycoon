using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using MTK.Tween;

namespace GMT.GamePlay
{
    public class TableUI : MonoBehaviour
    {
        private Transform _progressBar;
        private Image _filling;

        private void Awake()
        {
            _progressBar = transform.Find("Unlocked").Find("Canvas").Find("ProgressBar");
            _filling = _progressBar.Find("Fill").GetComponent<Image>();
        }

        private void Start()
        {
            _progressBar.gameObject.SetActive(false);
        }

        public void ServeAnimate(float duration)
        {
            _progressBar.gameObject.SetActive(true);

            _filling.fillAmount = 0;
            MyTween.To(
                () => _filling.fillAmount,
                value => _filling.fillAmount = value,
                duration,
                1
            );

            StartCoroutine(DisableProgressBar(duration));
        }

        private IEnumerator DisableProgressBar(float duration)
        {
            yield return new WaitForSeconds(duration);
            _progressBar.gameObject.SetActive(false);
        }
    }
}