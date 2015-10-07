using UnityEngine;

namespace Assets.Scripts.Beats {
    public class BeatScaler: MonoBehaviour {
        private const float ScaleTrashold = 0.1f;
        public bool IsBeatScaling;
        private float ScaleFrom;
        private float ScaleTo;
        public float ScaleUpTo;

        private BeatTracker _cameraBeatTracker;

        private void Start() {
            _cameraBeatTracker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BeatTracker>();
            ScaleFrom = 1;
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
                transform.localScale = new Vector3(scaleHowMuch, scaleHowMuch, scaleHowMuch);
            }
        }
    }
}