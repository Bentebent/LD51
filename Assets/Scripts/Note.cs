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

        private int[] _startPositionsX = { -5, 0, 5 };

        void Start() {
            _startPos = new Vector3(_startPositionsX[Random.Range(0, 3)], -10.0f, 0.0f);
            transform.position = _startPos;
        }

        // Update is called once per frame
        void Update() {

            float targetHeight = goalHeight;
            if (transform.position.y >= goalHeight) {

                transform.position = Vector3.Lerp(
                    new Vector3(_startPos.x, goalHeight, _startPos.z),
                    new Vector3(_startPos.x, destroyHeight, 0),
                    (SongConductor.Instance.beatsShownInAdvance - (noteBeat + SongConductor.Instance.beatsShownInAdvance - SongConductor.Instance.songPositionInBeats)) / SongConductor.Instance.beatsShownInAdvance
                );

                if (transform.position.y >= destroyHeight) {
                    Destroy(this.gameObject);
                }
            } else {
                transform.position = Vector3.Lerp(
                    _startPos,
                    new Vector3(_startPos.x, goalHeight, 0),
                    (SongConductor.Instance.beatsShownInAdvance - (noteBeat - SongConductor.Instance.songPositionInBeats)) / SongConductor.Instance.beatsShownInAdvance
                );
            }
        }
    }
}

