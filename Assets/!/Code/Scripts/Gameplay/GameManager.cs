using DG.Tweening;
using GameplayNS.CubeTowerNS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityMethodsNS;

namespace GameplayNS
{
    public class GameManager : OnEnableMethodAfterStart
    {
        private const string HIGHESTSCORE_KEY = "hs";

        [SerializeField]
        private GameObject stickmanMainBody;
        [SerializeField]
        private GameObject stickmanRagdoll;
        [SerializeField]
        private Rigidbody stickmanRagdollMainRigidBody;
        [SerializeField]
        float stickmanImpulseForceMuliplier;
        [SerializeField]
        float stickmanMaxImpulseForce;
        [SerializeField]
        private PlayerController playerController;

        private int currentScore = 0;
        private int highestScore = 0;

        public int CurrentScore { get => currentScore; }
        public int HighestScore { get => highestScore; }

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
            ReadHighestScore();
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
                SaveHighestScore();
            }
        }
        private void CountScore() =>
            currentScore++;
        private void ReadHighestScore() =>
            highestScore = PlayerPrefs.GetInt(HIGHESTSCORE_KEY);

        private void SaveHighestScore()
        {
            if (HighestScore < currentScore)
                PlayerPrefs.SetInt(HIGHESTSCORE_KEY, CurrentScore);
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
            float forcePower = Mathf.Clamp(stickmanImpulseForceMuliplier * playerController.CurrentChracterSpeed, stickmanImpulseForceMuliplier, stickmanMaxImpulseForce);
            stickmanRagdollMainRigidBody.AddForce(stickmanRagdoll.transform.forward * forcePower, ForceMode.Impulse);
        }
    }
}