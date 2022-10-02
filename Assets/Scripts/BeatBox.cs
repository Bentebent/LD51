using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BeatBox : MonoBehaviour {

        public KeyCode keyCode;

        private GameObject _noteInside = null;
        private float _maxDist = 0.0f;

        public GameObject notePrefab = null;


        private Player _owner = null;
        // Start is called before the first frame update
        void Start() {
            _owner = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(keyCode)) {
                if (_noteInside) {
                    float dist = Vector3.Distance(transform.position, _noteInside.transform.position);
                    _owner.AddHit(1.0f - (dist / _maxDist));
                    Destroy(_noteInside);
                    _noteInside = null;
                }
                else {
                    _owner.AddMiss(false);
                }
            }
            
        }

        private void OnTriggerEnter(Collider other) {
            _noteInside = other.gameObject;
            _maxDist = Vector3.Distance(_noteInside.transform.position, transform.position);
        }

        private void OnTriggerExit(Collider other) {
            _noteInside = null;
            _owner.AddMiss(true);
        }

        private void OnTriggerStay(Collider other) {
        }
    }
}
