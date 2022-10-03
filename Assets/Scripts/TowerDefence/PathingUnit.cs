using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class PathingUnit : MonoBehaviour {
        public float speed = 10.0f;
        public float pathRadius = 5f;

        public float health = 100.0f;
        public float speedMultiplier = 1.0f;
        public int value = 100;

        public ProgressBar progressBar = null;
        public Transform visual;

        private float maxHealth = 0.0f;

        private float randOffs;
        void Start() {
            maxHealth = health;
            visual = transform.Find("Visual");
            randOffs = Random.Range(0f, 100f);
        }

        private void Update() {
            progressBar.SetProgress(health / maxHealth);

            //if (_currentVelocity.magnitude > 0.1f) {
            float sine = Mathf.Sin(Time.time * speed * 10f * speedMultiplier + randOffs);
            visual.transform.localPosition = Vector3.up * Mathf.Abs(sine) * 0.2f;
            visual.transform.localEulerAngles = new Vector3(0f, 0f, sine * 5f);
            //} else {
            //    visual.transform.localPosition = Vector3.zero;
            //    visual.transform.localEulerAngles = Vector3.zero;
            //}
        }
    }
}