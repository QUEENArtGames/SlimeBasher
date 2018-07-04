using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Game : MonoBehaviour
    {

        private static Game instance;
        private GamePhase _gamePhase;
        public Transform[] spawnPoints;
        public float _nextPhaseTimer = 5.0f;
        internal Phase _currentPhase;
        private bool _startTimer = false;
        public bool _readyButtonEnabled = false;
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
        public GameObject _readyUI;
        public GameObject _tutorialUI;
        public GameObject _towerUI;

        internal static Game Instance
        {
            get
            {
                return instance;
            }
        }

        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            enemyManagement.enabled = false;
            _gamePhase = new GamePhase();
            _currentPhase = _gamePhase.Current;
            RunPhase(_currentPhase);
            Time.timeScale = 1;
            FindObjectOfType<PlayerScrapInventory>().AddScrap(ScrapType.BOTTLE, 0);
        }

        private void RunPhase(Phase gamePhase)
        {
            switch (gamePhase)
            {
                case Phase.Building:
                    //Player kann sachen bauen
                    Debug.Log("Runde: " + _waveRoundNumber);
                    _waveRoundText.GetComponent<Text>().text = "Round: " + _waveRoundNumber;
                    Debug.Log("BUILDING");
                    _towerUI.GetComponent<TowerUIFading>().FadeIn();
                    if (_waveRoundNumber > 1)
                    {
                        _readyButtonEnabled = true;
                        _readyUI.SetActive(true);
                    }
                    phaseGUI.SetActive(false);
                    break;
                case Phase.Prepare:
                    //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                    Debug.Log("Prepare");
                    StartNextRoundCounter();
                    waveProvider.setWaveNumber(_waveRoundNumber);
                    _actualWave = waveProvider.GetNextWave();
                    _readyUI.SetActive(false);
                    _towerUI.GetComponent<TowerUIFading>().FadeOut();
                    if (_waveRoundNumber <= 2)
                        _tutorialUI.GetComponent<Tutorial>().FadeOut();
                    break;
                case Phase.Fight:
                    FindObjectOfType<GameSounds>().PlayRoundStartClip();
                    //Player bekämpft Enemys
                    Debug.Log("FIGHT");
                    if (_waveRoundNumber == 1)
                    {
                        _tutorialUI.GetComponentInChildren<Text>().text = "Da kommen sie! Ich muss meine Basis verteidigen bevor die Slimes sie verunreinigen!";
                        _tutorialUI.GetComponent<Tutorial>().FadeIn();
                    }
                    StartWave();
                    break;
                case Phase.End:
                    FindObjectOfType<GameSounds>().PlayRoundEndClip();
                    FindObjectOfType<PlayerSounds>().PlayCheerSound();
                    //Rundenende, bereitmachen für Bauphase
                    if (_waveRoundNumber == 1)
                        _tutorialUI.GetComponent<Tutorial>().FadeOut();
                    Debug.Log("End");
                    ShowWaveEndGUI();
                    StartNextRoundCounter();
                    _waveRoundNumber++;
                    break;
            }
        }

        private void StartWave()
        {
            Debug.Log("Spawning Enemys");
            enemyManagement.enabled = true;
            enemyManagement.EnableManager(_actualWave);

        }

        private void ShowWaveEndGUI()
        {
            phaseGUI.SetActive(true);
            enemyManagement.enabled = false;
        }


        void StartNextRoundCounter()
        {
            _startTimer = true;
        }

        private void CheckGamePhase()
        {

            if (_currentPhase != _gamePhase.Current)
            {
                _currentPhase = _gamePhase.Current;
                RunPhase(_currentPhase);
            }

        }


        private void PauseGame()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _pausemenu.SetActive(true);

        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }


        // Update is called once per frame
        void Update()
        {

            CheckGamePhase();


            if (_startTimer)
            {
                _countdown += Time.deltaTime;
                _phaseGUICountdown.GetComponent<Text>().text = _nextPhaseTimer - (int) _countdown + "";
            }

            if (_countdown > _nextPhaseTimer)
            {
                if (_gamePhase.Current == Phase.End && _waveRoundNumber == 2)
                {
                    _tutorialUI.GetComponentInChildren<Text>().text = "Sehr gut, ich hab es geschafft! Aber ich glaube, da kommen nochmehr. Ich sollte mich vorbereiten. Dafür brauche Türme. Die Baumaterialien kann ich durch Schrott ersetzen, der jetzt hier überall rumliegt. Also los gehts!";
                    _tutorialUI.GetComponent<Tutorial>().FadeIn();
                }
                _gamePhase.MoveToNextGamePhase();
                _countdown = 0.0f;
                _startTimer = false;
            }

            if (_readyButtonEnabled && Input.GetButtonDown("ready") && !Mathf.Approximately(Time.timeScale, 0))
            {
                Debug.Log("Starting Countdown");
                _gamePhase.MoveToNextGamePhase();
                _readyButtonEnabled = false;

            }

            if (Input.GetButtonDown("Cancel") && !Mathf.Approximately(Time.timeScale, 0))
                PauseGame();

            if (fightPhaseEnd)
            {
                fightPhaseEnd = false;
                _gamePhase.MoveToNextGamePhase();
            }

        }

        public GamePhase GamePhase
        {
            get
            {
                return _gamePhase;
            }
        }

    }
}