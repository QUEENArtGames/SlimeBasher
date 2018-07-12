using PathCreator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    class EnemyManagement : MonoBehaviour
    {

        public GameObject _normalSlime;
        public GameObject _hardSlime;
        public GameObject _gasSlime;

        private List<GameObject> _slimes = new List<GameObject>();
        private Wave _wave;
        private Game _game;
        private WaveEvent _actualEvent;
        private bool _timerAllowed = false;
        private bool _eventAllowed = false;
        private bool _allEnemysSpawned = false;
        private float _timer = 0;
        private int _eventCounter = 0;
        private int _spawnCounter = 0;


        public List<GameObject> Slimes
        {
            get
            {
                return _slimes;
            }

            set
            {
                _slimes = value;
            }
        }

        public void EnableManager(Wave wave)
        {
            _wave = wave;
            _game = Game.Instance;
            HandleWave();
        }

        private void HandleWave()
        {

            if (_eventCounter < _wave.Events.Length)
            {
                _actualEvent = _wave.Events.ElementAt(_eventCounter);
                StartCoroutine("HandleEvent");
                _eventCounter++;
            }


        }

        IEnumerator HandleEvent()
        {
            for (int i = 0; i < _actualEvent._normalSlimes; i++)
            {
                yield return new WaitForSeconds(2.5f);
                _slimes.Add(Instantiate(_normalSlime, _actualEvent._spawnPoint.transform.position, Quaternion.identity));
                _spawnCounter++;
            }

            for (int i = 0; i < _actualEvent._hardSlimes; i++)
            {
                yield return new WaitForSeconds(3.5f);
                _slimes.Add(Instantiate(_hardSlime, _actualEvent._spawnPoint.transform.position, Quaternion.identity));
                _spawnCounter++;
            }

            for (int i = 0; i < _actualEvent._gasSlimes; i++)
            {
                yield return new WaitForSeconds(4.5f);

                _gasSlime.GetComponent<PC_PathAgent>().path = _actualEvent._spawnPoint.GetComponent<PC_Path>();
                _slimes.Add(Instantiate(_gasSlime, _actualEvent._spawnPoint.transform.position, Quaternion.identity));
                _spawnCounter++;
            }

            _timerAllowed = true;

        }

        private void Update()
        {
            if (_spawnCounter == _wave.GetAllEnemysOfTheWave())
            {
                _allEnemysSpawned = true;
                _spawnCounter = 0;
            }

            if (_eventAllowed)
            {
                _eventAllowed = false;
                HandleWave();
            }

            if (_timerAllowed)
            {
                _timer += Time.deltaTime;
                if (_timer > _wave._delay)
                {
                    _eventAllowed = true;
                    _timerAllowed = false;
                    _timer = 0f;
                }
            }

            if (CheckIfSlimesAlive() && _eventCounter >= _wave._events.Length && _allEnemysSpawned)
            {
                _eventCounter = 0;
                _allEnemysSpawned = false;
                _game.GamePhase.MoveToNextGamePhase();

            }

        }

        private bool CheckIfSlimesAlive()
        {
            foreach (GameObject slime in _slimes)
            {
                if (slime.GetComponent<SlimeScript>()._hitpoints <= 0)
                {
                    DeleteEnemy(slime);
                    break;
                }
            }

            if (_slimes.Count == 0)
                return true;

            return false;
        }

        public void DeleteEnemy(GameObject slime)
        {
            _slimes.Remove(slime);
            slime.GetComponent<SlimeScript>().Kill();
        }
    }
}
