using AxisNS;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityNS;

namespace GameplayNS.LevelPartManagment
{
    [Serializable]
    public class LevelPart
    {
        public GameObject Prefab;
        public float Length;
        public int PoolAmount = 4;
        [HideInInspector]
        public GameObject InstantiatedPrefab;
        public LevelPart(LevelPart referenceLevelPart, Transform prefabParent)
        {
            Length = referenceLevelPart.Length;
            InstantiatedPrefab = GameObject.Instantiate(referenceLevelPart.Prefab, prefabParent);
            InstantiatedPrefab.SetActive(false);
        }
    }
    public class LevelPartSpawner : MonoBehaviour
    {
        private static LevelPartSpawner instance;
        public static LevelPartSpawner Instance
        {
            get { return instance; }
        }

        [SerializeField]
        private Axis spawnDirection;
        [SerializeField]
        private Axis upDirection;
        [SerializeField]
        private float spawnDuration = 5f;
        [SerializeField]
        private int amountLevelsOnStart = 4;
        [SerializeField]
        private int maxAmountOfActiveParts = 4;
        [SerializeField]
        private LevelPart[] levelParts;

        private List<LevelPart> instantiatedLevelParts = new List<LevelPart>();
        private Queue<LevelPart> activeLevelParts = new Queue<LevelPart>();
        private float currentAxisSpawnPoint = 0;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
            InitiazliePool();
        }
        private void Start()
        {
            for (int i = 0; i < amountLevelsOnStart; i++)
                SpawnLevelPart(spawnDuration: 0);
        }
        public void SpawnLevelPart() =>
            SpawnLevelPart(spawnDuration);
        private void SpawnLevelPart(float spawnDuration)
        {
            AddOldestLevelPartToPool();
            LevelPart randomLevelPartFromPool = GetFromPoolRandomLevelPart();
            MoveLevelPart(randomLevelPartFromPool, spawnDuration);
            activeLevelParts.Enqueue(randomLevelPartFromPool);
            randomLevelPartFromPool.InstantiatedPrefab.SetActive(true);
        }
        private void MoveLevelPart(LevelPart levelPart, float spawnDuration)
        {
            Vector3 animationStartPosition = -100 * Utilities.DefineDirection(upDirection) + levelPart.InstantiatedPrefab.transform.position;
            Vector3 levelSpawnPosition = Utilities.DefineDirection(spawnDirection) * currentAxisSpawnPoint;
            levelPart.InstantiatedPrefab.transform.DOMove(animationStartPosition, 0);
            levelPart.InstantiatedPrefab.transform.DOMove(levelSpawnPosition, spawnDuration);
            currentAxisSpawnPoint += levelPart.Length;
        }
        private void InitiazliePool()
        {
            foreach (var levelPart in levelParts)
            {
                for (int i = 0; i < levelPart.PoolAmount; i++)
                    InstantiateNewLevelPart(levelPart);
            }
        }
        private void InstantiateNewLevelPart(LevelPart levelReference)
        {
            LevelPart instantiatedNewLevelPart = new LevelPart(levelReference, transform);
            instantiatedLevelParts.Add(instantiatedNewLevelPart);
        }
        private LevelPart GetFromPoolRandomLevelPart()
        {
            LevelPart[] levelPartsInPool = instantiatedLevelParts.Where(x => !x.InstantiatedPrefab.activeInHierarchy).ToArray();
            return levelPartsInPool[UnityEngine.Random.Range(0, levelPartsInPool.Length)];
        }
        private void AddOldestLevelPartToPool()
        {
            if (activeLevelParts.Count > maxAmountOfActiveParts)
            {
                LevelPart levelPartToPool = activeLevelParts.Dequeue();
                levelPartToPool.InstantiatedPrefab.SetActive(false);
            }
        }
    }
}