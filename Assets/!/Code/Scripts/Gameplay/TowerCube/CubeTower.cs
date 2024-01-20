using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class CubeTower : MonoBehaviour
    {
        private static CubeTower instance;
        public static CubeTower Instance { get => instance; }

        [SerializeField]
        private GameObject cubeHolderPrefab;
        [SerializeField]
        private int maxAmountOfCubes = 10;
        [SerializeField]
        private Transform cubeOrigin;
        [SerializeField]
        private float cubeHeight;
        [SerializeField]
        private float physicsActiveTime = 3f;

        private List<TowerCube> cubes = new List<TowerCube>();
        private Queue<TowerCube> cubePool = new Queue<TowerCube>();
        private Coroutine rigidBodiesActiveCoroutine;
        private int amountOfCubesInTower = 0;

        public float CubeHeight { get => cubeHeight; }
        public int AmountOfCubesInTower { get => amountOfCubesInTower; }
        public int MaxAmountOfCubes { get => maxAmountOfCubes; }

        public event Action OnCubeAdd;
        public event Action OnCubeRemove;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        private void Start()
        {
            InitializePool();
            AddCubeToTower(false);
        }
        public void AddCubeToTower(bool invokeEvent = true)
        {
            if (amountOfCubesInTower < maxAmountOfCubes)
            {
                amountOfCubesInTower++;
                TowerCube newCube = GetCubeFromPool();
                newCube.AddCubeToTower();
                DefinePositionInTowerCubes();
                if (invokeEvent)
                    OnCubeAdd?.Invoke();
            }
        }
        public void RemoveCubeFromTower(GameObject hitFromGameObject)
        {
            TowerCube towerCube = FindHittedTowerCube(hitFromGameObject);
            if (towerCube)
            {
                amountOfCubesInTower--;
                towerCube.RemoveCubeFromTower();
                AddCubeToPool(towerCube);
                EnableAndDisableCubePhysics();
            }
            OnCubeRemove?.Invoke();
        }
        public bool CheckIfGameLost() =>
            AmountOfCubesInTower == 0;
        private void InitializePool()
        {
            for (int i = 0; i < maxAmountOfCubes; i++)
            {
                GameObject cubeGO = GameObject.Instantiate(cubeHolderPrefab, transform);
                TowerCube towerCube = UtilityNS.Utilities.GetComponentFromGameObject<TowerCube>(cubeGO);
                cubes.Add(towerCube);
                cubeGO.SetActive(false);
                AddCubeToPool(towerCube);
            }
        }
        private void AddCubeToPool(TowerCube towerCube) =>
            cubePool.Enqueue(towerCube);
        private TowerCube GetCubeFromPool() =>
            cubePool.Dequeue();
        private void DefinePositionInTowerCubes()
        {
            int towerFloor = 0;
            TowerCube[] inTowerCubes = GetActiveCubesInTower();
            foreach (var cube in inTowerCubes)
            {
                cube.DisableCubePhysics();
                cube.SetCubeTransformInTower(cubeOrigin, towerFloor);
                towerFloor++;
            }
        }
        private TowerCube FindHittedTowerCube(GameObject hitFromGameObject)
        {
            TowerCube[] activeTowerCubes = GetActiveCubesInTower();
            Vector3 positionGO = hitFromGameObject.transform.position;
            TowerCube closestCube = activeTowerCubes.OrderBy(x => Vector3.Distance(x.gameObject.transform.position, positionGO)).FirstOrDefault();
            return closestCube;
        }
        private void EnableAndDisableCubePhysics()
        {
            TowerCube[] inTowerCubes = GetActiveCubesInTower();
            foreach (TowerCube cube in inTowerCubes)
                cube.EnableInTowerCubePhysics();
            if (rigidBodiesActiveCoroutine != null)
                StopCoroutine(rigidBodiesActiveCoroutine);
            rigidBodiesActiveCoroutine = StartCoroutine(DisablePhysicsAfterTime());
        }
        private TowerCube[] GetActiveCubesInTower() =>
            cubes.Where(x => x.gameObject.activeInHierarchy && x.CubeInTower).ToArray();
        private IEnumerator DisablePhysicsAfterTime()
        {
            yield return new WaitForSeconds(physicsActiveTime);
            foreach (TowerCube cube in cubes)
                cube.DisableCubePhysics();
        }
    }
}
