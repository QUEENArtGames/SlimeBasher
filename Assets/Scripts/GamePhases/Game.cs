using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Game : MonoBehaviour
    {

        private static Game instance;
        private GamePhase _gamePhase;

        public GameObject[] spawnPoints;
        public GameObject _pausemenu;
        public Transform _finalDestination;
        public GameObject phaseGUI;
        public GameObject _phaseGUICountdown;
        public GameObject _waveRoundText;
        public GameObject _readyUI;
        public GameObject _tutorialUI;
        public GameObject _towerUI;

        public float _nextPhaseTimer = 5.0f;
        public bool _readyButtonEnabled = false;
        public bool _fightPhaseEnd = false;
        public EnemyManagement _enemyManagement;
        public WaveProvider _waveProvider;

        private float _countdown;        
        private bool _startTimer = false;
        private int _waveRoundNumber = 1;
        private Wave _actualWave;

        internal Phase _currentPhase;

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
            _enemyManagement.enabled = false;
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
                    _waveRoundText.GetComponent<Text>().text = "Wave: " + _waveRoundNumber;
                    _towerUI.GetComponent<TowerUIFading>().FadeIn();
                    if (_waveRoundNumber > 1)
                    {
                        _readyButtonEnabled = true;
                        _readyUI.SetActive(true);
                    }
                    phaseGUI.SetActive(false);
                    break;
                case Phase.Prepare:
                    StartNextRoundCounter();
                    _waveProvider.SetWaveNumber(_waveRoundNumber);
                    _actualWave = _waveProvider.GetNextWave();
                    _readyUI.SetActive(false);
                    _towerUI.GetComponent<TowerUIFading>().FadeOut();
                    if (_waveRoundNumber <= 2)
                        _tutorialUI.GetComponent<Tutorial>().FadeOut();
                    break;
                case Phase.Fight:
                    FindObjectOfType<GameSounds>().PlayRoundStartClip();
                    if (_waveRoundNumber == 1)
                    {
                        _tutorialUI.GetComponentInChildren<Text>().text = "Es geht los! Verteidige deine Basis vor diesen Slimes. Sie sehen nett aus, wollen aber deine Wasserversorgung verschmutzen, verhindere das!";
                        _tutorialUI.GetComponent<Tutorial>().FadeIn();
                        _tutorialUI.GetComponent<Tutorial>().FadeOutV(5f);
                    }
                    StartWave();
                    break;
                case Phase.End:
                    FindObjectOfType<GameSounds>().PlayRoundEndClip();
                    FindObjectOfType<PlayerSounds>().PlayCheerSound();
                    if (_waveRoundNumber == 1)
                        _tutorialUI.GetComponent<Tutorial>().FadeOut();
                    ShowWaveEndGUI();
                    StartNextRoundCounter();
                    _waveRoundNumber++;
                    break;
            }
        }

        private void StartWave()
        {
            _enemyManagement.enabled = true;
            _enemyManagement.EnableManager(_actualWave);
        }

        private void ShowWaveEndGUI()
        {
            phaseGUI.SetActive(true);
            _enemyManagement.enabled = false;
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
                    _tutorialUI.GetComponent<Tutorial>().FadeOutV(5f);
                }
                _gamePhase.MoveToNextGamePhase();
                _countdown = 0.0f;
                _startTimer = false;
            }

            if (_readyButtonEnabled && Input.GetButtonDown("ready") && !Mathf.Approximately(Time.timeScale, 0))
            {
                _gamePhase.MoveToNextGamePhase();
                _readyButtonEnabled = false;

            }

            if (Input.GetButtonDown("Cancel") && !Mathf.Approximately(Time.timeScale, 0))
                PauseGame();

            if (_fightPhaseEnd)
            {
                _fightPhaseEnd = false;
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