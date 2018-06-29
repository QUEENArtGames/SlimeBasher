﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUIFading : MonoBehaviour {

    private GameObject _towerUI;
    public float _fadeDuration = 3f;


    private void Start() {
        _towerUI = gameObject;
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
            Vector3 uiPosition = gameObject.transform.position;
            float position = Mathf.Lerp(uiPosition.y, -25, currentTime / _fadeDuration);
            gameObject.transform.position = new Vector3(uiPosition.x, position, uiPosition.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private IEnumerator FadeInCR() {

        float currentTime = 0f;
        while (currentTime < _fadeDuration) {
            Vector3 uiPosition = gameObject.transform.position;
            float position = Mathf.Lerp(uiPosition.y, 25, currentTime / _fadeDuration);
            gameObject.transform.position = new Vector3(uiPosition.x, position, uiPosition.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}