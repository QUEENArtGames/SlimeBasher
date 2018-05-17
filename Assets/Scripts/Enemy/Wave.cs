using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class Wave {
        
        private List<Vector3> _spawnPoints;
        private float _timeBetweenSpawnings = 5;
        private int _normalSlimes = 10;
        private int _hardSlimes = 0;
        private int _gasSlimes = 0;
        private int _waveNumber;

        public Wave(int waveNumber) {

            _waveNumber = waveNumber;
            createWave();

        }

        private void createWave() {
            
            // Wenn Wave 1 dann so bleiben
            if(_waveNumber != 1) {
                _normalSlimes += 5;
                _hardSlimes += 1 * _waveNumber;
                _gasSlimes += 1 * _waveNumber;

            }


            
        }

        public Wave(List<Vector3> spawnPoints, int[] enemys, int timeBetweenSpawnings) {

        }

        public int getNormalSlimes {
            get {
                return _normalSlimes;
            }
        }

        public int getHardSlimes {
            get {
                return _hardSlimes;
            }
        }

        public int getGasSlimes {
            get {
                return _gasSlimes;
            }

        }

        public float getTimeBetweenSpawnings {
            get {
                return _timeBetweenSpawnings;
            }

        }
    }
}
