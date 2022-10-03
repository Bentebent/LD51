using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class SingleTargetTower : BaseTower {

        public override void Start() {
            base.Start();

            SongConductor.Instance.Beats += OnBeat;
        }

        public override void OnDestroy() {
            base.OnDestroy();

            SongConductor.Instance.Beats -= OnBeat;
        }

        private void OnBeat(int quarterBeat) {
            if (state == TowerState.Building) {
                return;
            }

            if (targets.Count > 0 && quarterBeat == beatOffset) {
                PathingUnit target = targets.OrderBy(_ => Random.Range(0f, 1f)).FirstOrDefault();
                if (target != null) {
                    CreateBeamOneShot(target, 0.25f);
                    target.health -= Mathf.Lerp(10f, 25f, efficiency);
                    if (target.health <= 0) {
                        player.AddMoney(target.value);
                        Destroy(target.gameObject);
                    }
                }
            }
        }

        // Update is called once per frame
        protected override void Update() {
            base.Update();

            if (state == TowerState.Building) {
                return;
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
