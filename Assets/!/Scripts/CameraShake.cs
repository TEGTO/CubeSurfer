using System.Collections;
using UnityEngine;
namespace CameraNS
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float magnitude;

        private Coroutine shakeCoroutine;

        public void ShakeCamera(float duration)
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(Shake(duration));
        }
        private IEnumerator Shake(float duration)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = originalPos;
        }
    }
}