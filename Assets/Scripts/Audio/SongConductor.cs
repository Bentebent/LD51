using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class SongConductor : MonoBehaviour {

        //Song beats per minute
        //This is determined by the song you're trying to sync up to
        public float songBpm;

        //The number of seconds for each song beat
        public float secPerBeat;

        //Current song position, in seconds
        public float songPosition;

        //Current song position, in beats
        public float songPositionInBeats;

        public int prevSongPosInBeatsTruncated;

        //How many seconds have passed since the song started
        public float dspSongTime;

        //The offset to the first beat of the song in seconds
        public float firstBeatOffset;

        public int beatsShownInAdvance;

        //an AudioSource attached to this GameObject that will play the music.
        public AudioSource musicSource;

        public GameObject notePrefab;

        private static SongConductor _instance = null;
        //Conductor instance
        public static SongConductor Instance => _instance;

        public static List<GameObject> spawnBeat = new List<GameObject>();

        void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
                Debug.LogWarning("A duplicate SongConductor was found");
            }
        }

        void Start() {
            //Load the AudioSource attached to the Conductor GameObject
            musicSource = GetComponent<AudioSource>();

            //Calculate the number of seconds in each beat
            secPerBeat = 60f / songBpm;

            //Record the time when the music starts
            dspSongTime = (float)AudioSettings.dspTime;

            //Start the music
            musicSource.Play();
        }

        void Update() {
            //determine how many seconds since the song started
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

            //determine how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;

            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (player.state == PlayerState.Dancing) {
                int beatToCalc = Mathf.FloorToInt(songPositionInBeats) + beatsShownInAdvance;
                if (beatToCalc - prevSongPosInBeatsTruncated > 0) {

                    GameObject spawn = GetNextNote(beatToCalc);
                    GameObject note = spawn.GetComponent<BeatBox>().notePrefab;
                    GameObject go = GameObject.Instantiate(note);

                    go.GetComponent<Note>().noteBeat = Mathf.FloorToInt(beatToCalc);
                    go.transform.position = new Vector3(spawn.transform.position.x, 0.5f, spawn.transform.position.z);
                    go.SetActive(true);
                    prevSongPosInBeatsTruncated = beatToCalc;
                }
            }
        }

        private GameObject GetNextNote(int truncatedBeatPos) {
            return spawnBeat[Random.Range(0, spawnBeat.Count)];
        }

        public void GetBeatBoxes() {
            spawnBeat.Clear();
            spawnBeat.AddRange(GameObject.FindGameObjectsWithTag("BeatBox"));
        }
    }

}