using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Explosion : MonoBehaviour {
        // Start is called before the first frame update
        Player player = null;

        bool destroyMe = false;
        void Start() {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update() {
            if (destroyMe) {
                Destroy(gameObject);
            }

            if (!destroyMe) {
                destroyMe = true;
            }
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
