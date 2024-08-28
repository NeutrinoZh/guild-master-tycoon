using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

using MTK.Services;

namespace GMT.GamePlay
{
    [DefaultExecutionOrder(-50)]
    public class NavControlPoints : MonoBehaviour, IService
    {
        [SerializeField] private List<int> _startPoints;
        [SerializeField] private List<int> _endPoints;
        [SerializeField] private List<int> _servePoints;
        [SerializeField] private List<int> _stayPoints;

        private List<int> _servePointsAvailable = new();
        private List<int> _stayPointsAvailable = new();

        public void AddServePoints(List<int> servePoints)
        {
            _servePoints.AddRange(servePoints);
            _servePointsAvailable.AddRange(servePoints);
        }

        public void AddStayPoints(List<int> stayPoints)
        {
            _stayPoints.AddRange(stayPoints);
            _stayPointsAvailable.AddRange(stayPoints);
        }

        public int GetRandomStartPoint()
        {
            return _startPoints[Random.Range(0, _startPoints.Count)];
        }

        public int GetRandomEndPoint()
        {
            return _endPoints[Random.Range(0, _startPoints.Count)];
        }

        public int TakeRandomServePoint()
        {
            if (_servePointsAvailable.Count == 0)
                throw new InvalidOperationException("No starting points available");

            int point = _servePointsAvailable[Random.Range(0, _servePointsAvailable.Count)];
            _servePointsAvailable.Remove(point);
            return point;
        }

        public int TakeRandomStayPoint()
        {
            if (_stayPointsAvailable.Count == 0)
                throw new InvalidOperationException("No staying points available");

            int point = _stayPointsAvailable[Random.Range(0, _stayPointsAvailable.Count)];
            _stayPointsAvailable.Remove(point);
            return point;
        }

        public bool IsItServingPoint(int pointId)
        {
            return _servePoints.Contains(pointId);
        }

        public bool IsItStayingPoint(int pointId)
        {
            return _stayPoints.Contains(pointId);
        }

        public bool IsItEndPoint(int pointId)
        {
            return _endPoints.Contains(pointId);
        }

        public void ReturnServePoint(int point)
        {
            if (!_servePoints.Contains(point))
                throw new InvalidOperationException("You are trying to return a point that is not part of list 'ServePoints'");

            if (_servePointsAvailable.Contains(point))
            {
                Debug.LogWarning("You are trying to return a point that was not taken from list 'ServePoints'");
                return;
            }

            _servePointsAvailable.Add(point);
        }

        public void ReturnStayPoint(int point)
        {
            if (!_stayPoints.Contains(point))
                throw new InvalidOperationException("You are trying to return a point that is not part of list 'StayPoints'");

            if (_stayPointsAvailable.Contains(point))
            {
                Debug.LogWarning("You are trying to return a point that was not taken from list 'StayPoints'");
                return;
            }

            _stayPointsAvailable.Add(point);
        }

        public bool IsExistAvailableStayingPoint()
        {
            return _stayPointsAvailable.Count > 0;
        }

        public bool IsExistAvailableServingPoint()
        {
            return _servePointsAvailable.Count > 0;
        }

        private void Awake()
        {
            ServiceContainer.Instance.Register(this);

            _servePointsAvailable = _servePoints.ToList();
            _stayPointsAvailable = _stayPoints.ToList();
        }
    }
}