using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class BuildTile : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layers.GetMask(Layers.BuildTile))) {
                    Debug.Log("COCKS");
                }
            }
        }
    }
}

