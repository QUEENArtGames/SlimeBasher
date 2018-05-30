using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class EnemyManagement : MonoBehaviour {

        private List<GameObject> _Slimes = new List<GameObject>(); //SLIMEKLASSE
        private Wave _wave;
        private Game _game;
        private float _timer = 0;
        private bool _timerAllowed = false;
        private bool _eventAllowed = false;
        private int _eventCounter = 0;
        public GameObject _normalSlime;
        private WaveEvent actualEvent;
        //public GameObject _hardSlime;
        //public GameObject _gasSlime;

        public void EnableManager(Wave wave) {
            _wave = wave;
            _game = Game.Instance;
            HandleWave();
            
        }

        //Funktion für Gegner Abstände;

        private void HandleWave() {
            //Für jedes Event in WaveEvents 
                //Event hat Delay nach wann das Event abgehandelt wird (Counter im Update)
                //Spawnen der Gegner nacheinander vom Spawnpoint
                //Füge den jeweiligen Gegner der Liste hinzu

            
                if(_eventCounter < _wave.Events.Length) {
                    actualEvent = _wave.Events.ElementAt(_eventCounter);
                    HandleEvent();
                    _eventCounter++;
                }
            
            
        }

        private void SpawnNormalSlimes() {
            _Slimes.Add(Instantiate(_normalSlime, actualEvent.SpawnPoint.transform.position, actualEvent.SpawnPoint.rotation));
        }
        /*
        private void SpawnGasSlimes() {
            _Slimes.Add(Instantiate(_gasSlimes, waveEvent.SpawnPoint.transform.position, waveEvent.SpawnPoint.rotation));
        }

        private void SpawnHardSlimes() {
            _Slimes.Add(Instantiate(_hardSlimes, waveEvent.SpawnPoint.transform.position, waveEvent.SpawnPoint.rotation));
        } */

        private void HandleEvent() {
            for(int i=0; i< actualEvent._normalSlimes; i++) {
                
                Invoke("SpawnNormalSlimes", 1.5f);
            }

            /*
            for (int i = 0; i < waveEvent._gasSlimes; i++) {
                
                Invoke("SpawnGasSlimes", 2.0f);
            }

            for (int i = 0; i < waveEvent._hardSlimes; i++) {
                
                Invoke("SpawnHardSlimes", 2.5f);
            } */

            _timerAllowed = true;
        }

        private void Update() {

            if (_eventAllowed) {
                _eventAllowed = false;
                HandleWave();
            }

            if (_timerAllowed) {
                _timer += Time.deltaTime;
                if(_timer>_wave.delay) {
                    _eventAllowed = true;
                    _timerAllowed = false;
                    _timer = 0f;
                }
            }

            if(CheckIfSlimesAlive() && _eventCounter >= _wave.events.Length) {
                _game.GamePhase.MoveToNextGamePhase();
            }

            //Kontrolliere die Länge der Gegnerlisten
            //wenn alle leer, dann Welle vorbei -> Game.fightPhaseEnd boolean auf true setzen
            //wenn nicht, dann nichts machen

            //Liste der Slimes durchgehen und Lebenspunkte kontrollieren
            //wenn 0 dann deleteEnemy

        }

        private bool CheckIfSlimesAlive() {

            foreach(GameObject slime in _Slimes) {
                if(slime.GetComponent<EnemyDummy>().Hitpoints <= 0) {
                    deleteEnemy(slime);
                    break;
                }
            }

            if (_Slimes.Count == 0)
                return true;
            return false;
                
        }

        public void deleteEnemy(GameObject slime ) { //SlimeKlasse

            _Slimes.Remove(slime);
            slime.GetComponent<EnemyDummy>().Kill();
            //Liste durchgehen und dem Slime sagen das er sterben soll
            //Slime aus der Liste entfernen
        }

        

    }
}
