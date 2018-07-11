using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class WaveEvent
    {
        public GameObject _spawnPoint;
        public int _normalSlimes;
        public int _hardSlimes;
        public int _gasSlimes;

        private int _waveNumber;


        public WaveEvent(int waveNumber)
        {
            _waveNumber = waveNumber;
            CreateWave();
        }

        private void CreateWave()
        {
            _normalSlimes = 5 * _waveNumber;
            _hardSlimes = 2 * (_waveNumber / 2);
            _gasSlimes = 2 * (_waveNumber / 3);
        }

        public int GetSlimeCounter()
        {
            return _normalSlimes + _hardSlimes + _gasSlimes;
        }
    }

    [Serializable]
    public class Wave
    {
        public WaveEvent[] _events = new WaveEvent[] { };
        public float _delay;

        private int _waveNumber;

        public Wave(int waveNumber)
        {
            _waveNumber = waveNumber;
        }

        public int GetAllEnemysOfTheWave()
        {
            int enemys = 0;

            foreach (WaveEvent ev in _events)
                enemys += ev.GetSlimeCounter();

            return enemys;
        }

        public WaveEvent[] Events
        {
            get
            {
                return _events;
            }

            set
            {
                _events = value;
            }
        }
    }
}
