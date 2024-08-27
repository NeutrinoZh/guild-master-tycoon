using UnityEngine;

namespace GMT.GamePlay
{
    [CreateAssetMenu(fileName = "Building", menuName = "SO/New Building")]
    public class BuildingSO : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int TablePrice { get; private set; }
    }
}