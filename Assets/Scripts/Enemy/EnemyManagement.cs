using System;
using System.Collections;
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
        private bool _allEnemysSpawned = false;
        private int _eventCounter = 0;
        private int _spawnCounter = 0;
        public GameObject _normalSlime;
        private WaveEvent actualEvent;
        public GameObject _hardSlime;
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
                    //HandleEvent();
                    StartCoroutine("HandleEvent");
                    _eventCounter++;
                }
            
            
        }

        IEnumerator HandleEvent() {
            for(int i=0; i< actualEvent._normalSlimes; i++) {

                yield return new WaitForSeconds(2.5f);
                _Slimes.Add(Instantiate(_normalSlime, actualEvent.SpawnPoint.transform.position, actualEvent.SpawnPoint.rotation));
                _spawnCounter++;
                _timerAllowed = true;
            }


            
            for(int i=0; i< actualEvent._hardSlimes; i++) {

                yield return new WaitForSeconds(3.5f);
                _Slimes.Add(Instantiate(_hardSlime, actualEvent.SpawnPoint.transform.position, actualEvent.SpawnPoint.rotation));
                
            } /*

            for(int i=0; i< actualEvent._gasSlimes; i++) {

                yield return new WaitForSeconds(4.5f);
                _Slimes.Add(Instantiate(_gasSlime, actualEvent.SpawnPoint.transform.position, actualEvent.SpawnPoint.rotation));
                
            } */

            
            
        }

        private void Update() {

            if(_spawnCounter == _wave.getAllEnemysOfTheWave()) {
                _allEnemysSpawned = true;
                _spawnCounter = 0;
            }

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

            //Einmal Spawnen EventCounter = 1 und Event Länge = 1 -> true
            //Einmal Spawnen EventCounter = 1 und Event Länge = 2 -> false
            //Zweimal Spawnen EventCounter = 2 und Event Länge = 2 -> true
            if(CheckIfSlimesAlive() && _eventCounter >= _wave.events.Length && _allEnemysSpawned) {
                _game.GamePhase.MoveToNextGamePhase();
                _eventCounter = 0;
                _allEnemysSpawned = false;
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
