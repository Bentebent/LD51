using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class SlowingTower : BaseTower {

        public override void Start() {
            base.Start();

            SongConductor.Instance.Beats += OnBeat;
        }

        public override void OnDestroy() {
            base.OnDestroy();

            SongConductor.Instance.Beats -= OnBeat;
        }

        private void OnBeat(int quarterBeat) {
            if (state == TowerState.Building || quarterBeat != beatOffset)
                return;

            foreach (var target in targets) {
                CreateBeamOneShot(target, 0.25f);
                target.speedMultiplier = Mathf.Min(target.speedMultiplier, Mathf.Lerp(0.8f, 0.5f, efficiency));
            }
        }

        // Update is called once per frame
        protected override void Update() {
            base.Update();

        }

        protected override void OnTriggerExit(Collider other) {
            if (other.TryGetComponent(out PathingUnit unit))
                unit.speedMultiplier = 1.0f;
            base.OnTriggerExit(other);
        }


    }

}
