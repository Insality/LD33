using UnityEngine;

public class OrbLogic: MonoBehaviour {
    private bool IsCollected;
    private Vector3 _curVelocity;
    private LevelManager _levelManager;

    private GameObject _light;
    private PlayerStat _playerStat;
    private float timeToLife;

    private void Start() {
        IsCollected = false;
        timeToLife = 1f;

        _levelManager = Camera.main.GetComponent<LevelManager>();
        _light = GameObject.FindGameObjectWithTag("Light");
        _playerStat = Camera.main.GetComponent<PlayerStat>();
    }

    private void Update() {
        if (IsCollected){
            timeToLife -= Time.deltaTime;
            if (timeToLife < 0f || Vector3.Distance(transform.position, _light.transform.position) < 0.65f){
                _playerStat.AddOrb();
                Destroy(gameObject);
            }

            var goal = _light.transform.position;
            transform.position = Vector2.Lerp(goal, transform.position, timeToLife);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            IsCollected = true;
            GetComponent<CircleCollider2D>().enabled = false;
            AudioSource.PlayClipAtPoint(_levelManager.OrbPickup, Camera.main.transform.position);
        }
    }
}