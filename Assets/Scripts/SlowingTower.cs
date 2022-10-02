using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class SlowingTower : BaseTower {

        // Update is called once per frame
        protected override void Update() {
            base.Update();

            foreach (var kvp in targets) {
                kvp.Value.speedMultiplier = 0.5f;
            }

        }

        protected override void OnTriggerExit(Collider other) {
            other.GetComponent<PathingUnit>().speedMultiplier = 1.0f;
            base.OnTriggerExit(other);
        }


    }

}
