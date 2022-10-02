using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class NoteHitStatus : MonoBehaviour {
        private static NoteHitStatus _instance = null;
        public static NoteHitStatus Instance => _instance;

        public GameObject missTemplate;
        public GameObject badTemplate;
        public GameObject okTemplate;
        public GameObject goodTemplate;
        public GameObject perfectTemplate;
        public RectTransform splineStart;
        public RectTransform splineMid;
        public RectTransform splineEnd;

        private void Awake() {
            _instance = this;
        }

        public enum NoteHitType {
            Bad,
            Ok,
            Good,
            Perfect,
            Miss,
        }

        private class HitText {
            public NoteHitType type;
            public TMPro.TMP_Text text;
            public RectTransform rectTransform;
            public float t;
        }

        private List<HitText> texts = new List<HitText>();

        public void AddNoteHit(NoteHitType type) {
            GameObject template = type switch {
                NoteHitType.Miss => missTemplate,
                NoteHitType.Bad => badTemplate,
                NoteHitType.Ok => okTemplate,
                NoteHitType.Good => goodTemplate,
                NoteHitType.Perfect => perfectTemplate,
                _ => badTemplate,
            };

            GameObject text = Instantiate(template, transform);
            text.SetActive(true);
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = splineStart.anchoredPosition;

            HitText hitText = new HitText {
                type = type,
                text = text.GetComponent<TMPro.TMP_Text>(),
                rectTransform = text.GetComponent<RectTransform>(),
                t = 0f,
            };
            texts.Add(hitText);
        }

        Vector2 GetSmoothPos(float t) {
            Vector2 start = splineStart.anchoredPosition;
            Vector2 mid = splineMid.anchoredPosition;
            Vector2 end = splineEnd.anchoredPosition;

            Vector2 p0 = Vector2.Lerp(start, mid, t);
            Vector2 p1 = Vector2.Lerp(mid, end, t);
            return Vector2.Lerp(p0, p1, t);
        }

        private void Update() {
            for (int i = texts.Count - 1; i >= 0; i--) {
                HitText text = texts[i];
                text.rectTransform.anchoredPosition = GetSmoothPos(text.t);
                Color color = text.text.color;
                color.a = 1f - text.t;
                text.text.color = color;

                text.t += 0.5f * Time.deltaTime;
                if (text.t > 1f) {
                    Destroy(text.text.gameObject);
                    texts.RemoveAt(i);
                }
            }
        }


    }
}