using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    class EnemyManagement : MonoBehaviour {

        private List<GameObject> _Slimes; //SLIMEKLASSE
        private Wave _wave;
        private Game _game;

        public void EnableManager(Wave wave) {
            _wave = wave;
            _game = Game.Instance;
            HandleWave();
            
        }

        //Funktion für Gegner  Abstände;

        private void HandleWave() {
            //Für jedes Event in WaveEvents 
                //Event hat Delay nach wann das Event abgehandelt wird (Counter im Update)
                //Spawnen der Gegner nacheinander vom Spawnpoint
                //Füge den jeweiligen Gegner der Liste hinzu
            
        }

        private void Update() {

            //Kontrolliere die Länge der Gegnerlisten
            //wenn alle leer, dann Welle vorbei -> Game.fightPhaseEnd boolean auf true setzen
            //wenn nicht, dann nichts machen

            //Liste der Slimes durchgehen und Lebenspunkte kontrollieren
            //wenn 0 dann deleteEnemy

        }

        public void deleteEnemy(GameObject slime ) { //SlimeKlasse
            
            //Liste durchgehen und dem Slime sagen das er sterben soll
            //Slime aus der Liste entfernen
        }

        

    }
}
