using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD51 {
    public class BeatCalibrator : MonoBehaviour {
        public BeatVibeCalibrationSquash squash;
        public TMPro.TMP_Text progressText;

        float keyDownTime = 0f;
        bool keyDownThisFrame = false;

        const int totalTimingsNeeded = 20;
        int currentTimingIndex = 0;
        float[] timings = new float[totalTimingsNeeded];

        private void Start() {
            SongConductor.Instance.Beats += OnBeat;
        }

        private void OnDestroy() {
            SongConductor.Instance.Beats -= OnBeat;
        }

        private void Update() {
            progressText.text = $"Taps: {currentTimingIndex}/{totalTimingsNeeded}";
            if (currentTimingIndex >= totalTimingsNeeded || keyDownThisFrame) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow)) {
                keyDownTime = (float)AudioSettings.dspTime;
                squash.Squash();
                keyDownThisFrame = true;
            }
        }

        private void OnBeat(int quarterBeat) {
            if (quarterBeat == 0) {
                if (keyDownThisFrame) {
                    float error = ((float)AudioSettings.dspTime - keyDownTime) % SongConductor.Instance.secPerBeat;
                    error = Mathf.Min(error, SongConductor.Instance.secPerBeat - error);
                    Debug.Log(error);
                    timings[currentTimingIndex] = error;
                    currentTimingIndex++;
                    keyDownThisFrame = false;
                }

                if (currentTimingIndex >= totalTimingsNeeded) {
                    float averageError = 0f;
                    foreach (var timing in timings) {
                        averageError += timing;
                    }
                    averageError /= totalTimingsNeeded;

                    Debug.Log(averageError);
                    SongConductor.firstBeatOffset = -averageError;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}