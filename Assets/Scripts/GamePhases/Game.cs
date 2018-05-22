using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class Game : MonoBehaviour {
        
        public Wave[] Waves;
        private GamePhase _gamePhase;
        public float _nextPhaseTimer = 5.0f;
        internal Phase _currentPhase;
        private bool _startTimer = false;
        private bool _readyButtonEnabled = false;
        private float _countdown;
        public GameObject _pausemenu;
        int _waveRoundNumber = 1;
        private Wave _actualWave;
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
                    Debug.Log("Runde: " + _waveRoundNumber);
                    Debug.Log("BUILDING");
                    _readyButtonEnabled = true;
                    break;
                case Phase.Prepare:
                    //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                    Debug.Log("Prepare");
                    StartNextRoundCounter();
                    Wave wave = GetNextWave();
                    StartWave(wave);
                    CalculateWave();
                    break;
                case Phase.Fight:
                    //Player bekämpft Enemys
                    Debug.Log("FIGHT");
                    SpawnEnemys(_actualWave);
                    break;
                case Phase.End:
                    //Rundenende, bereitmachen für Bauphase
                    Debug.Log("End");
                    ShowWaveEndGUI();
                    StartNextRoundCounter();
                    _waveRoundNumber++;
                    break;
            }
        }

        private void StartWave(Wave wave) {
            SpawnEnemys(wave);
        }

        private Wave GetNextWave() {
            if(_waveRoundNumber >= Waves.Length) {
                return CreateProceduralWave();
            }
            return Waves[_waveRoundNumber];
        }

        private Wave CreateProceduralWave() {
            Wave newWave = new Wave(_waveRoundNumber);
            newWave.Events = new WaveEvent[2];
            newWave.Events[0] = CreateRandomWaveEvent();
            newWave.Events[1] = CreateRandomWaveEvent();
            return newWave;
        }

        private WaveEvent CreateRandomWaveEvent() {
            WaveEvent newWaveEvent = new WaveEvent(_waveRoundNumber);
            return newWaveEvent;
        }

        private void CalculateWave() {

            _actualWave = new Wave(_waveRoundNumber);

        }

        private void SpawnEnemys(Wave wave) {
            Debug.Log("Spawning Enemys");
        }

        private void ShowWaveEndGUI() {
            Debug.Log("Round Complete");
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


        private void PauseGame() {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pausemenu.SetActive(true);

        }

        public void ResumeGame() {
            Time.timeScale = 1;
            Cursor.visible = false;
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

            if (_readyButtonEnabled && Input.GetKeyDown(KeyCode.G) && Time.timeScale != 0) {
                Debug.Log("Starting Countdown");
                _gamePhase.MoveToNextGamePhase();
                _readyButtonEnabled = false;

            }

            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
                PauseGame();

            if (_currentPhase == Phase.Fight && Input.GetKeyDown(KeyCode.T) && Time.timeScale != 0) {
                _gamePhase.MoveToNextGamePhase();
            }

            

        }

    }
}