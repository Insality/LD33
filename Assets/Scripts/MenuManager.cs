using System.Collections;
using System.Threading;
using Assets.Scripts.Network;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager: MonoBehaviour {
    public Button DoubleJumpB;
    private Text DoubleJumpBText;
    public Text HighscoreText;

    public Button MagnetB;
    private Text MagnetBText;
    public Button ScoreB;
    private Text ScoreBText;
    public Text StatsText;
    public InputField UsernameField;

    private Highscores _hscores;
    private string _hscoretext;
    private PlayerStat _playerStats;

    private void Awake() {
        _hscores = GetComponent<Highscores>();
        _playerStats = GetComponent<PlayerStat>();
        _playerStats.LoadPersonalStats();
        UsernameField.text = _playerStats.Username;

        MagnetBText = MagnetB.GetComponentInChildren<Text>();
        DoubleJumpBText = DoubleJumpB.GetComponentInChildren<Text>();
        ScoreBText = ScoreB.GetComponentInChildren<Text>();

        UpdateHighscores();
    }

    private void UpdateHighscores() {
        var networkThread = new Thread(UpdateHighscoresAsync);
        networkThread.Start();
    }

    private void UpdateHighscoresAsync() {
        var result = _hscores.GetTop();
        if (!result.ToString().StartsWith("[Error]")){
            _hscoretext = "Highscores\n";
            for (var i = 0; i < result.Count; i++){
                _hscoretext += result[i]["Username"] + " - " + result[i]["Score"] + "\n";
            }
        }
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            Application.LoadLevel("Game");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        UpdateLabels();
    }


    private void UpdateLabels() {
        if (!Input.GetKey(KeyCode.H)){
            StatsText.text = string.Format("Highscore: {0}\nOrbs: {1}\nGame Started:{2} times\nTime played:{3} minutes",
                _playerStats.Highscore,
                _playerStats.OrbsCollected, _playerStats.TimesStarted, (int) (_playerStats.TimePlayed/60));
        }
        else{
            StatsText.text =
                "Space to jump to middle\nPickup orbs to recharge jump\nPickuping orbs in row increase score multiplier, otherwise decreasing\nR to fast restart\nESC to leave from game";
        }

        if (_playerStats.MagnetPower == 0){
            MagnetBText.text = "MAGNET 1 (300)";
        }
        if (_playerStats.MagnetPower == 1){
            MagnetBText.text = "MAGNET 2 (600)";
        }
        if (_playerStats.MagnetPower == 2){
            MagnetBText.text = "MAGNET MAX";
            MagnetB.interactable = false;
        }

        if (_playerStats.DoubleJump == 0){
            DoubleJumpBText.text = "DOUBLE JUMP (500)";
        }
        if (_playerStats.DoubleJump == 1){
            DoubleJumpBText.text = "DOUBLE JUMP MAX";
            DoubleJumpB.interactable = false;
        }

        if (_playerStats.ScorePerOrb == 100){
            ScoreBText.text = "SCORE PER ORB (200)";
        }
        if (_playerStats.ScorePerOrb == 140){
            ScoreBText.text = "SCORE PER ORB (450)";
        }
        if (_playerStats.ScorePerOrb == 180){
            ScoreBText.text = "SCORE PER ORB (700)";
        }
        if (_playerStats.ScorePerOrb == 220){
            ScoreBText.text = "SCORE PER ORB (1000)";
        }
        if (_playerStats.ScorePerOrb == 250){
            ScoreBText.text = "SCORE PER ORB MAX";
            ScoreB.interactable = false;
        }

        HighscoreText.text = _hscoretext;
    }

    public void SetUsername(string username) {
        _playerStats.Username = username;
        _playerStats.SavePersonalStats();
    }
}