using SoundNS;
using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class RemoveCubeFromTower : RestartableElement
    {
        private bool isEnabled = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (CheckConditions(collision))
                RemoveCube();
        }
        public override void RestartElement()
        {
            isEnabled = true;
        }
        private bool CheckConditions(Collision collision) =>
            isEnabled && collision.gameObject.TryGetComponent(out TowerCube towerCube) && towerCube.CubeInTower;
        private void RemoveCube()
        {
            CubeTower.Instance.RemoveCubeFromTower(gameObject);
            isEnabled = false;
            SoundManager.Instance.PlayCollisionWithWall();
        }
    }
}