using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class ScorePopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject scorePrefab;
        [SerializeField]
        private float moveYDuration = 5f;
        [SerializeField]
        private float dissapearingSpeed = 0.5f;
        [SerializeField]
        private float poolingAmount = 5f;

        private List<GameObject> scorePopupsPooling = new List<GameObject>();

        private void Start()
        {
            InitializePooling();
        }
        public void CreatePopup()
        {
            GameObject popup = scorePopupsPooling.FirstOrDefault(x => !x.activeInHierarchy) ?? scorePopupsPooling.First();
            popup.transform.DOMove(transform.position, 0f);
            popup.SetActive(true);
            popup.transform.DOMoveY(popup.transform.position.y + 100, moveYDuration);
            StartCoroutine(DissapearingEffect(popup));
        }
        private void InitializePooling()
        {
            for (int i = 0; i < poolingAmount; i++)
            {
                GameObject popup = Instantiate(scorePrefab, transform.position, Quaternion.identity, transform);
                popup.SetActive(false);
                scorePopupsPooling.Add(popup);
            }
        }
        private IEnumerator DissapearingEffect(GameObject popup)
        {
            TMP_Text popupTexMesh = UtilityNS.Utilities.GetComponentFromGameObject<TMP_Text>(popup);
            if (popupTexMesh)
            {
                Color currentColor = popupTexMesh.color;
                currentColor.a = 1f;
                while (currentColor.a > 0)
                {
                    currentColor.a -= dissapearingSpeed * Time.deltaTime;
                    popupTexMesh.color = currentColor;
                    yield return null;
                }
            }
            BackPopupToPool(popup);
        }
        private void BackPopupToPool(GameObject popup) =>
            popup.SetActive(false);
    }
}
