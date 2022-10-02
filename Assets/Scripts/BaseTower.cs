using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public enum TowerState {
        Building,
        Active,
        Dead
    }

    public enum TowerType {
        SingleTarget,
        Splash,
        DOT,
        Slow
    }

    public class BaseTower : MonoBehaviour {
        public float buildCost = 0;
        public float buildValue = 0;

        public int buildProgress = 0;
        private int notesRequiredToBuild = 25;
        float efficiency = 0f;

        public TowerState state = TowerState.Building;
        public TowerType type = TowerType.SingleTarget;
        public BoxCollider buildArea = null;
        public SphereCollider areaOfEffect = null;
        public GameObject visual;
        public GameObject ghost;

        protected Dictionary<int, PathingUnit> targets = new Dictionary<int, PathingUnit>();

        private ProgressBar progressBar;

        private void Awake() {
            buildArea = GetComponent<BoxCollider>();
            areaOfEffect = GetComponent<SphereCollider>();
            ghost.SetActive(true);
            visual.SetActive(false);

            progressBar = GetComponentInChildren<ProgressBar>();
            progressBar.SetProgress(0f);
        }

        protected virtual void Update() {
            List<int> removeIds = new List<int>();
            foreach(var kvp in targets) {
                if (kvp.Value == null) {
                    removeIds.Add(kvp.Key);
                }
            }

            removeIds.ForEach(x => targets.Remove(x));
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == Layers.Enemy) {
                if (!targets.ContainsKey(other.GetInstanceID())) {
                    targets.Add(other.GetInstanceID(), other.GetComponent<PathingUnit>());
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == Layers.Enemy) {
                targets.Remove(other.GetInstanceID());
            }
        }

        public bool AddBuildProgress(float score) {
            buildProgress++;
            progressBar.SetProgress(buildProgress / (float)notesRequiredToBuild);
            efficiency += score;
            if (buildProgress >= notesRequiredToBuild) {
                state = TowerState.Active;
                AudioMixer.Instance.AddTower(this);

                ghost.SetActive(false);
                visual.SetActive(true);

                efficiency = efficiency / notesRequiredToBuild;

                progressBar.SetProgress(0f);

                return true;
            }

            return false;
        }

    }
}
