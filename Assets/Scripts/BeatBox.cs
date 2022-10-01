using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BeatBox : MonoBehaviour {

        public KeyCode keyCode;

        private GameObject _noteInside = null;

        public GameObject notePrefab = null;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (_noteInside && Input.GetKeyDown(keyCode)) {
                Destroy(_noteInside);
                _noteInside = null;
            }
        }

        private void OnTriggerEnter(Collider other) {
            _noteInside = other.gameObject;
        }

        private void OnTriggerExit(Collider other) {
            _noteInside = null;
        }

        private void OnTriggerStay(Collider other) {
        }
    }
}
