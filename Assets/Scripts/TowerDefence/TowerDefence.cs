using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class TowerDefence : MonoBehaviour {
        [SerializeField]
        private PathingUnit m_unitPrefab;

        [SerializeField]
        private Path m_path;

        private List<Path.Unit> m_units = new List<Path.Unit>();

        private void Start() {
            m_path.UnitEscaped += OnUnitEscaped;
        }

        private void OnUnitEscaped(Path.Unit unit) {
            Destroy(unit.pathUnit.gameObject);
            m_units.Remove(unit);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                SpawnWave(Random.Range(5, 10));
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