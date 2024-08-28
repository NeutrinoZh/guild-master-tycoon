using MTK.Tween;
using System.Collections;

using UnityEngine;

namespace GMT.GamePlay
{
    public class Adventurer : MonoBehaviour
    {
        [SerializeField] int _revenue;
        [SerializeField] int _servingTime;

        private Coroutine _routine;
        private AdventurerSM _adventurerSM;

        public int ServingTime => _servingTime;
        public int Revenue => _revenue;

        public void Release()
        {
            var pool = GetComponentInParent<AdventurersPool>();
            pool.ReturnAdventurer(transform);
        }

        public void StartSingleRoutine(IEnumerator routine)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(routine);
        }

        private void Awake()
        {
            _adventurerSM = new(transform);
        }

        private void Update()
        {
            _adventurerSM.Update();
        }
    }
}