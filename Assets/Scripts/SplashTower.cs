using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD51 {
    public class SplashTower : BaseTower {
        int quartedBeatCooldown = 16;

        public GameObject explosionPrefab = null;

        public override void Start() {
            base.Start();

            SongConductor.Instance.Beats += OnBeat;
        }

        public override void OnDestroy() {
            base.OnDestroy();
        }

        private void OnBeat(int quarterBeat) {

            if (state == TowerState.Building) {
                return;
            }

            if (targets.Count > 0 && quartedBeatCooldown <= 0) {
                quartedBeatCooldown = 16;

                PathingUnit target = targets.OrderBy(_ => Random.Range(0f, 1f)).FirstOrDefault();
                if (target != null) {
                    CreateBeamOneShot(target, 0.25f);
                    Explosion explosion = Instantiate(explosionPrefab, target.transform.position, Quaternion.identity).GetComponent<Explosion>();
                    explosion.damage = Mathf.Lerp(5f, 15f, efficiency);
                }
            }
            quartedBeatCooldown = Mathf.Max(0, quartedBeatCooldown - 1);
        }

        // Update is called once per frame
        protected override void Update() {
            base.Update();
        }
    }
}

