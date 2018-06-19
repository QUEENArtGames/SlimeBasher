using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Game : MonoBehaviour {

        private static Game instance;
        private GamePhase _gamePhase;
        public Transform[] spawnPoints;
        public float _nextPhaseTimer = 5.0f;
        internal Phase _currentPhase;
        private bool _startTimer = false;
        private bool _readyButtonEnabled = false;
        private float _countdown;
        public GameObject _pausemenu;
        int _waveRoundNumber = 1;
        private Wave _actualWave;
        public EnemyManagement enemyManagement;
        public bool fightPhaseEnd = false;
        public WaveProvider waveProvider;
        public Transform FinalDestination;
        public GameObject phaseGUI;
        public GameObject _phaseGUICountdown;
        public GameObject _waveRoundText;

        internal static Game Instance {
            get {
                return instance;
            }
        }

        void Start() {
            if(instance==null) {
                instance = this;
            } else {
                Destroy(this.gameObject);
                return;
            }
            enemyManagement.enabled = false;
            _gamePhase = new GamePhase();
            _currentPhase = _gamePhase.Current;
            RunPhase(_currentPhase);
            Time.timeScale = 1;
            //FinalDestination = this.gameObject.transform;
        }

        private void RunPhase(Phase gamePhase) {
            switch (gamePhase) {
                case Phase.Building:
                    //Player kann sachen bauen
                    Debug.Log("Runde: " + _waveRoundNumber);
                    _waveRoundText.GetComponent<Text>().text = "Round: " + _waveRoundNumber;
                    Debug.Log("BUILDING");
                    _readyButtonEnabled = true;
                    phaseGUI.SetActive(false);
                    break;
                case Phase.Prepare:
                    //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                    Debug.Log("Prepare");
                    StartNextRoundCounter();
                    waveProvider.setWaveNumber(_waveRoundNumber);
                    _actualWave = waveProvider.GetNextWave();
                    break;
                case Phase.Fight:
                    //Player bekämpft Enemys
                    Debug.Log("FIGHT");
                    StartWave();
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

        private void StartWave() {
            Debug.Log("Spawning Enemys");
            enemyManagement.enabled = true;
            enemyManagement.EnableManager(_actualWave);
            
        }

        private void ShowWaveEndGUI() {
            phaseGUI.SetActive(true);
            enemyManagement.enabled = false;
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
                _phaseGUICountdown.GetComponent<Text>().text = _nextPhaseTimer - (int)_countdown + "";
            }

            if (_countdown > _nextPhaseTimer) {
                _gamePhase.MoveToNextGamePhase();
                _countdown = 0.0f;
                _startTimer = false;
            }

            if (_readyButtonEnabled && Input.GetButtonDown("ready") && !Mathf.Approximately(Time.timeScale, 0)) {
                Debug.Log("Starting Countdown");
                _gamePhase.MoveToNextGamePhase();
                _readyButtonEnabled = false;

            }

            if (Input.GetButtonDown("Cancel") && !Mathf.Approximately(Time.timeScale,0))
                PauseGame();

            if(fightPhaseEnd) {
                fightPhaseEnd = false;
                _gamePhase.MoveToNextGamePhase();
            }

        }

        public GamePhase GamePhase {
            get {
                return _gamePhase;
            }
        }

    }
}