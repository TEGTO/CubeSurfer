using DG.Tweening;
using UnityEngine;
namespace GameplayNS.CubeTowerNS
{
    public class StickmanControlling : MonoBehaviour
    {
        public const string STICKMAN_JUMPING_ANIMATION = "Jumping";

        [SerializeField]
        private GameObject stickman;
        [SerializeField]
        private Animator stickManAnimator;
        [SerializeField]
        private Rigidbody stickmanRigidbody;
        [SerializeField]
        private float jumpDuration = 0.5f;

        private void OnEnable()
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
            float newStickmanYPosition = CubeTower.Instance.CubeHeight * CubeTower.Instance.AmountOfActiveCubes;
            stickman.transform.DOMoveY(newStickmanYPosition, jumpDuration);
        }
        private void MoveDown()
        {
            stickmanRigidbody.isKinematic = false;
            stickmanRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }
}