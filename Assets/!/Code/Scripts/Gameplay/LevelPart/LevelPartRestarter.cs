using UnityEngine;
namespace GameplayNS.LevelPartManagment
{
    public class LevelPartRestarter : MonoBehaviour
    {
        [SerializeField]
        private RestartableElement[] restartableElements;

        private void OnEnable()
        {
            foreach (var restartableElement in restartableElements)
                restartableElement.RestartElement();
        }
    }
}