using UnityEngine;

namespace GMT.GamePlay
{
    [CreateAssetMenu(fileName = "Department", menuName = "SO/New Department")]
    public class DepartmentSO : ScriptableObject
    {
        [field: SerializeField] public BuildingSO Building { get; private set; }
        [field: SerializeField] public int DirectionOfExpansion { get; private set; }
        [field: SerializeField] public int NumBuildings { get; private set; }
    }
}