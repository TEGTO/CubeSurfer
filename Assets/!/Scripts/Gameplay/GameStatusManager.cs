using DG.Tweening;
using GameplayNS.CubeTowerNS;
using UnityEngine;

namespace GameplayNS
{
    public class GameStatusManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject stickmanMainBody;
        [SerializeField]
        private GameObject stickmanRagdoll;
        [SerializeField]
        private Rigidbody stickmanRagdollMainRigidBody;
        [SerializeField]
        float stickmanImpulseForce;
        [SerializeField]
        private PlayerController playerController;

        private void OnEnable()
        {
            CubeTower.Instance.OnCubeRemove += EndGame;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeRemove -= EndGame;
        }
        public void StartGame()
        {
            playerController.enabled = true;
        }
        private void EndGame()
        {
            if (CubeTower.Instance.AmountOfActiveCubes <= 0)
            {
                playerController.enabled = false;
                CubeTowerGameEnd();
                StickmanGameEnd();
            }
        }
        private void CubeTowerGameEnd()
        {
            CubeTower.Instance.StopAllCoroutines();
            CubeTower.Instance.enabled = false;
        }
        private void StickmanGameEnd()
        {
            stickmanMainBody.SetActive(false);
            stickmanRagdoll.SetActive(true);
            stickmanRagdoll.transform.DOMoveY(stickmanMainBody.transform.position.y, 0);
            stickmanRagdollMainRigidBody.AddForce(stickmanRagdoll.transform.forward * stickmanImpulseForce, ForceMode.Impulse);
        }
    }
}