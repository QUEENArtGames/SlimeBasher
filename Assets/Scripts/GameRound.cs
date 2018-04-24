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
                
                if (GamePhase == Round.Building) {
                    Debug.Log("FAIL");
                }
                break;
            case Round.Prepare:
                GamePhase = Round.Fight;
                Debug.Log("GamePhase aktualiesiert");
                break;
            case Round.Fight:
                GamePhase = Round.End;
                Debug.Log("GamePhase aktualiesiert");
                break;
            case Round.End:
                GamePhase = Round.Building;
                Debug.Log("GamePhase aktualiesiert");
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
