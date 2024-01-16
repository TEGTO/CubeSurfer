using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GameplayNS.CubeTowerNS
{
    public class CubeTower : MonoBehaviour
    {
        private static CubeTower instance;
        public static CubeTower Instance { get { return instance; } }

        [SerializeField]
        private TowerCube[] cubes;
        [SerializeField]
        private Transform cubeOrigin;
        [SerializeField]
        private float cubeHeight;
        [SerializeField]
        private float rigidBodiesActiveTime = 3f;

        private Coroutine rigidBodiesActiveCoroutine;
        private int amountOfActiveCubes = 1;

        public float CubeHeight { get => cubeHeight; }
        public int AmountOfActiveCubes { get => amountOfActiveCubes; }

        public event Action OnCubeAdd;
        public event Action OnCubeRemove;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        public void AddCubeToTower()
        {
            amountOfActiveCubes++;
            TowerCube newCube = GetCubeFromPool();
            if (newCube)
            {
                float currentCubeFloor = CubeHeight * AmountOfActiveCubes - 1;
                newCube.SetCubeTransformInTower(cubeOrigin, currentCubeFloor);
                OnCubeAdd?.Invoke();
            }
        }
        public void RemoveCubeFromTower(TowerCube towerCube)
        {
            amountOfActiveCubes--;
            towerCube.RemoveCubeFromTower();
            foreach (TowerCube cube in cubes.Where(x => x != towerCube))
                cube.EnableInTowerCubePhysics();
            if (rigidBodiesActiveCoroutine != null)
                StopCoroutine(rigidBodiesActiveCoroutine);
            rigidBodiesActiveCoroutine = StartCoroutine(DisableRigidBodiesAfterTime());
            StartCoroutine(ReturnObjectToPoolAfterTime(towerCube));
            OnCubeRemove?.Invoke();
        }
        private TowerCube GetCubeFromPool() =>
            cubes.FirstOrDefault(x => !x.gameObject.activeInHierarchy) ?? cubes.First();
        private IEnumerator DisableRigidBodiesAfterTime()
        {
            yield return new WaitForSeconds(rigidBodiesActiveTime);
            foreach (TowerCube cube in cubes)
                cube.DisableCubePhysics();
        }
        private IEnumerator ReturnObjectToPoolAfterTime(TowerCube cube)
        {
            yield return new WaitForSeconds(rigidBodiesActiveTime);
            cube.ReturnCubeToPool();
        }
    }
}
