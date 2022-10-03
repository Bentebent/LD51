using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BeatVibeCalibrationSquash : MonoBehaviour {
        public Transform target;
        public float amount = 0.2f;

        private void Awake() {
            if (target == null) {
                target = transform;
            }
        }

        public void Squash() {
            target.localScale = new Vector3(1f, Mathf.Clamp01(1f - amount), 1f);
        }

        private void Update() {
            target.localScale = Vector3.MoveTowards(target.localScale, Vector3.one, 1.0f * Time.deltaTime);
        }
    }
}