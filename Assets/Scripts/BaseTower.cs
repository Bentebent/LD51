using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public enum TowerState {
        Building,
        Active,
        Dead
    }
    public class BaseTower : MonoBehaviour {
        public int buildCost = 0;
        public int buildValue = 0;

        public TowerState state = TowerState.Building;

        private void Start() {

        }

        private void Update() {

        }

        protected void OnTriggerEnter(Collider other) {
            Player player = other.GetComponent<Player>();
            if (player != null) { 
            }
        }

        protected void OnTriggerExit(Collider other) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
            }
        }
    }
}
