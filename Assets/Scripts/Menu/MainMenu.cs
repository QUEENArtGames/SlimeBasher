using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Slider VolumeSlider;

    public static float MainMenuVolume;

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void OnValueChanged()
    {
        AudioListener.volume = VolumeSlider.value;
        MainMenuVolume = VolumeSlider.value;
    }
}
