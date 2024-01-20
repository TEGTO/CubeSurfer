using UnityEngine;
using UnityEngine.Events;

namespace GameplayNS.CubeTowerNS
{
    public class UnityEventOnCubeTowerActions : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent eventOnCubeAdd;
        [SerializeField]
        private UnityEvent eventOnCubeRemove;

        private void Start()
        {
            CubeTower.Instance.OnCubeAdd += () => eventOnCubeAdd?.Invoke();
            CubeTower.Instance.OnCubeRemove += () => eventOnCubeRemove?.Invoke();
        }
    }
}