using Assets.Scripts.Beats;
using UnityEngine;

public class LightLogic: MonoBehaviour {
    private float _battleModeTimer;

    private float _goalX;
    private LevelManager _levelManager;
    private BeatLightScaler _lightScaler;
    private Light2D _lightSource;
    private float _startRadius;
    // Use this for initialization
    private void Start() {
        _lightSource = GetComponent<Light2D>();
        _startRadius = _lightSource.LightRadius;
        _lightScaler = GetComponent<BeatLightScaler>();
        _goalX = 0;


        _levelManager = Camera.main.GetComponent<LevelManager>();


        Camera.main.GetComponent<BeatTracker>().BeatEvent += (sender, args)=>LightBeat();
    }

    // Update is called once per frame
    private void Update() {
        if (_levelManager.BattleMode){
            _battleModeTimer += Time.deltaTime;
            _lightSource.LightType = Light2D.LightTypeSetting.Directional;
            _lightSource.LightBeamRange = 1.5f;
            _lightSource.LightBeamSize = 80f - _battleModeTimer*20;

            if (_lightSource.LightBeamSize <= 0.01){
                gameObject.SetActive(false);
            }

            var curRot = transform.localEulerAngles;
            curRot.z += 12;
            transform.localEulerAngles = curRot;
        }
        else{
            var goalV = new Vector3(_goalX, transform.position.y, transform.position.z);

            var velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, goalV, ref velocity, 0.1f);
        }
    }

    private void LightBeat() {
        _goalX = Random.Range(-2f, 2f);
    }

    public void Restart() {
        _goalX = 0;
    }

    public void UpdateLightSource(int orbs) {
        var newRadius = _startRadius + orbs/25f;
        _lightScaler.UpdateScaleRange(newRadius);
    }
}