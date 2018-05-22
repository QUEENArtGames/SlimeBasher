using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class Game : MonoBehaviour {
        private GamePhase _gamePhase;
        public float _nextPhaseTimer = 5.0f;
        internal Phase _currentPhase;
        private bool _startTimer = false;
        private bool _readyButtonEnabled = false;
        private float _countdown;
        public Transform FinalDestination;

        // Use this for initialization
        void Start() {
            _gamePhase = new GamePhase();
            _currentPhase = _gamePhase.Current;
            RunPhase(_currentPhase);
        }

        private void RunPhase(Phase gamePhase) {
            switch (gamePhase) {
                case Phase.Building:
                    //Player kann sachen bauen
                    Debug.Log("BUILDING");
                    _readyButtonEnabled = true;
                    break;
                case Phase.Prepare:
                    //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                    Debug.Log("Prepare");
                    StartNextRoundCounter();
                    break;
                case Phase.Fight:
                    //Player bekämpft Enemys
                    Debug.Log("FIGHT");
                    SpawnEnemys();
                    break;
                case Phase.End:
                    //Rundenende, bereitmachen für Bauphase
                    Debug.Log("End");
                    ShowWaveEndGUI();
                    StartNextRoundCounter();
                    break;
            }
        }

        private void ShowWaveEndGUI() {
            Debug.Log("Round Complete");
        }

        private void SpawnEnemys() {
            Debug.Log("Spawning Enemys");
        }

        void StartNextRoundCounter() {
            _startTimer = true;
        }

        private void CheckGamePhase() {

            if (_currentPhase != _gamePhase.Current) {
                _currentPhase = _gamePhase.Current;
                RunPhase(_currentPhase);
            }

        }

        // Update is called once per frame
        void Update() {

            CheckGamePhase();


            if (_startTimer) {
                _countdown += Time.deltaTime;
                Debug.Log(_nextPhaseTimer - (int)_countdown);
            }

            if (_countdown > _nextPhaseTimer) {
                _gamePhase.MoveToNextGamePhase();
                _countdown = 0.0f;
                _startTimer = false;
            }

            if (_readyButtonEnabled && Input.GetKeyDown(KeyCode.G)) {
                Debug.Log("Starting Countdown");
                _gamePhase.MoveToNextGamePhase();
                _readyButtonEnabled = false;

            }

            if (_currentPhase == Phase.Fight && Input.GetKeyDown(KeyCode.T)) {
                _gamePhase.MoveToNextGamePhase();
            }

        }


    }
}