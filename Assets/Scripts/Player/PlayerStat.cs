using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat: MonoBehaviour {
    public int CurOrbsCollected;
    public float CurTimePlayed;
    public int DoubleJump;
    public int Highscore;
    public AudioClip LoseMultiplier;
    public int MagnetPower;
    public AudioClip NewHighscore;
    public int OrbsCollected;
    public int Score;
    public Text ScoreLabel;
    public float ScoreMultiplier;
    public int ScorePerOrb;
    public Text TimeLabel;
    public float TimePlayed;
    public int TimesStarted;
    public AudioClip Upgrade;
    public string Username;
    private Highscores _highscores;


    // upgrades

    private bool _isNewNighscore;

    private LevelManager _levelManager;
    private PlayerMovement _playerMovement;

    // Use this for initialization
    private void Start() {
        LoadPersonalStats();

        _levelManager = Camera.main.GetComponent<LevelManager>();
        if (_levelManager != null){
            _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _highscores = GetComponent<Highscores>();
            ResetCurrentGame();
        }
    }

    private void Awake() {
        LoadPersonalStats();
    }

    public void LoadPersonalStats() {
        OrbsCollected = PlayerPrefs.GetInt("orbs");
        TimePlayed = PlayerPrefs.GetFloat("timeplayer");
        Highscore = PlayerPrefs.GetInt("highscore");
        TimesStarted = PlayerPrefs.GetInt("timesstarted");
        Username = PlayerPrefs.GetString("username");
        ScorePerOrb = PlayerPrefs.GetInt("scoreperorb", 100);
        DoubleJump = PlayerPrefs.GetInt("doublejump", 0);
        MagnetPower = PlayerPrefs.GetInt("magnetpower", 0);
    }

    public void ResetCurrentGame() {
        TimesStarted++;
        _isNewNighscore = false;
        CurOrbsCollected = 0;
        CurTimePlayed = 0;
        Score = 0;
        ScoreMultiplier = 1;
        UpdateLabels();
        _playerMovement.UpdateCanJump();
    }

    public void AddOrb() {
        CurOrbsCollected++;
        OrbsCollected++;
        Score += (int) (ScorePerOrb*ScoreMultiplier);
        IncreaseMultiplier();
        if (Score > Highscore){
            Highscore = Score;
            if (!_isNewNighscore)
                AudioSource.PlayClipAtPoint(NewHighscore, Camera.main.transform.position);
            _isNewNighscore = true;
        }

        _levelManager.OrbsPickup();
        UpdateLabels();
        _playerMovement.UpdateCanJump();
    }

    public void IncreaseMultiplier() {
        ScoreMultiplier += 0.5f;
    }

    public void DecreaseMultiplier() {
        ScoreMultiplier -= 4;

        if (ScoreMultiplier < 1)
            ScoreMultiplier = 1;

        UpdateLabels();
    }

    private bool PayOrbs(int count) {
        if (OrbsCollected >= count){
            OrbsCollected -= count;
            AudioSource.PlayClipAtPoint(Upgrade, Camera.main.transform.position);
            return true;
        }
        return false;
    }

    public void UpdgrageStat(string stat) {
        if (stat == "magnetpower"){
            if (MagnetPower == 1){
                if (PayOrbs(600)) MagnetPower = 2;
            }
            if (MagnetPower == 0){
                if (PayOrbs(300)) MagnetPower = 1;
            }
        }
        else if (stat == "doublejump"){
            if (DoubleJump == 0){
                if (PayOrbs(500)) DoubleJump = 1;
            }
        }
        else if (stat == "scoreperorb"){
            if (ScorePerOrb == 220)
                if (PayOrbs(1000)) ScorePerOrb = 250;
            if (ScorePerOrb == 180)
                if (PayOrbs(700)) ScorePerOrb = 220;
            if (ScorePerOrb == 140)
                if (PayOrbs(450)) ScorePerOrb = 180;
            if (ScorePerOrb == 100)
                if (PayOrbs(200)) ScorePerOrb = 140;
        }
        SavePersonalStats();
    }

    public void SavePersonalStats() {
        PlayerPrefs.SetInt("orbs", OrbsCollected);
        PlayerPrefs.SetFloat("timeplayer", TimePlayed);
        PlayerPrefs.SetInt("highscore", Highscore);
        PlayerPrefs.SetInt("timesstarted", TimesStarted);
        PlayerPrefs.SetString("username", Username);
        PlayerPrefs.SetInt("scoreperorb", ScorePerOrb);
        PlayerPrefs.SetInt("doublejump", DoubleJump);
        PlayerPrefs.SetInt("magnetpower", MagnetPower);
    }

    public void OnDestroy() {
        SavePersonalStats();
    }

    private void UpdateLabels() {
        if (_levelManager != null){
            ScoreLabel.text = Score + "    x" + string.Format("{0:F1}", ScoreMultiplier) + "";
        }
    }

    // Update is called once per frame
    private void Update() {
        if (_levelManager != null){
            if (!_levelManager.BattleMode && !_levelManager.IsLose){
                CurTimePlayed += Time.deltaTime;
                TimePlayed += Time.deltaTime;
            }

            TimeLabel.text = string.Format("{0:F2}", CurTimePlayed);
        }
    }

    public bool SendStats(bool is_win) {
        var isSuccess = true;

        if (_isNewNighscore){
            var t = _highscores.SendRecord(Username, Highscore, 0);
            if (!t) isSuccess = t;
        }

        var tt = _highscores.SendStats(Username, Score, 0, is_win, CurOrbsCollected, (int) (CurTimePlayed));
        if (!tt) isSuccess = tt;

        return isSuccess;
    }
}