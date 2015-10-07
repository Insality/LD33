using UnityEngine;

public class PlayerMovement: MonoBehaviour {
    public AudioClip Jump;
    public float PlayerBorder;
    public float PlayerSpeed;

    private int _jumpCount;
    private PlayerStat _pStat;


    private void Awake() {
        if (_pStat == null){
            _pStat = Camera.main.GetComponent<PlayerStat>();
            _pStat.LoadPersonalStats();
        }
        _jumpCount = _pStat.DoubleJump + 1;

        var v = new Vector2 {x = (_pStat.MagnetPower + 1)*0.34f, y = (_pStat.MagnetPower + 1)*0.34f};
        if (_pStat.MagnetPower == 2) v.x += 0.3f;
        GetComponent<BoxCollider2D>().size = v;
    }

    private void Update() {
        var moveVector = new Vector3(0, 0, 0);
        var goalZ = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            moveVector = new Vector3(-PlayerSpeed*Time.deltaTime, 0, 0);
            goalZ = 15;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            moveVector = new Vector3(PlayerSpeed*Time.deltaTime, 0, 0);
            goalZ = -15;
        }


        var curV = transform.localEulerAngles;
        curV.z = goalZ;
        transform.localEulerAngles = curV;


        if (_jumpCount > 0 && Input.GetKeyDown(KeyCode.Space)){
            _jumpCount -= 1;
            var p = transform.position;
            p.x = -0.1f;
            transform.position = p;
            AudioSource.PlayClipAtPoint(Jump, Camera.main.transform.position);
        }

        transform.position += moveVector;

        // Check borders:
        var pos = transform.position;
        if (pos.x > PlayerBorder){
            pos.x = PlayerBorder;
        }

        if (pos.x < -PlayerBorder){
            pos.x = -PlayerBorder;
        }
        transform.position = pos;
    }

    public void UpdateCanJump() {
        _jumpCount = _pStat.DoubleJump + 1;
    }
}