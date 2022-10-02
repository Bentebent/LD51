using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class ExplodeArrow : MonoBehaviour {
        public float lifeTime = 1f;
        public new Renderer renderer;
        public float size = 1.5f;

        private float birthTime = 0f;

        static MaterialPropertyBlock mpb = null;

        private void Start() {
            if (mpb == null) {
                mpb = new MaterialPropertyBlock();
            }
            birthTime = Time.time;
        }

        int colorId = Shader.PropertyToID("_Color");

        void Update() {
            float t = Mathf.Clamp01((Time.time - birthTime) / lifeTime);
            mpb.SetColor(colorId, new Color(1f, 1f, 1f, 0.1f * (1f - t)));
            renderer.SetPropertyBlock(mpb);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * size, t) * 3f;

            if (t >= 1f) {
                Destroy(gameObject);
            }
        }
    }
}