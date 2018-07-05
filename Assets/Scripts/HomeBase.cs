using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HomeBase : MonoBehaviour
{
    public Slider BaseWaterSlider;
    public int NumberOfSlimesUntilDeath = 10;
    public float NeededSlimeDistanceToBase = 3.0f;
    private int _currentNumberOfSlimesUntilDeath;
    private EnemyManagement _enemymanagement;

    private void Start()
    {
        BaseWaterSlider.value = NumberOfSlimesUntilDeath;
        BaseWaterSlider.maxValue = NumberOfSlimesUntilDeath;
        _currentNumberOfSlimesUntilDeath = NumberOfSlimesUntilDeath;
        _enemymanagement = FindObjectOfType<EnemyManagement>();
    }

    private void Update()
    {
        CheckForDamage();

        if (_currentNumberOfSlimesUntilDeath <= 0)
            SetGameOver();
    }

    private void CheckForDamage()
    {
        foreach (GameObject slimeObject in _enemymanagement.Slimes)
        {
            if (Vector3.Distance(slimeObject.transform.position, transform.position) <= NeededSlimeDistanceToBase)
            {
                slimeObject.GetComponent<SlimeScript>()._hitpoints = 0;
                _currentNumberOfSlimesUntilDeath--;
                BaseWaterSlider.value = _currentNumberOfSlimesUntilDeath;
            }
        }

    }

    private void SetGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void TakeDamage(EnemyDummy slime)
    {
        Debug.Log("ARGH");
        slime.Kill();
        _currentNumberOfSlimesUntilDeath--;
    }
}
