using UnityEngine;

namespace Assets.Scripts
{
    class Game : MonoBehaviour {
        
        
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
        public EnemyManagement enemyManagement;
        public bool fightPhaseEnd = false;

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
                    WaveProvider waveProvider = new WaveProvider(_waveRoundNumber);
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
            enemyManagement.EnableManager(this, _actualWave);
            
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

            if(fightPhaseEnd) {
                fightPhaseEnd = false;
                _gamePhase.MoveToNextGamePhase();
            }



            

        }

    }
}