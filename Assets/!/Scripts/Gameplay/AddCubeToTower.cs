using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class AddCubeToTower : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<TowerCube>() != null)
            {
                CubeTower.Instance.AddCubeToTower();
                gameObject.SetActive(false);
                Destroy(this);
            }
        }
    }
}
