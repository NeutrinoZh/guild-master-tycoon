using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

using MTK.Services;
using System.Collections;
using GMT.GamePlay;
using MTK;

namespace GMT
{
    public class AdventurersPool : MonoBehaviour, IService
    {
        [SerializeField] private Transform _adventurerPrefab;
        [SerializeField] private float _spawnInterval;

        private IObjectPool<Transform> _adventurersPool;
        private List<Transform> _adventurers;

        private NavControlPoints _navControlPoints;
        private NavGraph _navGraph;

        public void Init(NavControlPoints navControlPoints, NavGraph navGraph)
        {
            _navControlPoints = navControlPoints;
            _navGraph = navGraph;
        }

        private void Awake()
        {
            ServiceContainer.Instance.Register(this);

            _adventurers = new();
            _adventurersPool = new ObjectPool<Transform>(
                () => Instantiate(_adventurerPrefab, transform),
                obj => obj.gameObject.SetActive(true),
                obj => obj.gameObject.SetActive(false),
                obj => Destroy(obj.gameObject),
                true, 10
            );
        }

        private void Start()
        {
            StartCoroutine(SpawnAdventurerRoutine());
        }

        private IEnumerator SpawnAdventurerRoutine()
        {
            while (true)
            {
                while (!_navControlPoints.IsExistAvailableServePoint())
                {
                    yield return new WaitForSeconds(_spawnInterval);
                    continue;
                }

                yield return new WaitForSeconds(_spawnInterval);
                SpawnAdventurer();
            }
        }

        private void SpawnAdventurer()
        {
            var adventurer = _adventurersPool.Get();

            var spawnPointId = _navControlPoints.GetRandomStartPoint();
            var point = _navGraph.points[spawnPointId];

            adventurer.transform.position = point;

            adventurer.GetComponent<NavGraphAgent>().Init(
                _navGraph,
                spawnPointId,
                _navControlPoints.TakeRandomServePoint()
            );

            _adventurers.Add(adventurer);
        }
    }
}