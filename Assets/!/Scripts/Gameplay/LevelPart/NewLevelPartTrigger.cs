using UnityEngine;
namespace GameplayNS.LevelPartManagment
{
    public class NewLevelPartTrigger : MonoBehaviour
    {
        private const string NEW_LEVEL_PART_TAG = "NewLevelPartTrigger";

        private LevelPartSpawner levelPartSpawner;

        private void Start()
        {
            levelPartSpawner = LevelPartSpawner.Instance;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(NEW_LEVEL_PART_TAG))
                levelPartSpawner.SpawnLevelPart();
        }
    }
}