using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace LD51 {

    [Serializable]
    public class TowerTrack {
        public List<GameObject> towers = new List<GameObject>();
        public AudioSource track = null;
        public TowerType type;
    }

    public class AudioMixer : MonoBehaviour {

        private static AudioMixer _instance = null;
        public static AudioMixer Instance => _instance;

        public List<TowerTrack> tracks = new List<TowerTrack>();

        private Player player = null;
        void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
                Debug.LogWarning("A duplicate SongConductor was found");
            }
        }

        public void AddTower(BaseTower tower) {
            foreach (var track in tracks) {
                if (tower.type == track.type) {
                    track.towers.Add(tower.gameObject);
                }
            }
        }

        public void RemoveTower(BaseTower tower) {
            foreach (var track in tracks) {
                if (tower.type == track.type) {
                    track.towers.Remove(tower.gameObject);
                }
            }
        }

        // Start is called before the first frame update
        void Start() {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update() {
            foreach (var track in tracks) {
                float  minDist = Mathf.Infinity;
                foreach (var tower in track.towers) {
                    float dist = Vector3.Distance(player.transform.position, tower.transform.position);
                    if (dist < minDist) {
                        minDist = dist;
                    }
                }

                float vol = scale(0.0f, 10.0f, 0.2f, 0.0f, minDist);
                track.track.volume = vol;
            }
        }

        public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue) {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

    }
}

