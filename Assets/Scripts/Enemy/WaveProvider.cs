using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {

    [Serializable]
    class WaveProvider {
        public Wave[] Waves = new Wave[] { };
        private int _waveRoundNumber;

        public WaveProvider(int waveRoundNumber) {
            _waveRoundNumber = waveRoundNumber;
        }

        public Wave GetNextWave() {
            Debug.Log(Waves.Length);
            if (_waveRoundNumber >= Waves.Length) {
                return CreateProceduralWave();
            }
            return Waves[_waveRoundNumber];
        }

        private Wave CreateProceduralWave() {
            Wave newWave = new Wave(_waveRoundNumber);
            newWave.Events = new WaveEvent[_waveRoundNumber];
            for (int i = 0; i < newWave.Events.Length; i++) {
                newWave.Events[i] = CreateRandomWaveEvent();
            }
            return newWave;
        }

        private WaveEvent CreateRandomWaveEvent() {
            WaveEvent newWaveEvent = new WaveEvent(_waveRoundNumber);
            return newWaveEvent;
        }

    }
}
