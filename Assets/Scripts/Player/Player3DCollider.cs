using UnityEngine;

public class Player3DCollider: MonoBehaviour {
    private LevelManager _levelManager;

    private void Start() {
        _levelManager = Camera.main.GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Wall"){
            _levelManager.LoseGame();
        }
    }
}