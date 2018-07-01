using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPauseMenu : MonoBehaviour {
    public Slider VolumeSlider;

    private void Start()
    {
        VolumeSlider.value = (MainMenu.MainMenuVolume != 0)? MainMenu.MainMenuVolume : AudioListener.volume;
    }
    // Use this for initialization
    public void QuitGame() {
        Application.Quit();
    }

    public void EnableCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnValueChanged()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Game.Instance.ResumeGame();
            gameObject.SetActive(false);
        }
    }
}
