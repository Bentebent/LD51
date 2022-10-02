using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Note : MonoBehaviour {
        public int noteBeat;

        private Vector3 _startPos = Vector3.zero;
        // Start is called before the first frame update
        public Vector3 goalPos = Vector3.zero;
        //public Vector3 killPos = Vector3.zero;
        public float birthDspTime = 0f;
        public float goalDspTime = 0f;

        void Start() {
            _startPos = transform.position;
            //killPos = goalPos + transform.up * 5.0f;
            //transform.LookAt(Camera.main.transform);

            this.name = noteBeat.ToString();
        }

        float Lerp(float a, float b, float t) {
            return (1f - t) * a + b * t;
        }

        Vector3 Lerp(Vector3 a, Vector3 b, float t) {
            return (1f - t) * a + b * t;
        }

        float InvLerp(float a, float b, float value) {
            return (value - a) / (b - a);
        }

        // Update is called once per frame
        void Update() {

            float time = (float)AudioSettings.dspTime;
            float t = InvLerp(birthDspTime, goalDspTime, time);

            if (t > 1.2f) {
                Destroy(this.gameObject);
            } else {
                transform.position = Lerp(_startPos, goalPos, t);
            }
        }
    }
}

