using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD51 {
    public class BeatCalibrator : MonoBehaviour {
        public BeatVibeCalibrationSquash squash;
        public TMPro.TMP_Text progressText;

        private float averageError = 0f;
        float keyDownTime = 0f;

        int taps = 0;
        int totalTapsNeeded = 20;

        bool keyDownThisFrame = false;

        private void Start() {
            SongConductor.Instance.Beats += OnBeat;
        }

        private void OnDestroy() {
            SongConductor.Instance.Beats -= OnBeat;
        }

        private void Update() {
            if (Input.anyKeyDown) {
                keyDownTime = (float)AudioSettings.dspTime;
                squash.Squash();
                taps++;
                keyDownThisFrame = true;
            }
            progressText.text = $"Taps: {taps}/{totalTapsNeeded}";
        }

        private void OnBeat(int quarterBeat) {
            if (quarterBeat == 0) {
                if (keyDownThisFrame) {
                    float error = ((float)AudioSettings.dspTime - keyDownTime) % SongConductor.Instance.secPerBeat;

                    averageError = Mathf.Lerp(averageError, error, 0.25f);

                    Debug.Log(averageError);

                    keyDownThisFrame = false;
                }

                if (taps > totalTapsNeeded) {
                    SongConductor.firstBeatOffset = averageError;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}