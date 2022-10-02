using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BeatVibe : MonoBehaviour {
        public Transform target;
        public float amount = 0.1f;
        public int beat = 0;

        private void Awake() {
            if (target == null) {
                target = transform;
            }
        }

        public virtual void Start() {
            SongConductor.Instance.Beats += OnBeat;
        }

        public virtual void OnDestroy() {
            SongConductor.Instance.Beats -= OnBeat;
        }

        private void OnBeat(int quarterBeat) {
            if (quarterBeat == beat) {
                target.localScale = new Vector3(1f, Mathf.Clamp01(1f - amount), 1f);
            }
        }

        private void Update() {
            target.localScale = Vector3.MoveTowards(target.localScale, Vector3.one, 1.0f * Time.deltaTime);
        }
    }
}