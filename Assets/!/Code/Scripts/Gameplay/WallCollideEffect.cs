using CameraNS;
using GameplayNS.CubeTowerNS;
using UnityEngine;
using UnityMethodsNS;

namespace GameplayNS
{
    public class WallCollideEffect : OnEnableMethodAfterStart
    {
        [SerializeField]
        private CameraShaker cameraShake;
        [SerializeField]
        private float shakeDuration = 1f;
        [SerializeField]
        private float shakeMagnitude = 1f;

        protected override void OnEnableAfterStart() =>
            CubeTower.Instance.OnCubeRemove += OnWallCollide;
        private void OnDisable() =>
            CubeTower.Instance.OnCubeRemove -= OnWallCollide;
        private void OnWallCollide()
        {
            cameraShake.ShakeCamera(shakeDuration, shakeMagnitude);
#if !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
