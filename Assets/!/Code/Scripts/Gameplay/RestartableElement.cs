using UnityEngine;
namespace GameplayNS
{
    public abstract class RestartableElement : MonoBehaviour
    {
        protected void OnEnable()
        {
            RestartElement();
        }
        public abstract void RestartElement();
    }
}