using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Poison : MonoBehaviour {
        // Start is called before the first frame update
        public float lastTick = 0.0f;
        public float startTime = 0.0f;
        void Start() {
            startTime = Time.time;
        }

        // Update is called once per frame
        void Update() {
            if (Time.time - lastTick > 0.25f) {
                var unit = gameObject.GetComponent<PathingUnit>();
                unit.health -= 5;

                if (unit.health <= 0) {
                    Destroy(unit.gameObject);
                }

                lastTick = Time.time;
            }

            if (Time.time - startTime > 3.0f) {
                Destroy(this);
            }
        }
    }
}

