using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BeatBox : MonoBehaviour {

        public KeyCode keyCode;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter(Collider other) {
            Debug.Log("Ayy lmao");
        }

        private void OnTriggerStay(Collider other) {
            if (Input.GetKey(keyCode)) {
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
