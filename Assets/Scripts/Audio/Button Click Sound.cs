using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private Button button;
    AudioManager audioManager;

    private void Start()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // Add listener to play the button press sound
        button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonpress);
        }
    }
}
