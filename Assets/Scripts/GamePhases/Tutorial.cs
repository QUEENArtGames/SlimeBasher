using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Text _tutorialText;
    public Image _tutorialBackgroundImage;
    public float _fadeDuration = 0.5f; //0.5 secs

    private void Start() {
        StartCoroutine("FadeInCR");
    }

    private IEnumerator FadeInCR() {

        float currentTime = 0f;
        while (currentTime < _fadeDuration) {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / _fadeDuration);
            float backgroundAlpha = Mathf.Lerp(0f, 0.5f, currentTime / _fadeDuration);
            _tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, alpha);
            _tutorialBackgroundImage.color = new Color(_tutorialBackgroundImage.color.r, _tutorialBackgroundImage.color.g, _tutorialBackgroundImage.color.b, backgroundAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void FadeOut() {
        StartCoroutine("FadeOutCR");
    }

    public void FadeIn() {
        StartCoroutine("FadeInCR");
    }

    private IEnumerator FadeOutCR() {

        float currentTime = 0f;
        while (currentTime < _fadeDuration) {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / _fadeDuration);
            float backgroundAlpha = Mathf.Lerp(0.5f, 0f, currentTime / _fadeDuration);
            _tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, alpha);
            _tutorialBackgroundImage.color = new Color(_tutorialBackgroundImage.color.r, _tutorialBackgroundImage.color.g, _tutorialBackgroundImage.color.b, backgroundAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
