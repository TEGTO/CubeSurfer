using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class AddCubeToTower : RestartableElement
    {
        [SerializeField]
        private GameObject triggerBody;

        private bool IsEnabled = true;

        private void OnTriggerEnter(Collider other)
        {
            if (IsEnabled && other.gameObject.GetComponent<TowerCube>() != null)
            {
                CubeTower.Instance.AddCubeToTower();
                IsEnabled = false;
                triggerBody.SetActive(false);
            }
        }
        public override void RestartElement()
        {
            IsEnabled = true;
            triggerBody.SetActive(true);
        }
    }
}
