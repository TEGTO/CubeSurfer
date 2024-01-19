using DG.Tweening;
using GameplayNS.CubeTowerNS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityMethodsNS;

namespace GameplayNS
{
    public class GameStatusManager : OnEnableMethodAfterStart
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
        [SerializeField]
        private int currentScore = 0;

        public int CurrentScore { get => currentScore; }

        protected override void OnEnableAfterStart()
        {
            CubeTower.Instance.OnCubeRemove += EndGame;
            CubeTower.Instance.OnCubeAdd += CountScore;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeRemove -= EndGame;
            CubeTower.Instance.OnCubeAdd -= CountScore;
        }
        public void StartGame()
        {
            playerController.enabled = true;
        }
        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void EndGame()
        {
            if (CubeTower.Instance.CheckIfGameLost())
            {
                playerController.enabled = false;
                CubeTowerGameEnd();
                StickmanGameEnd();
            }
        }
        private void CountScore() =>
            currentScore++;
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