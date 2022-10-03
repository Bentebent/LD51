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
        public int cost = 0;

        public int buildProgress = 0;
        public int NotesRequiredToBuild => 25;
        protected float efficiency = 0f;

        public TowerState state = TowerState.Building;
        public TowerType type = TowerType.SingleTarget;
        public BoxCollider buildArea = null;
        public SphereCollider areaOfEffect = null;
        public GameObject visual;
        public GameObject ghost;
        private TowerLineRenderer towerLineTemplate;

        protected HashSet<PathingUnit> targets = new HashSet<PathingUnit>();
        protected int beatOffset = 0;

        private ProgressBar progressBar;
        protected Player player;
        Transform wrap;

        private void Awake() {
            buildArea = GetComponent<BoxCollider>();
            areaOfEffect = GetComponent<SphereCollider>();
            ghost.SetActive(true);
            visual.SetActive(false);

            progressBar = GetComponentInChildren<ProgressBar>();
            progressBar.SetProgress(0f);

            wrap = transform.Find("wrap");
        }

        public virtual void Start() {
            beatOffset = Random.Range(0, 4);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            AudioMixer.Instance.AddTower(this);
            towerLineTemplate = GetComponentInChildren<TowerLineRenderer>();
            towerLineTemplate.gameObject.SetActive(false);
        }

        public virtual void OnDestroy() {
            AudioMixer.Instance.RemoveTower(this);
        }

        protected virtual void Update() {
            targets.RemoveWhere(x => x == null);
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == Layers.Enemy && other.TryGetComponent(out PathingUnit pathingUnit)) {
                if (!targets.Contains(pathingUnit)) {
                    targets.Add(pathingUnit);
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == Layers.Enemy && other.TryGetComponent(out PathingUnit pathingUnit)) {
                targets.Remove(pathingUnit);
            }
        }

        protected void CreateBeamOneShot(PathingUnit unit, float duration) {
            TowerLineRenderer instance = Instantiate(towerLineTemplate);
            instance.gameObject.SetActive(true);
            instance.transform.position = towerLineTemplate.transform.position;
            instance.SetUnitTarget(unit);
            Destroy(instance.gameObject, duration);
        }

        private void UpdateEfficiencyScale(int totalNotes) {
            float efficiencyScale = efficiency / totalNotes;
            efficiencyScale = Mathf.Clamp01(efficiencyScale * efficiencyScale);
            wrap.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, efficiencyScale);
        }

        public bool AddBuildProgress(float score) {
            buildProgress++;
            progressBar.SetProgress(buildProgress / (float)NotesRequiredToBuild);
            efficiency += score;
            UpdateEfficiencyScale(buildProgress);

            if (buildProgress >= NotesRequiredToBuild) {
                state = TowerState.Active;
                AudioMixer.Instance.AddTower(this);

                ghost.SetActive(false);
                visual.SetActive(true);

                efficiency = efficiency / NotesRequiredToBuild;
                UpdateEfficiencyScale(NotesRequiredToBuild);

                wrap.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, efficiency);

                progressBar.SetProgress(0f);

                return true;
            }

            return false;
        }

    }
}
