using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Round {
    Building, Prepare, Fight, End,
}

public class GameRound : MonoBehaviour {

    private Round _gamePhase;
    private bool _nextPhase = false;

    void Start () {
        GamePhase = Round.Building;
	}
	
	void Update () {
        if(_nextPhase) {
            UpdateGamePhase();
            _nextPhase = false;
        }
            
	}

    private void UpdateGamePhase() {
        switch (GamePhase) {
            case Round.Building:
                GamePhase = Round.Prepare;
                break;
            case Round.Prepare:
                GamePhase = Round.Fight;
                break;
            case Round.Fight:
                GamePhase = Round.End;
                break;
            case Round.End:
                GamePhase = Round.Building;
                break;               
        }
    }

    public void StartNextPhase() {
        _nextPhase = true;
    }

    public Round GamePhase {
        get {
            return _gamePhase;
        }

        set {
            _gamePhase = value;
        }
    }


}
