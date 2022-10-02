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

        public static List<BeatBox> spawnBeat = new List<BeatBox>();

        private List<Note> _inProgressNotes = new List<Note>();

        private float _waitTimer = 0.0f;

        //the number of beats in each loop
        public float beatsPerLoop;

        //the total number of loops completed since the looping clip first started
        public int completedLoops = 0;

        //The current position of the song within the loop in beats.
        public float loopPositionInBeats;

        //The current relative position of the song within the loop measured between 0 and 1.
        public float loopPositionInAnalog;

        public delegate void BeatDelegate(int quarterBeat);
        public event BeatDelegate Beats;

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
            //songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
            songPosition = (musicSource.timeSamples + (int)(firstBeatOffset * musicSource.clip.frequency)) / (float)musicSource.clip.frequency;


            float prevSongPositionInBeats = songPositionInBeats;

            //determine how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;

            //calculate the loop position
            if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
                completedLoops++;
            loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

            int quarterBeat = Mathf.FloorToInt(songPositionInBeats * 4);
            if (Mathf.FloorToInt(prevSongPositionInBeats * 4) != Mathf.FloorToInt(songPositionInBeats * 4)) {
                Beats?.Invoke(quarterBeat%4);
            }

            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (player.state == PlayerState.Dancing) {

                if (_waitTimer == 0.0f) {
                    _waitTimer = Time.time;
                    return;
                }

                if (Time.time - _waitTimer < 1.0f)
                    return;

                int notesPerBeat = Random.Range(0f, 1f) > 0.2f ? 1 : 2;
                int beat = Mathf.FloorToInt(songPositionInBeats * notesPerBeat);
                if (Mathf.FloorToInt(prevSongPositionInBeats * notesPerBeat) != Mathf.FloorToInt(songPositionInBeats * notesPerBeat)) {
                    BeatBox beatBox = GetNextNote(beat);
                    GameObject notePrefab = beatBox.notePrefab;
                    Note noteInstance = Instantiate(notePrefab).GetComponent<Note>();

                    noteInstance.noteBeat = Mathf.FloorToInt(beat);
                    noteInstance.goalPos = beatBox.transform.position;
                    noteInstance.birthDspTime = (float)AudioSettings.dspTime;
                    noteInstance.goalDspTime = noteInstance.birthDspTime + secPerBeat * beatsShownInAdvance;
                    noteInstance.transform.position = beatBox.transform.position - beatBox.transform.up * 5;
                    noteInstance.transform.rotation = beatBox.transform.rotation;
                    noteInstance.gameObject.SetActive(true);

                    _inProgressNotes.Add(noteInstance);
                }
            } else {
                if (_inProgressNotes.Count > 0) {
                    foreach (Note note in _inProgressNotes) {
                        if (note != null) {
                            Destroy(note.gameObject);
                        }
                    }

                    _inProgressNotes.Clear();
                }

                _waitTimer = 0.0f;
            }
        }

        private BeatBox GetNextNote(int truncatedBeatPos) {
            return spawnBeat[Random.Range(0, spawnBeat.Count)];
        }

        public void GetBeatBoxes() {
            spawnBeat.Clear();
            var beats = GameObject.FindGameObjectsWithTag("BeatBox");
            foreach (var beat in beats) {
                spawnBeat.Add(beat.GetComponent<BeatBox>());
            }
        }
    }

}
