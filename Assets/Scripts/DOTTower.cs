using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class DOTTower : BaseTower {
        float timeSinceLastShot = 0.0f;

        // Update is called once per frame
        protected override void Update() {
            base.Update();

            if (state == TowerState.Building) {
                return;
            }

            if (targets.Count > 0 && Time.time - timeSinceLastShot > 0.5f) {
                timeSinceLastShot = Time.time;

                KeyValuePair<int, PathingUnit> target = targets.ElementAt(Random.Range(0, targets.Count));
                if (target.Value != null && target.Value.gameObject.GetComponent<Poison>() == null) {
                    target.Value.gameObject.AddComponent<Poison>();
                }
                else if (target.Value == null) {
                    targets.Remove(target.Key);
                }

            }
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
        }

        protected override void OnTriggerExit(Collider other) {
            base.OnTriggerExit(other);
        }
    }

}
