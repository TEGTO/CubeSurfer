using UnityEngine;
using UnityEngine.Events;

namespace GameplayNS.CubeTowerNS
{
    public class UnityEventOnCubeCollection : MonoBehaviour
    {
        [SerializeField]
        private CubeTower cubeTower;
        [SerializeField]
        private UnityEvent unityEvent;

        private void Start() =>
            cubeTower.OnCubeAdd += () => unityEvent?.Invoke();
    }
}