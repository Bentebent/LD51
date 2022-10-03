using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class DOTTower : BaseTower {
        int quartedBeatCooldown = 16;

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

            if (targets.Count > 0 && quartedBeatCooldown <= 0) {
                quartedBeatCooldown = 16;

                // Find a random target without the poison component
                PathingUnit target = targets
                    .Where(x => x?.TryGetComponent<Poison>(out _) == false)
                    .OrderBy(_ => Random.Range(0f, 1f))
                    .FirstOrDefault();

                if (target != null) {
                    CreateBeamOneShot(target, 0.25f);
                    Poison poison = target.gameObject.AddComponent<Poison>();
                    poison.damagePerTick = 5f;
                    poison.duration = Mathf.Lerp(3f, 6f, efficiency * efficiency);
                }

            }
            quartedBeatCooldown = Mathf.Max(0, quartedBeatCooldown - 1);
        }

        // Update is called once per frame
        protected override void Update() {
            base.Update();
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
        }

        protected override void OnTriggerExit(Collider other) {
            base.OnTriggerExit(other);
        }
    }

}
