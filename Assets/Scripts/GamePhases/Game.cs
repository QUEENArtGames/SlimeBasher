using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class Game : MonoBehaviour {
        public Vector3[] _possilbeSpawnpoints = new Vector3[6];
        public Wave2[] Waves;
        private GamePhase _gamePhase;
        public float _nextPhaseTimer = 5.0f;
        internal Phase _currentPhase;
        private bool _startTimer = false;
        private bool _readyButtonEnabled = false;
        private float _countdown;
        public GameObject _pausemenu;
        int _waveRoundNumber = 1;
        private Wave _actualWave;

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
                    Wave2 wave = GetNextWave();
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

        private Wave2 GetNextWave() {
            if(_waveRoundNumber >= Waves.Length) {
                return CreateProceduralWave();
            }
            return Waves[_waveRoundNumber];
        }

        private Wave2 CreateProceduralWave() {
            Wave2 newWave = new Wave2();
            newWave.Events = new WaveEvent[2];
            newWave.Events[0] = CreateRandomWaveEvent();
            newWave.Events[1] = CreateRandomWaveEvent();
            return newWave;
        }

        private WaveEvent CreateRandomWaveEvent() {
            WaveEvent newWaveEvent = new WaveEvent();
            newWaveEvent.normalSlimes = _waveRoundNumber / 5;
            newWaveEvent.gasSlimes = _waveRoundNumber / 5;
            return newWaveEvent;
        }

        private Vector3[] CalculateSpawnPoints() {

            Vector3[] spawnpoints = new Vector3[3];
            int[] points = new int[3];
            bool isNotIn = true;

            for (int i = 0; i < points.Length; i++) {
                int number;
                while (isNotIn) {
                    number = UnityEngine.Random.Range(0, 5);
                    if (!points.Contains(number)) {
                        isNotIn = false;
                        points[i] = number;
                    }

                }
                isNotIn = true;
            }

            for(int i= 0; i< points.Length; i++) {
                spawnpoints[i] = _possilbeSpawnpoints[points[i]];
            }


            return spawnpoints;
        }

        private void CalculateWave() {

            _actualWave = new Wave(_waveRoundNumber, CalculateSpawnPoints());

        }

        private void SpawnEnemys(Wave wave) {
            Debug.Log("Spawning Enemys");
            Debug.Log("Spawnen an Positionen: ");
            for(int i = 0; i<3; i++) {
                Debug.Log(_actualWave.SpawnPoints[i].x + " " +  _actualWave.SpawnPoints[i].y + " " + _actualWave.SpawnPoints[i].z);
            }
            Debug.Log("Spawne " + _actualWave.getNormalSlimes + " normale Schleime");
            Debug.Log("Spawne " + _actualWave.getHardSlimes + " feste Schleime");
            Debug.Log("Spawne " + _actualWave.getGasSlimes + " Gas-Schleime");
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