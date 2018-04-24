using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoundTest : MonoBehaviour {

    private GameRound _gameRoundScript;
    public float _nextPhaseTimer = 10.0f;
    private Round _actualPhase;
    private bool _startTimer = false;
    private bool _readyButtonEnabled = false;
    private float _timer;

	// Use this for initialization
	void Start () {
        _gameRoundScript = GetComponent<GameRound>();
        _actualPhase = _gameRoundScript.GamePhase;
        RunGame(_actualPhase);
    }

    private void RunGame(Round gamePhase) {
        switch(gamePhase) {
            case Round.Building:
                //Player kann sachen bauen
                Debug.Log("BUILDING");
                _readyButtonEnabled = true;
                break;
            case Round.Prepare:
                //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                Debug.Log("Prepare");
                StartNextRoundCounter();
                break;
            case Round.Fight:
                //Player bekämpft Enemys
                Debug.Log("FIGHT");
                SpawnEnemys();
                break;
            case Round.End:
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
        _actualPhase = _gameRoundScript.GamePhase;

        if (_actualPhase == Round.Building)
            Debug.Log("FAILT");
    }

    // Update is called once per frame
    void Update () {


        if (_startTimer) {
            _timer += Time.deltaTime;
            Debug.Log(10-(int)_timer);
        }

        if(_timer>_nextPhaseTimer) {
            switch(_actualPhase) {
                case Round.Prepare:
                    _gameRoundScript.StartNextPhase();
                    break;
                case Round.End:
                    _gameRoundScript.StartNextPhase();
                    break;
            }
            _timer = 0.0f;
            _startTimer = false;
            CheckGamePhase();
            RunGame(_actualPhase);
        }

        if(_readyButtonEnabled && Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("Starting Countdown");
            _gameRoundScript.StartNextPhase();
            _readyButtonEnabled = false;
            CheckGamePhase();
            RunGame(_actualPhase);
            
        }

        if(_actualPhase == Round.Fight && Input.GetKeyDown(KeyCode.T)) {
            _gameRoundScript.StartNextPhase();
            CheckGamePhase();
            RunGame(_actualPhase);
        }



    }

    
}
