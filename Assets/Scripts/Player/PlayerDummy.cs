using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

//TESTFUNKTIONEN
public class PlayerDummy : MonoBehaviour
{
    private PlayerScrapDropAndCollection _playerScrapDropAndCollection;
    private TowerBuilding _towerBuilding;
    private PlayerScrapInventory _scrapInventory;
    private TowerBuildingUI _towerBuildingUI;
    public int _playerHealth = 100;
    public Slider _healthSlider;
    public GameObject _endUI;
    public float _moveCharacterTimer = 15f;
    private float _timer;
    public Transform _playerSpawnPoint;

    private bool _isInDefaultMode = true;
    private PlayerSounds _playersounds;
    private bool _allowsounds = true;
    private Animator _anim;

    public int MaxDistanceToTower = 2;

    private void Start()
    {
        _playersounds = GetComponent<PlayerSounds>();
        _anim = GetComponent<Animator>();
    }

    public bool IsInDefaultMode
    {
        get
        {
            return _isInDefaultMode;
        }

        set
        {
            _isInDefaultMode = value;
        }
    }

    public int PlayerHealth
    {
        get
        {
            return _playerHealth;
        }
    }

    void Awake()
    {
        _playerScrapDropAndCollection = GetComponent<PlayerScrapDropAndCollection>();
        _towerBuilding = FindObjectOfType<TowerBuilding>();
        _scrapInventory = GetComponent<PlayerScrapInventory>();
        _towerBuildingUI = FindObjectOfType<TowerBuildingUI>();
        _healthSlider.value = PlayerHealth;
    }

    void Update()
    {

        if (PlayerHealth <= 0)
        {
            _anim.SetBool("Dead", true);
            _endUI.SetActive(true);
            _timer += Time.deltaTime;
            _playerScrapDropAndCollection.DropScraps();
            if (_allowsounds)
            {
                _playersounds.PlayKoSound();
                _allowsounds = false;
            }
        }

        if (_timer >= _moveCharacterTimer)
        {
            _anim.SetBool("Dead", false);
            gameObject.transform.position = _playerSpawnPoint.position;
            _endUI.SetActive(false);
            _playerHealth = 100;
            _healthSlider.value = _playerHealth;
            _timer = 0f;
            _allowsounds = true;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.transform.gameObject.CompareTag("Tower") && Vector3.Distance(transform.position, hit.transform.position) <= MaxDistanceToTower && IsInDefaultMode)
            {
                _towerBuildingUI.ShowTowerKillNotification();
                TowerRessourceManagement towermanagement = hit.transform.gameObject.GetComponent<TowerRessourceManagement>();
                if (_towerBuilding.CheckForRessources(_scrapInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
                    _towerBuildingUI.ShowTowerUpgraeNotification();

                if (Input.GetButtonDown("Upgrade Tower") && IsInDefaultMode)
                {
                    _towerBuildingUI.OpenTowerBuildingMenu(hit.transform.gameObject);
                }

                if (Input.GetButtonDown("Deconstruct Tower") && IsInDefaultMode)
                {
                    _towerBuildingUI.CloseTowerUpgradeNotification();
                    _towerBuildingUI.CloseTowerBuildingMenu();
                    hit.transform.gameObject.GetComponent<Tower>().Kill();
                }

                if (Input.GetButtonDown("QuickUpgrade Tower") && IsInDefaultMode)
                {
                    _towerBuildingUI.CloseTowerUpgradeNotification();
                    _towerBuildingUI.CloseTowerBuildingMenu();
                    _towerBuilding.UpgradeWithAnyScrap(hit.transform.gameObject.GetComponent<TowerRessourceManagement>());
                }
            }
            else
            {
                _towerBuildingUI.CloseTowerKillNotification();
                _towerBuildingUI.CloseTowerUpgradeNotification();
            }

        }

    }

    internal void Damage(int damage)
    {
        _playerHealth -= damage;
        _healthSlider.value = _playerHealth;
        _playersounds.PlayPainSound();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.CompareTag("Scrap") && _playerHealth > 0)
            _playerScrapDropAndCollection.CollectScrap(other.transform.gameObject);
    }

}
