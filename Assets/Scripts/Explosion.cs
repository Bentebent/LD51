using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Explosion : MonoBehaviour {
        // Start is called before the first frame update
        Player player = null;
        void Start() {
            Destroy(gameObject, 1.0f);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == Layers.Enemy) {
                var unit = other.GetComponent<PathingUnit>();
                unit.health -= 25;
                if (unit.health <= 0) {
                    player.AddMoney(unit.value);
                    Destroy(unit.gameObject);
                }
            }
        }

    }

}
