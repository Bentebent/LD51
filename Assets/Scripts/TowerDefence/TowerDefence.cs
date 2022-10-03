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

        private Player player = null;

        float startTime = 0.0f;
        float lastSpawn = 0.0f;
        private void Start() {
            m_path.UnitEscaped += OnUnitEscaped;
            lastSpawn = Time.time;
            startTime = Time.time;

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        public void RegisterOnUnitEscaped(Path.UnitEscapedDelegate foo) {
            m_path.UnitEscaped += foo;
        }

        private void OnUnitEscaped(Path.Unit unit) {
            Destroy(unit.pathUnit.gameObject);
            m_units.Remove(unit);
        }

        private void Update() {
            if (Time.time - lastSpawn - waitTime > spawnTimer && player.IBUILTATOWER) {
                SpawnWave(Random.Range(spawnMin, spawnMax) * ((Mathf.FloorToInt(Time.timeSinceLevelLoad / 55) * 2 + 1))) ;
                lastSpawn = Time.time;
            }
        }

        public void SpawnWave(int numEnemies) {
            StartCoroutine(Spawn(numEnemies));
        }
        
        private IEnumerator Spawn(int numEnemies) {
            int remainingEnemies = numEnemies;

            while(remainingEnemies > 0) {
                int waveSize = Random.Range(1, Mathf.Min(Mathf.CeilToInt(numEnemies / 5), remainingEnemies));

                for (int i = 0; i < waveSize; i++) {
                    Path.Unit unit = m_path.AddUnit(Instantiate(m_unitPrefab));
                    m_units.Add(unit);
                }

                remainingEnemies -= waveSize;

                if (remainingEnemies <= 0)
                    break;

                yield return new WaitForSeconds(1.0f);
            }

            yield return null;
           
        }
    }
}