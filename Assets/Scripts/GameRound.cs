using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase {
    Building, Prepare, Fight, End,
}

public class GameRound : MonoBehaviour {

    private Phase _gamePhase;
    private bool _nextPhase = false;

    void Start () {
        _gamePhase = Phase.Building;
	}
	
	void Update () {
        if(_nextPhase) {
            UpdateGamePhase();
            _nextPhase = false;
        }
            
	}

    private void UpdateGamePhase() {
        switch (GamePhase) {
            case Phase.Building:
                _gamePhase = Phase.Prepare;
                break;
            case Phase.Prepare:
                _gamePhase = Phase.Fight;
                break;
            case Phase.Fight:
                _gamePhase = Phase.End;
                break;
            case Phase.End:
                _gamePhase = Phase.Building;
                break;               
        }
    }

    public void StartNextPhase() {
        _nextPhase = true;
    }

    public Phase GamePhase {
        get {
            return _gamePhase;
        }
    }
}
