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

        private float maxHealth = 0.0f;

        void Start() {
            maxHealth = health;
        }

        private void Update() {
            progressBar.SetProgress(health / maxHealth);
        }
    }
}