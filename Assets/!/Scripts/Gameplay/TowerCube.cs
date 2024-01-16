using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class TowerCube : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        public void ReturnCubeToPool()
        {
            gameObject.SetActive(false);
        }
        public void SetCubeTransformInTower(Transform originTransform, float currentCubeFloor)
        {
            Vector3 originPosition = originTransform.position;
            transform.position = new Vector3(originPosition.x, originPosition.y + currentCubeFloor, originPosition.z);
            gameObject.SetActive(true);
            transform.parent = originTransform.parent;
        }
        public void RemoveCubeFromTower()
        {
            transform.parent = null;
            rigidbody.isKinematic = false;
            rigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
        }
        public void EnableInTowerCubePhysics()
        {
            rigidbody.isKinematic = false;
            rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        public void DisableCubePhysics()
        {
            rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}