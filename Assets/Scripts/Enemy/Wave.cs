using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {

    [Serializable]
    public class WaveEvent {
        public Transform SpawnPoint;
        public int _normalSlimes;
        public int _hardSlimes;
        public int _gasSlimes;
        private int _waveNumber;

        public WaveEvent(int waveNumber) {
            _waveNumber = waveNumber;
            createWave();
        }

        private void createWave() {

            _normalSlimes = 5 * _waveNumber;
            _hardSlimes = 2 * (_waveNumber / 2);
            _gasSlimes = 2 * (_waveNumber / 3);


        }
    }

    [Serializable]
    public class Wave {

        public WaveEvent[] Events;
        public float delay;
        private int _waveNumber;

        public Wave(int waveNumber) {

            _waveNumber = waveNumber;
            

        }
    }
}
