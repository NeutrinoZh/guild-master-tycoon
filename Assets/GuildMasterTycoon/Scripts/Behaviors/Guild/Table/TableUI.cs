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
            _progressBar = transform.Find("ProgressBar");
            _filling = _progressBar.Find("Filling").GetComponent<Image>();
        }

        public void ServeAnimate(float duration)
        {
            _progressBar.gameObject.SetActive(true);

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