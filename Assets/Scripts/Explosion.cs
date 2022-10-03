using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD51 {
    public class Explosion : MonoBehaviour {
        // Start is called before the first frame update
        Player player = null;
        public float damage = 15f;

        void Start() {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update() {

            var colliders = Physics.OverlapSphere(transform.position, 5.0f, Layers.GetMask(Layers.Enemy));
            foreach (var collider in colliders) {
                var unit = collider.GetComponent<PathingUnit>();
                unit.health -= damage;
                if (unit.health <= 0) {
                    player.AddMoney(unit.value);
                    Destroy(unit.gameObject);
                }
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            //if (other.gameObject.layer == Layers.Enemy) {
            //    var unit = other.GetComponent<PathingUnit>();
            //    unit.health -= 25;
            //    if (unit.health <= 0) {
            //        player.AddMoney(unit.value);
            //        Destroy(unit.gameObject);
            //    }
            //}
        }

    }

}
