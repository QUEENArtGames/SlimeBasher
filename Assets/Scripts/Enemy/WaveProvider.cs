using System;
using UnityEngine;

namespace Assets.Scripts
{

    [Serializable]
    class WaveProvider
    {
        public Wave[] Waves;
        private int _waveRoundNumber;

        public WaveProvider(int waveRoundNumber)
        {
            _waveRoundNumber = waveRoundNumber;
        }

        public Wave GetNextWave()
        {
            if (_waveRoundNumber > Waves.Length)
            {
                return CreateProceduralWave();
            }
            return Waves[_waveRoundNumber - 1];
        }

        private Wave CreateProceduralWave()
        {
            Wave newWave = new Wave(_waveRoundNumber);
            newWave.Events = new WaveEvent[_waveRoundNumber];
            for (int i = 0; i < newWave.Events.Length; i++)
            {
                newWave.Events[i] = CreateRandomWaveEvent();
            }
            newWave.delay = UnityEngine.Random.Range(0, 5);
            Debug.Log("Delay: " + newWave.delay);
            return newWave;
        }

        private WaveEvent CreateRandomWaveEvent()
        {
            WaveEvent newWaveEvent = new WaveEvent(_waveRoundNumber);
            int counter = UnityEngine.Random.Range(0, Game.Instance.spawnPoints.Length);
            Debug.Log("SpawnPoint an der Stelle: " + counter);
            newWaveEvent.SpawnPoint = Game.Instance.spawnPoints[counter];
            return newWaveEvent;
        }

        public void setWaveNumber(int waveRoundNumber)
        {
            _waveRoundNumber = waveRoundNumber;
        }
    }
}
