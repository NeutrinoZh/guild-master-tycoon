using System.Collections;

using UnityEngine.UI;
using UnityEngine;

using GMT.GamePlay;

namespace GMT.UI
{
    public class Tutorial : MonoBehaviour
    {
        private const float _delayBeforePossibleToSkipTutorial = 3;
        private const float _maxTimeTutorial = 10f;

        private SavesManager _savesManager;
        private Button _button;

        public void Init(SavesManager savesManager)
        {
            _savesManager = savesManager;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (_savesManager.IsTutorialCompleted())
            {
                gameObject.SetActive(false);
                return;
            }

            _button.onClick.AddListener(OnButtonHandler);
            StartCoroutine(CompleteTutorialByDelay());
        }

        private void OnButtonHandler()
        {
            if (Time.timeSinceLevelLoad > _delayBeforePossibleToSkipTutorial)
            {
                _savesManager.CompleteTutorial();
                gameObject.SetActive(false);
            }
        }

        private IEnumerator CompleteTutorialByDelay()
        {
            yield return new WaitForSeconds(_maxTimeTutorial);
            _savesManager.CompleteTutorial();
            gameObject.SetActive(false);
        }
    }
}