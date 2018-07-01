using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour {

    public Image imageNormalSpeed;
    public Image imageFastForward;
    public float forwardSpeed = 2.0f;

    private bool _fastForward = false;

	void Awake () {
        if(imageNormalSpeed != null && imageFastForward != null)
        {
            imageNormalSpeed.enabled = true;
            imageFastForward.enabled = false;
        }
        else
        {
            Debug.Log("FastFoward images missing!");
        }
    }
	
	void Update ()
    {
        KeyPressedCheck();
    }

    private void KeyPressedCheck()
    {
        if (Input.GetButtonDown("FastForward"))
        {
            if (_fastForward)
            {
                Time.timeScale = 1;
                SwapImages();
                _fastForward = false;
            }
            else
            {
                Time.timeScale = forwardSpeed;
                SwapImages();
                _fastForward = true;
            }
        }
    }

    private void SwapImages()
    {
        if (imageNormalSpeed.enabled)
        {
            imageNormalSpeed.enabled = false;
            imageFastForward.enabled = true;
        }
        else
        {
            imageNormalSpeed.enabled = true;
            imageFastForward.enabled = false;
        }
    }
}
