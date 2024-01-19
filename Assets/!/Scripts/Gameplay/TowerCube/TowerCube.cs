using System.Collections;
using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class TowerCube : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;

        private bool cubeInTower = true;
        private Coroutine returnToPoolCoroutine;

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
        public void SetCubeTransformInTower(Transform originTransform, float currentCubeTowerHeight)
        {
            Vector3 originPosition = originTransform.position;
            transform.position = new Vector3(originPosition.x, originPosition.y + currentCubeTowerHeight, originPosition.z);
            transform.parent = originTransform.parent;
        }
        public void ReturnCubeToPoolAfterTime(float afterTime)
        {
            if (returnToPoolCoroutine != null)
                StopCoroutine(returnToPoolCoroutine);
            returnToPoolCoroutine = StartCoroutine(ReturnCubeToPool(afterTime));
        }
        private IEnumerator ReturnCubeToPool(float afterTime)
        {
            yield return new WaitForSeconds(afterTime);
            if (!CubeInTower)
                gameObject.SetActive(false);
        }
    }
}