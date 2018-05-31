using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class EnemyManagement : MonoBehaviour {

        private List<GameObject> _normalSlimes;
        private List<GameObject> _hardSlimes;
        private List<GameObject> _gasSlimes;
        private Wave _wave;
        private Game _game;

        public void EnableManager(Game game, Wave wave) {
            _wave = wave;
            _game = game;
            HandleWave();
            
        }

        private void HandleWave() {
            //Für jedes Event in WaveEvents
                //Spawnen der Gegner nacheinander (mit Delay) vom Spawnpoint
                //Füge den jeweiligen Gegner der Liste hinzu
            
        }

        private void Update() {

            //Kontrolliere die Länge der Gegnerlisten
            //wenn alle leer, dann Welle vorbei -> Game.fightPhaseEnd boolean auf true setzen
            //wenn nicht, dann nichts machen

        }

        public void deleteEnemy(SlimeType type, int instanceID) {
            
            //SwitchCase für den richtigen Slimetypen
                //Die richtige Liste durch gehen und wenn die instance ID übereinstimmt, den Slime aus der Liste entfernen
                //Das Gameobject muss dabei noch leben und danach kann es erst gelöscht werden.
        }

        

    }
}
