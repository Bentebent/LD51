using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class SlowingTower : BaseTower {

        // Update is called once per frame
        protected override void Update() {
            base.Update();

            if (state == TowerState.Building)
                return;

            foreach (var kvp in targets) {
                kvp.Value.speedMultiplier = 0.5f;
            }

        }

        protected override void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<PathingUnit>(out PathingUnit unit))
                unit.speedMultiplier = 1.0f;
            base.OnTriggerExit(other);
        }


    }

}
