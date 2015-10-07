using UnityEngine;

public class OrbDestroyer: MonoBehaviour {
    private PlayerStat _pStat;

    private void Start() {
        _pStat = Camera.main.GetComponent<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Orb"){
            _pStat.DecreaseMultiplier();
            Destroy(other.gameObject);
        }
    }
}