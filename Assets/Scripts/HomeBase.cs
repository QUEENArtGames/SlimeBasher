using Assets.Scripts;
using System.Collections;
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
            StartCoroutine(SetGameOver());
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

    private IEnumerator SetGameOver()
    {
        FindObjectOfType<GameOverUI>().ShowGameOverPanel();
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void TakeDamage(EnemyDummy slime)
    {
        slime.Kill();
        _currentNumberOfSlimesUntilDeath--;
    }
}
