using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Note : MonoBehaviour {
        public int noteBeat;

        public float goalHeight = 0.0f;
        public float destroyHeight = 0.0f;
        private Vector3 _startPos = Vector3.zero;
        // Start is called before the first frame update
        public Vector3 goalPos = Vector3.zero;
        public Vector3 killPos = Vector3.zero;

        bool killMe = false;
        void Start() {
            _startPos = transform.position;
            killPos = goalPos + transform.up * 5.0f;
            //transform.LookAt(Camera.main.transform);

            this.name = noteBeat.ToString();
        }

        // Update is called once per frame
        void Update() {

            if (transform.position == goalPos) {
                killMe = true;
            }


            if (killMe) {
                transform.position = Vector3.Lerp(
                    goalPos,
                    killPos,
                    (SongConductor.Instance.beatsShownInAdvance - (noteBeat + SongConductor.Instance.beatsShownInAdvance - SongConductor.Instance.songPositionInBeats)) / SongConductor.Instance.beatsShownInAdvance
                );
                
                if (transform.position == killPos) {
                    Destroy(this.gameObject);
                }
            } else {
                transform.position = Vector3.Lerp(
                    _startPos,
                    //new Vector3(_startPos.x, goalHeight, _startPos.z),
                    goalPos,
                    (SongConductor.Instance.beatsShownInAdvance - (noteBeat - SongConductor.Instance.songPositionInBeats)) / SongConductor.Instance.beatsShownInAdvance
                );
            }
        }
    }
}

