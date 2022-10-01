using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Explosion : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            Destroy(gameObject, 1.0f);
        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == Layers.Enemy) {
                other.GetComponent<PathingUnit>().health -= 25;
            }
        }

    }

}
