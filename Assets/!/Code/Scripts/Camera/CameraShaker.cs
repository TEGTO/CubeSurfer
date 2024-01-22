using DG.Tweening;
using UnityEngine;
namespace CameraNS
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField]
        private int shakeVibration = 5;
        [SerializeField]
        private float toDefaultValuesDuration = 1f;

        private Quaternion startLocalRotation;
        private Tweener currentTween;

        private void Start()
        {
            startLocalRotation = transform.localRotation;
        }
        public void ShakeCamera(float duration, float magnitude)
        {
            if (currentTween.IsActive())
                currentTween.Kill();
            currentTween = transform.DOShakeRotation(duration, magnitude, shakeVibration).OnComplete(() => transform.DOLocalRotate(startLocalRotation.eulerAngles, toDefaultValuesDuration));
        }
    }
}