using System;
using UnityEngine;

namespace Assets.Scripts
{

    [Serializable]
    class WaveProvider
    {
        public Wave[] _waves;
        private int _waveRoundNumber;

        public WaveProvider(int waveRoundNumber)
        {
            _waveRoundNumber = waveRoundNumber;
        }

        public Wave GetNextWave()
        {
            if (_waveRoundNumber > _waves.Length)
            {
                return CreateProceduralWave();
            }
            return _waves[_waveRoundNumber - 1];
        }

        private Wave CreateProceduralWave()
        {
            Wave newWave = new Wave(_waveRoundNumber);
            newWave.Events = new WaveEvent[_waveRoundNumber];
            for (int i = 0; i < newWave.Events.Length; i++)
            {
                newWave.Events[i] = CreateRandomWaveEvent();
            }
            newWave._delay = UnityEngine.Random.Range(0, 5);
            return newWave;
        }

        private WaveEvent CreateRandomWaveEvent()
        {
            WaveEvent newWaveEvent = new WaveEvent(_waveRoundNumber);
            int counter = UnityEngine.Random.Range(0, Game.Instance.spawnPoints.Length);
            newWaveEvent._spawnPoint = Game.Instance.spawnPoints[counter];
            return newWaveEvent;
        }

        public void SetWaveNumber(int waveRoundNumber)
        {
            _waveRoundNumber = waveRoundNumber;
        }
    }
}
