using UnityEngine;

namespace Assets.Scripts.Beats {
    public class BeatLightScaler: MonoBehaviour {
        private const float ScaleTrashold = 0.1f;
        public bool IsBeatScaling;
        private float ScaleFrom;
        private float ScaleTo;
        public int ScaleUpTo;

        private BeatTracker _cameraBeatTracker;
        private Light2D _light2D;

        private void Start() {
            _cameraBeatTracker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BeatTracker>();
            _light2D = GetComponent<Light2D>();
            ScaleFrom = _light2D.LightRadius;
            ScaleTo = ScaleFrom + ScaleUpTo;
        }


        public void UpdateScaleRange(float newRadius) {
            ScaleFrom = newRadius;
            ScaleTo = ScaleFrom + ScaleUpTo;
        }

        private void Update() {
            if (IsBeatScaling){
                var scaleCoef = _cameraBeatTracker.GetBeatPower();
                if (scaleCoef <= ScaleTrashold){
                    scaleCoef = 0;
                }

                var scaleHowMuch = Mathf.Lerp(ScaleFrom, ScaleTo, scaleCoef);
                _light2D.LightRadius = scaleHowMuch;
            }
        }
    }
}