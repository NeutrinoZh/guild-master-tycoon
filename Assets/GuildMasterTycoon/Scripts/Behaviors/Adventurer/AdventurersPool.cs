using System.Collections.Generic;
using System.Collections;

using UnityEngine.Pool;
using UnityEngine;

using MTK.Services;
using MTK;

using GMT.GamePlay;

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

        public void ReturnAdventurer(Transform adventurer)
        {
            _adventurers.Remove(adventurer);
            _adventurersPool.Release(adventurer);
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
                while (!_navControlPoints.IsExistAvailableServingPoint() && !_navControlPoints.IsExistAvailableStayingPoint())
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

            var navAgent = adventurer.GetComponent<NavGraphAgent>();
            navAgent.Init(
                _navGraph,
                spawnPointId
            );

            if (_navControlPoints.IsExistAvailableServingPoint())
                navAgent.MoveTo(
                    _navControlPoints.TakeRandomServePoint()
                );
            else
                navAgent.MoveTo(
                    _navControlPoints.TakeRandomStayPoint()
                );

            adventurer.GetComponent<Adventurer>().Init(
                ServiceContainer.Instance.Get<PlayerStats>(),
                ServiceContainer.Instance.Get<NavControlPoints>()
            );

            _adventurers.Add(adventurer);
        }
    }
}