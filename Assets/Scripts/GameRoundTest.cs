using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoundTest : MonoBehaviour {

    private GameRound _gameRoundScript;
    private int _enemyCounter = 0;
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
                _readyButtonEnabled = true;
                break;
            case Round.Prepare:
                //Player bereitet sich auf die jetzt kommende Nächste Phase vor
                StartNextRoundCounter();
                break;
            case Round.Fight:
                //Player bekämpft Enemys
                SpawnEnemys();
                break;
            case Round.End:
                //Rundenende, bereitmachen für Bauphase
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
    }

    // Update is called once per frame
    void Update () {

        CheckGamePhase();
		
        if(_startTimer) {
            _timer += Time.deltaTime;
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
        }

        if(_readyButtonEnabled && Input.GetButtonDown("G")) {
            _gameRoundScript.StartNextPhase();
        }

        
	}

    
}
