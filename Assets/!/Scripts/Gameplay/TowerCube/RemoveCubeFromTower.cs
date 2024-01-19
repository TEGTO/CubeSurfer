using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class RemoveCubeFromTower : RestartableElement
    {
        private bool isEnabled = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (CheckConditions(collision))
            {
                CubeTower.Instance.RemoveCubeFromTower(gameObject);
                isEnabled = false;
            }
        }
        public override void RestartElement()
        {
            isEnabled = true;
        }
        private bool CheckConditions(Collision collision) =>
            isEnabled && collision.gameObject.TryGetComponent(out TowerCube towerCube);
    }
}