using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD51 {
    public class UIManager : MonoBehaviour {
        private static UIManager _instance;
        public static UIManager Instance => _instance;

        public GameObject singleTargetTower = null;
        public GameObject splashTower = null;
        public GameObject dotTower = null;
        public GameObject slowTower = null;

        public GameObject selectedTile = null;

        public GameObject endText = null;
        public TMPro.TMP_Text lifeText;


        void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
                Debug.LogWarning("A duplicate SongConductor was found");
            }

            endText.SetActive(false);
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (endText != null && endText.activeInHierarchy) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    Application.Quit();
                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            lifeText.text = Player.Instance.lives.ToString();
        }

        public void BuildTower(string tower) {
            if (tower == "single") {

            }
            else if (tower == "splash") {

            }
            else if (tower == "dot") {

            }
            else if (tower == "slow") {

            }
        }
    }
}

