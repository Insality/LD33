using UnityEngine;
using UnityEngine.UI;

public class TauntManager: MonoBehaviour {
    public Text TauntText;
    public string[] Taunts;

    private float _curOpacity;
    private bool _isBack;
    private bool _isShow;
    private float showFullTime = 2f;
    private float showTime;
    private float _showEverySeconds = 20;
    private float _curShowEverySeconds;

    // Use this for initialization
    private void Start() {
        _curShowEverySeconds = 2;
        _isShow = false;
        _isBack = false;
        _curOpacity = 0;
        TauntText.text = Taunts[Random.Range(0, Taunts.Length - 1)];
        TauntText.color = new Color(0.9f, 0.9f, 0.9f, _curOpacity);
    }

    // Update is called once per frame
    private void Update() {

        _curShowEverySeconds -= Time.deltaTime;
        if (_curShowEverySeconds < 0){
            _curShowEverySeconds = _showEverySeconds;
            ShowRandomTaunt();
        }

        if (_isShow){
            if (!_isBack){
                showTime += Time.deltaTime;
            }
            else{
                showTime -= Time.deltaTime;
            }


            _curOpacity = Mathf.Lerp(0, 1, showTime);
            TauntText.color = new Color(0.9f, 0.9f, 0.9f, _curOpacity);

            if (showTime > showFullTime && !_isBack){
                _isBack = true;
            }
            if (showTime < 0 && _isBack){
                _isShow = false;
                _isBack = false;
            }
        }
    }

    public void Restart() {
        _curShowEverySeconds = 2;
        _isShow = false;
        _isBack = false;
        _curOpacity = 0;
        TauntText.text = Taunts[Random.Range(0, Taunts.Length - 1)];
        TauntText.color = new Color(0.9f, 0.9f, 0.9f, _curOpacity);
    }

    public void ShowRandomTaunt() {
        showTime = 0;
        _isShow = true;
        _isBack = false;
        TauntText.text = Taunts[Random.Range(0, Taunts.Length - 1)];
    }
}