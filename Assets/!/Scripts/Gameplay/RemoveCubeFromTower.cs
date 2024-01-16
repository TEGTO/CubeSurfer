using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class RemoveCubeFromTower : MonoBehaviour
    {
        private bool isEnabled = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (isEnabled && collision.gameObject.TryGetComponent(out TowerCube towerCube))
            {
                isEnabled = false;
                CubeTower.Instance.RemoveCubeFromTower(towerCube);
                Destroy(this);
            }
        }
    }
}