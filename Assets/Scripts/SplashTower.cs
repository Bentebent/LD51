using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class SplashTower : BaseTower {

        public GameObject explosionPrefab = null;
        float timeSinceLastShot = 0.0f;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        protected override void Update() {
            base.Update();

            if (state == TowerState.Building) {
                return;
            }

            if (targets.Count > 0 && Time.time - timeSinceLastShot > 2.0f) {
                timeSinceLastShot = Time.time;

                KeyValuePair<int, PathingUnit> target = targets.ElementAt(Random.Range(0, targets.Count));
                if (target.Value != null) {
                    GameObject.Instantiate(explosionPrefab, target.Value.transform.position, Quaternion.identity);
                } else {
                    targets.Remove(target.Key);
                }

            }
        }
    }
}

