using DG.Tweening;
using UnityEngine;
namespace CameraNS
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField]
        private int shakeVibration = 5;

        private Vector3 startLocalPosition;
        private Quaternion startLocalRotation;

        private void Start()
        {
            startLocalPosition = transform.localPosition;
            startLocalRotation = transform.localRotation;
        }
        public void ShakeCamera(float duration, float magnitude)
        {
            transform.DOShakePosition(duration, magnitude, shakeVibration).OnComplete(() => transform.localPosition = startLocalPosition);
            transform.DOShakeRotation(duration, magnitude, shakeVibration).OnComplete(() => transform.localRotation = startLocalRotation);
        }
    }
}