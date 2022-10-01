using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class SingleTargetTower : BaseTower {

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

            if (targets.Count > 0 && Time.time - timeSinceLastShot > 0.5f) {
                timeSinceLastShot = Time.time;

                KeyValuePair<int, PathingUnit> target = targets.ElementAt(Random.Range(0, targets.Count));
                if (target.Value != null) {
                    target.Value.health -= 50;
                    if (target.Value.health <= 0) {
                        targets.Remove(target.Key);
                        Destroy(target.Value.gameObject);
                    }
                }
                else {
                    targets.Remove(target.Key);
                }
                
            }

            Debug.Log(targets.Count);
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
        }

        protected override void OnTriggerExit(Collider other) {
            base.OnTriggerExit(other);
        }
    }
}
