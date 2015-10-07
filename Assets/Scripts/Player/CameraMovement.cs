using Assets.Scripts.Beats;
using UnityEngine;

namespace Assets.Scripts.Player {
    public class CameraMovement: MonoBehaviour {
        private const float ScaleTrashold = 0.1f;

        public int CameraFOV = 70;
        public bool IsScaleCamera;
        public GameObject Player;
        private float _battleModeTimer;

        private Camera _camera;
        private BeatTracker _cameraBeatTracker;
        private float _deltaHeight;

        private LevelManager _levelManager;


        private void Start() {
            _deltaHeight = 0;
            _camera = GetComponent<Camera>();
            _cameraBeatTracker = GetComponent<BeatTracker>();
            _levelManager = GetComponent<LevelManager>();
            _battleModeTimer = 0;
        }

        private void Update() {
            if (_levelManager.BattleMode || _levelManager.IsLose){
                _battleModeTimer += Time.deltaTime;
                var velocity = Vector2.zero;
                var goalV3 = Player.transform.position;
                goalV3.x /= 2;
                if (_battleModeTimer < 6){
                    goalV3.y += 6;
                }

                if (_battleModeTimer < 7){
                    _levelManager.SendStats();
                }
                if (_battleModeTimer > 14){
                    Application.LoadLevel("Menu");
                }
                goalV3.z = -14;
                transform.position = Vector2.SmoothDamp(transform.position, goalV3, ref velocity, 0.04f);
                transform.position += new Vector3(0, 0, -14);
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else{
                var velocity = Vector2.zero;
                var goalV3 = Player.transform.position;
                goalV3.x /= 2;
                //            goalV3.y += 0;
                goalV3.z = -14;

                if (goalV3.y >= 15){
                    transform.position = Vector2.SmoothDamp(transform.position, goalV3, ref velocity, 0.03f);
                    transform.position += new Vector3(0, 0, -14);
                }
                else{
                    var newVector = transform.position;
                    newVector.y += 1;
                    transform.position = Vector2.SmoothDamp(transform.position, newVector, ref velocity, 0.1f);
                    transform.position += new Vector3(0, 0, -14);
                }
                transform.localEulerAngles = new Vector3(335, 0);
            }

            if (IsScaleCamera){
                var scaleCoef = _cameraBeatTracker.GetBeatPower();
                if (scaleCoef <= ScaleTrashold){
                    scaleCoef = 0;
                }

                _deltaHeight = Mathf.Lerp(0f, 3f, scaleCoef);
                _camera.fieldOfView = CameraFOV + _deltaHeight;
            }
        }
    }
}