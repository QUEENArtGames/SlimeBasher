using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {

    [Serializable]
    public class Wave2 {
        public WaveEvent[] Events;
        public float delay;
    }
    [Serializable]
    public class WaveEvent {
        public Transform SpawnPoint;
        public int normalSlimes;
        public int hardSlimes;
        public int gasSlimes;
    }
    public class Wave {
        
        private Vector3[] _spawnPoints;
        public float _timeBetweenSpawnings = 5;
        public int _normalSlimes = 5;
        public int _hardSlimes = 0;
        public int _gasSlimes = 0;
        public int _waveNumber;

        public Wave(int waveNumber, Vector3[] spawnPoints) {

            _spawnPoints = spawnPoints;
            _waveNumber = waveNumber;
            createWave();

        }

        private void createWave() {

            _normalSlimes = 5 * _waveNumber;
            _hardSlimes = 2 * (_waveNumber / 2);
            _gasSlimes = 2 * (_waveNumber / 3);
            
            // Wenn Wave 1 dann so bleiben
            if(_waveNumber != 1) {
                _normalSlimes += 5 * _waveNumber;
                if(_waveNumber%2==0) {
                    _hardSlimes += 1 * _waveNumber;
                } else {
                    _hardSlimes += 1 * (_waveNumber-1);
                    if (_hardSlimes < 0)
                        _hardSlimes = 0;
                }
                if(_waveNumber%3==0) {
                    _gasSlimes += 1 * _waveNumber;
                } else {
                    _gasSlimes += 1 * (_waveNumber-2);
                    if (_gasSlimes < 0)
                        _gasSlimes = 0;
                }
            }   
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

        public Vector3[] SpawnPoints {
            get {
                return _spawnPoints;
            }

            set {
                _spawnPoints = value;
            }
        }
    }
}
