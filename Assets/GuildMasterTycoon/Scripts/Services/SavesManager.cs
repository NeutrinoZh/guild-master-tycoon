using System.Collections.Generic;
using System;

using UnityEngine;

using MTK.SaveSystem;
using MTK.Services;
using MTK.Timer;

namespace GMT.GamePlay
{
    public class SavesManager : IService
    {
        public event Action OnBuildingPurchase;
        public event Action<int, int, int> OnWorkerPurchase;

        public int Money
        {
            get => _gameData.money;
            set
            {
                _gameData.money = value;
                _isDirty = true;
            }
        }

        public void PurchaseBuilding(int departmentId, int buildingId)
        {
            _gameData.departments[departmentId].buildings[buildingId].purchased = true;
            _isDirty = true;

            OnBuildingPurchase?.Invoke();
        }

        public void PurchaseWorker(int departmentId, int buildingId, int workerId)
        {
            if (IsWorkerPurchased(departmentId, buildingId, workerId))
                return;

            _gameData.departments[departmentId].buildings[buildingId].workers.Add(workerId);
            _isDirty = true;

            OnWorkerPurchase?.Invoke(departmentId, buildingId, workerId);
        }

        public void CompleteTutorial()
        {
            _gameData.isTutorialCompleted = true;
            _isDirty = true;
        }

        public bool IsBuildingPurchased(int departmentId, int buildingId)
        {
            return _gameData.departments[departmentId].buildings[buildingId].purchased;
        }

        public bool IsWorkerPurchased(int departmentId, int buildingId, int workerId)
        {
            return _gameData.departments[departmentId].buildings[buildingId].workers.Contains(workerId);
        }

        public bool IsTutorialCompleted()
        {
            return _gameData.isTutorialCompleted;
        }

        public int CountOfPurchasedBuldings()
        {
            int count = 0;
            foreach (var department in _gameData.departments)
                foreach (var building in department.buildings)
                    count += building.purchased ? 1 : 0;
            return count;
        }

        public SavesManager()
        {
            _saveSystem = new SaveSystem(Application.persistentDataPath, new JSONSerializer());

            try
            {
                _gameData = _saveSystem.Load<GameData>(k_gameDataKey);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Couldn't load GameData: {ex}");
                _gameData = k_playerScoresDefaultData;
            }

            /*
                There will be migrations
            */

            ServiceContainer.Instance.SetCallback<MTKTimer>(() =>
            {
                ServiceContainer.Instance.Get<MTKTimer>().StartInterval(SaveInterval, k_saveInterval);
            });
        }

        [Serializable]
        private class Building
        {
            public bool purchased;
            public List<int> workers;
        }

        [Serializable]
        private class Department
        {
            public List<Building> buildings;
        }

        [Serializable]
        private class GameData
        {
            public bool isTutorialCompleted;
            public int money;
            public List<Department> departments;
        }

        private const float k_saveInterval = 1;
        private const string k_gameDataKey = "gameData";
        private static readonly GameData k_playerScoresDefaultData = new()
        {
            money = 100,
            isTutorialCompleted = false,
            departments = new()
            {
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = true,
                            workers=new(){0}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        }
                    }
                },
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        }
                    }
                },
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                    }
                },
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                    }
                },
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        }
                    }
                },
                new()
                {
                    buildings=new()
                    {
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        },
                        new()
                        {
                            purchased = false,
                            workers=new(){}
                        }
                    }
                },
            }
        };

        private GameData _gameData;
        private SaveSystem _saveSystem;
        private bool _isDirty = false;

        private void SaveInterval()
        {
            if (_isDirty)
            {
                _isDirty = false;
                _saveSystem.Save(k_gameDataKey, _gameData);
            }
        }
    }
}