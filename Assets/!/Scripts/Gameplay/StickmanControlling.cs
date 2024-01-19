using DG.Tweening;
using UnityEngine;
using UnityMethodsNS;

namespace GameplayNS.CubeTowerNS
{
    public class StickmanControlling : OnEnableMethodAfterStart
    {
        public const string STICKMAN_JUMPING_ANIMATION = "Jumping";

        [SerializeField]
        private GameObject stickman;
        [SerializeField]
        private Animator stickManAnimator;
        [SerializeField]
        private Rigidbody stickmanRigidbody;
        [SerializeField]
        private float heightOffest = 0.1f;
        [SerializeField]
        private float jumpDuration = 0.5f;

        protected override void OnEnableAfterStart()
        {
            CubeTower.Instance.OnCubeAdd += MoveUp;
            CubeTower.Instance.OnCubeRemove += MoveDown;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeAdd -= MoveUp;
            CubeTower.Instance.OnCubeRemove -= MoveDown;
        }
        private void MoveUp()
        {
            stickmanRigidbody.isKinematic = true;
            stickmanRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            MoveTransformUp();
            stickManAnimator.Play(STICKMAN_JUMPING_ANIMATION);
        }
        private void MoveTransformUp()
        {
            float newStickmanYPosition = CubeTower.Instance.CubeHeight * CubeTower.Instance.AmountOfCubesInTower + heightOffest;
            stickman.transform.DOMoveY(newStickmanYPosition, jumpDuration);
        }
        private void MoveDown()
        {
            stickmanRigidbody.isKinematic = false;
            stickmanRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }
}