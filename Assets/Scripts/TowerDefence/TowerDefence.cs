using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class TowerDefence : MonoBehaviour {
        [SerializeField]
        private PathingUnit m_unitPrefab;

        [SerializeField]
        private Path m_path;

        [SerializeField]
        private float spawnTimer = 0.0f;

        [SerializeField]
        private int spawnMin = 0;

        [SerializeField]
        private int spawnMax = 0;

        [SerializeField]
        private int waitTime = 0;

        private List<Path.Unit> m_units = new List<Path.Unit>();

        float startTime = 0.0f;
        float lastSpawn = 0.0f;
        private void Start() {
            m_path.UnitEscaped += OnUnitEscaped;
            lastSpawn = Time.time;
            startTime = Time.time;
        }

        private void OnUnitEscaped(Path.Unit unit) {
            Destroy(unit.pathUnit.gameObject);
            m_units.Remove(unit);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                SpawnWave(Random.Range(5, 10));
            }

            if (Time.time - lastSpawn - waitTime > spawnTimer) {
                SpawnWave(Random.Range(spawnMin, spawnMax) * ((Mathf.FloorToInt(Time.timeSinceLevelLoad / 55) + 1))) ;
                lastSpawn = Time.time;
            }
        }

        public void SpawnWave(int numEnemies) {
            for (int i = 0; i < numEnemies; i++) {
                Path.Unit unit = m_path.AddUnit(Instantiate(m_unitPrefab));
                m_units.Add(unit);
            }
        }
    }
}