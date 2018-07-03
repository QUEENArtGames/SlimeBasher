using System;
using UnityEngine;

namespace Assets.Scripts
{

    [Serializable]
    public class WaveEvent
    {
        public Transform SpawnPoint;
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


        public WaveEvent[] events = new WaveEvent[] { };
        public float delay;
        private int _waveNumber;


        public Wave(int waveNumber)
        {

            _waveNumber = waveNumber;


        }

        public int getAllEnemysOfTheWave()
        {
            int enemys = 0;

            foreach (WaveEvent ev in events)
                enemys += ev.GetSlimeCounter();

            return enemys;
        }

        public WaveEvent[] Events
        {
            get
            {
                return events;
            }

            set
            {
                events = value;
            }
        }


    }
}
