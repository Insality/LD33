using System;
using UnityEngine;

namespace Assets.Scripts.Beats {
    public class BeatTracker: MonoBehaviour {
        private const float MinBeatCooldown = 0.35f;

        /// Limit sets in beat power by experemental way
        public float BeatLowerLimit;

        public float BeatUpperLimit;

        private float _beatCooldown;
        private bool _isBeatThisTurn;
        private AudioSource _track;

        // This events calls every beat, tracked by script
        public event EventHandler BeatEvent;

        private void Start() {
            _track = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

            // Null action to avoid null exception
            BeatEvent += delegate { };
        }

        private void Update() {
            var bassBeat = GetBassBeat();

            // Beat Tracking
            _beatCooldown -= Time.deltaTime;
            if (_beatCooldown < 0){
                _beatCooldown = 0;
            }

            if (_isBeatThisTurn && bassBeat < BeatLowerLimit){
                _isBeatThisTurn = false;
            }

            if (!_isBeatThisTurn && bassBeat > BeatLowerLimit && _beatCooldown == 0){
                _isBeatThisTurn = true;
                _beatCooldown = MinBeatCooldown;
                BeatAction();
            }
        }

        public float GetBassBeat() {
            var spectrum = _track.GetSpectrumData(1024, 0, FFTWindow.Hamming);
            var c1 = spectrum[2] + spectrum[3];

            return c1;
        }

        public float GetBeatPower() {
            var c1 = GetBassBeat();
            var beatLowerLimit = BeatLowerLimit;
            var beatUpperLimit = BeatUpperLimit;

            return c1/(beatLowerLimit + beatUpperLimit);
        }

        private void BeatAction() {
            //            Debug.Log("BEAT");
            BeatEvent(null, null);
        }
    }
}