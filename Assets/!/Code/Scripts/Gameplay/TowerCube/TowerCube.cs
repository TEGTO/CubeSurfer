using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class TowerCube : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        private bool cubeInTower = true;

        public bool CubeInTower
        {
            get { return cubeInTower; }
        }

        public void AddCubeToTower()
        {
            cubeInTower = true;
            gameObject.SetActive(true);
        }
        public void RemoveCubeFromTower()
        {
            cubeInTower = false;
            transform.parent = null;
            //rigidbody.isKinematic = false;
            rigidbody.constraints &= ~RigidbodyConstraints.FreezePosition;
        }
        public void EnableInTowerCubePhysics()
        {
            //rigidbody.isKinematic = false;
            rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        public void DisableCubePhysics()
        {
            //rigidbody.isKinematic = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        public void SetCubeTransformInTower(Transform originTransform, float currentCubeTowerHeight)
        {
            Vector3 originPosition = originTransform.position;
            transform.position = new Vector3(originPosition.x, originPosition.y + currentCubeTowerHeight, originPosition.z);
            transform.parent = originTransform.parent;
        }
    }
}