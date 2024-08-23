using UnityEngine;

namespace GMT.GamePlay
{
    [CreateAssetMenu(fileName = "Building", menuName = "SO/New Building")]
    public class BuildingSO : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}