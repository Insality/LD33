using System.Threading;
using Assets.Scripts.Beats;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager: MonoBehaviour {
    private const int _orbsPerLevel = 50;
    private const float _spawnEveryUnits = 40;
    public bool BattleMode;
    public AudioClip BeatGame;

    public float ColorChangeTime;
    public int CurrentLevel;
    public Image FillBar;
    public Text FillBarText;
    public AudioClip GameOver;
    public bool IsLose;
    public AudioClip LevelNext;
    public float LevelSpeed;
    public GameObject Light;
    public Light2D LightSource;
    public AudioClip NewStart;
    public AudioClip OrbPickup;
    public AudioClip OrbToLight;
    public GameObject Player;

    public GameObject ProgressBar;
    public Material WallMaterial;

    public GameObject[] WallPrefabsFirst;
    public GameObject[] WallPrefabsSecond;
    public GameObject[] WallPrefabsThird;
    public Text WinText;

    private Color _colorToLerp;
    private float _curColorChangeTime;
    private float _darkSpawn;


    private float _lastSpawnUnits;
    private LightLogic _lightLogic;
    private Thread _networkThread;
    private PlayerStat _playerStat;
    private BeatLightScaler _shadowSource;
    private int _spawnsCounter;
    private bool _wasErrorNetwork;
    private bool isStatsSended;


    private void Start() {
        _colorToLerp = LightSource.LightColor;
        _lastSpawnUnits = 0;
        _spawnsCounter = 3;
        isStatsSended = false;
        _wasErrorNetwork = false;
        _playerStat = GetComponent<PlayerStat>();
        _lightLogic = Light.GetComponent<LightLogic>();
        _darkSpawn = 0;

        ProgressBar.SetActive(true);
        WinText.gameObject.SetActive(false);
        _shadowSource = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<BeatLightScaler>();
        AudioSource.PlayClipAtPoint(NewStart, transform.position);
    }

    private void Update() {
        UpdateColor();
        UpdateLevelSpeed();
        UpdateLevelGenerate();
        UpdateFillBar();
        UpdateDarkSpawnOrLose();
        UpdateControl();
    }


    private void UpdateControl() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.LoadLevel("Menu");
        }

        if (Input.GetKeyDown(KeyCode.R)){
            RestartGame();
            _playerStat.TimesStarted--;
        }
    }

    private void UpdateLevelSpeed() {
        var deltaMove = new Vector3(0, LevelSpeed*Time.deltaTime, 0);

        Player.transform.position += deltaMove;
        Light.transform.position += deltaMove;
    }


    public void SetRandomColor() {
        _colorToLerp = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        _curColorChangeTime = 0;
    }

    private void UpdateColor() {
        _curColorChangeTime += Time.deltaTime;
        if (_curColorChangeTime >= ColorChangeTime){
            _curColorChangeTime = ColorChangeTime;
        }
        LightSource.LightColor = Color.Lerp(LightSource.LightColor, _colorToLerp, _curColorChangeTime/ColorChangeTime);
        WallMaterial.color = Color.Lerp(WallMaterial.color, _colorToLerp, _curColorChangeTime/ColorChangeTime);
    }


    private void UpdateLevelGenerate() {
        GameObject[] WallPrefabs = null;

        if (CurrentLevel == 1) WallPrefabs = WallPrefabsFirst;
        else if (CurrentLevel == 2) WallPrefabs = WallPrefabsSecond;
        else if (CurrentLevel == 3) WallPrefabs = WallPrefabsThird;
        else if (CurrentLevel == 4){
        }
        else{
            WallPrefabs = WallPrefabsFirst;
            Debug.Log("WRONG LEVEL");
        }


        if (CurrentLevel <= 3){
            if (transform.position.y - _lastSpawnUnits > _spawnEveryUnits){
                Instantiate(WallPrefabs[Random.Range(0, WallPrefabs.Length)],
                    new Vector3(0, _spawnsCounter*_spawnEveryUnits, 0), new Quaternion());

                _spawnsCounter++;
                _lastSpawnUnits = transform.position.y;
            }
        }
    }

    private void UpdateFillBar() {
        var curOrbs = _playerStat.CurOrbsCollected;
        var progress = ((float) (curOrbs - ((CurrentLevel - 1)*_orbsPerLevel))/_orbsPerLevel);
        FillBarText.text = "Level " + CurrentLevel;
        FillBar.fillAmount = progress;

        if (progress >= 1){
            LevelUp();
        }
    }

    private void LevelUp() {
        CurrentLevel++;
        _spawnsCounter++;
        _lastSpawnUnits = transform.position.y;
        SetRandomColor();
        if (CurrentLevel <= 3){
            AudioSource.PlayClipAtPoint(LevelNext, transform.position);
        }

        if (CurrentLevel == 2){
            LevelSpeed = 15;
        }
        if (CurrentLevel == 3){
            LevelSpeed = 17;
        }
        if (CurrentLevel == 4){
            LevelSpeed = 0;
            DarkAndSpawn();
        }
    }

    public void OrbsPickup() {
        //        SetRandomColor();
        _lightLogic.UpdateLightSource(_playerStat.CurOrbsCollected);
        AudioSource.PlayClipAtPoint(OrbToLight, transform.position);
    }

    public void DarkAndSpawn() {
        _darkSpawn = 2;
        _shadowSource.UpdateScaleRange(40);
        BattleMode = true;

        ProgressBar.SetActive(false);
        WinText.gameObject.SetActive(true);


        Camera.main.GetComponent<AudioSource>().Stop();
        AudioSource.PlayClipAtPoint(BeatGame, transform.position);
        ClearLevel();
    }

    private void UpdateDarkSpawnOrLose() {
        if ((CurrentLevel == 4 || IsLose) && _darkSpawn > 0){
            _darkSpawn -= Time.deltaTime;
            if (_darkSpawn > 0 && _darkSpawn < 1){
                _shadowSource.UpdateScaleRange(40*_darkSpawn + 4);
            }
        }

        if (_darkSpawn <= 0){
            _shadowSource.UpdateScaleRange(4);
            _darkSpawn = 0;

            if (IsLose){
                IsLose = false;
                RestartGame();
            }
        }
    }

    public void LoseGame() {
        SendStats();

        IsLose = true;
        _darkSpawn = 2;
        LevelSpeed = 0;
        _shadowSource.UpdateScaleRange(50);
        Camera.main.GetComponent<AudioSource>().Stop();
        AudioSource.PlayClipAtPoint(GameOver, transform.position);
        ClearLevel();
    }

    private void ClearLevel() {
        foreach (var e in GameObject.FindGameObjectsWithTag("Wall")){
            Destroy(e);
        }
        foreach (var e in GameObject.FindGameObjectsWithTag("Orb")){
            Destroy(e);
        }
    }

    private void RestartGame() {
        _playerStat.ResetCurrentGame();
        Player.transform.position = new Vector3(0, -3, 0);
        Light.transform.position = new Vector3(0, 10, 0);
        Light.GetComponent<LightLogic>().Restart();
        _lightLogic.UpdateLightSource(0);
        CurrentLevel = 1;
        LevelSpeed = 14;
        Camera.main.GetComponent<AudioSource>().Play();
        Camera.main.transform.position = new Vector3(0, 15, -12);
        isStatsSended = false;

        ClearLevel();
        _lastSpawnUnits = 0;
        _spawnsCounter = 3;
        AudioSource.PlayClipAtPoint(NewStart, transform.position);

        _colorToLerp = new Color(0.5f, 0.5f, 0.5f);
        _curColorChangeTime = 0;

        var taunts = GetComponent<TauntManager>();
        taunts.Restart();
    }

    public void SendStats() {
        _networkThread = new Thread(SendAsyncStats);
        _networkThread.Start();
    }

    private void SendAsyncStats() {
        if (!_wasErrorNetwork && !isStatsSended){
            isStatsSended = true;
            var result = _playerStat.SendStats(BattleMode);
            if (!result){
                Debug.Log("WAS ERROR");
                _wasErrorNetwork = true;
            }
        }
    }
}