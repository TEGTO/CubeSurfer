using UnityEngine;

namespace UnityMethodsNS
{
    public class OnEnableMethodAfterStart : MonoBehaviour
    {
        private bool isStarted = false;

        protected void Start()
        {
            isStarted = true;
            OnEnableAfterStart();
        }
        private void OnEnable()
        {
            if (isStarted)
                OnEnableAfterStart();
        }
        protected virtual void OnEnableAfterStart() { }
    }
}