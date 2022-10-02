using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class ProgressBar : MonoBehaviour {
        public Transform bar;
        private float m_progress = 1f;

        public void SetProgress(float progress) {
            progress = Mathf.Clamp01(progress);
            if (progress != m_progress) {
                m_progress = progress;
                bar.transform.localScale = new Vector3(progress, 1f, 1f);
                gameObject.SetActive(progress != 0f);
            }
        }
    }
}