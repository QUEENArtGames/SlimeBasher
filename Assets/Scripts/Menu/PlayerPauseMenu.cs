using UnityEngine;
using UnityEngine.UI;

public class PlayerPauseMenu : MonoBehaviour {
    public Slider VolumeSlider;

    private void Start()
    {
        VolumeSlider.value = MainMenu.MainMenuVolume;
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
}
