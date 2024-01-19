using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityMethodsNS;

namespace GameplayNS.CubeTowerNS
{
    public class VisualizedTextOfCubeAmount : OnEnableMethodAfterStart
    {
        [SerializeField]
        private TextMeshProUGUI cubeLimitText;
        [SerializeField]
        private Vector3 textGameObjectAnimationRotation;
        [SerializeField]
        private float animationDuration = 3f;

        private int maxAmountOfCubes;
        private int currentAmountOfCubes = 1;

        protected override void OnEnableAfterStart()
        {
            CubeTower.Instance.OnCubeAdd += OnCubeAmountChange;
            CubeTower.Instance.OnCubeRemove += OnCubeAmountChange;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeAdd -= OnCubeAmountChange;
            CubeTower.Instance.OnCubeRemove -= OnCubeAmountChange;
        }
        private new void Start()
        {
            base.Start();
            CubeLimitTextAnimation();
            maxAmountOfCubes = CubeTower.Instance.MaxAmountOfCubes;
            cubeLimitText.text = $"{currentAmountOfCubes}/{maxAmountOfCubes}";
        }
        private void OnCubeAmountChange()
        {
            currentAmountOfCubes = CubeTower.Instance.AmountOfCubesInTower;
            cubeLimitText.text = $"{currentAmountOfCubes}/{maxAmountOfCubes}";
        }
        private void CubeLimitTextAnimation()
        {
            cubeLimitText.gameObject.transform
                .DORotate(textGameObjectAnimationRotation, animationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
    }
}