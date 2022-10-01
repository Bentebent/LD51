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
        public float buildCost = 0;
        public float buildValue = 0;

        public TowerState state = TowerState.Building;
        public BoxCollider buildArea = null;
        public SphereCollider areaOfEffect = null;

        private void Awake() {
            buildArea = GetComponent<BoxCollider>();
            areaOfEffect = GetComponent<SphereCollider>();
        }

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

        public bool AddBuildValue(float buildValue) {
            this.buildValue += buildValue;

            if (this.buildValue >= buildCost) {
                state = TowerState.Active;

                return true;
            }

            return false;
        }
    }
}
