using SlimeBasher.Cameras;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public Text _tutorialText;
    public Image _tutorialBackgroundImage;
    public float _fadeDuration = 0.5f; //0.5 secs

    private bool _firstTut = true;
    private float _showTime = 3;

    private void Start()
    {
        StartCoroutine("FadeInCR");
    }

    public void FadeOut()
    {
        StartCoroutine("FadeOutCR");
    }

    public void FadeOutV(float time)
    {
        _showTime = time;
        StartCoroutine("FadeOutCRV");
    }

    public void FadeIn()
    {
        StartCoroutine("FadeInCR");
    }

    private IEnumerator FadeInCR()
    {

        float currentTime = 0f;
        while (currentTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / _fadeDuration);
            float backgroundAlpha = Mathf.Lerp(0f, 1.0f, currentTime / _fadeDuration);
            _tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, alpha);
            _tutorialBackgroundImage.color = new Color(_tutorialBackgroundImage.color.r, _tutorialBackgroundImage.color.g, _tutorialBackgroundImage.color.b, backgroundAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private IEnumerator FadeOutCR()
    {

        float currentTime = 0f;
        while (currentTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / _fadeDuration);
            float backgroundAlpha = Mathf.Lerp(0.5f, 0f, currentTime / _fadeDuration);
            _tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, alpha);
            _tutorialBackgroundImage.color = new Color(_tutorialBackgroundImage.color.r, _tutorialBackgroundImage.color.g, _tutorialBackgroundImage.color.b, backgroundAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private IEnumerator FadeOutCRV()
    {
        float showTime = _showTime;
        while (showTime > 0)
        {
            showTime -= Time.deltaTime;
            yield return null;
        }
        FadeOut();
        yield break;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _firstTut)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<FreeLookCam>().enabled = true;
            FadeOut();
            _firstTut = false;
        }
    }
}
