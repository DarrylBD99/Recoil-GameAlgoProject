using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundEffects;
    public static AudioManager instance;
    
    // Set instance to call static function
    void Start() { instance = this; }

    // Play sound effect
    public static void PlaySoundEffect(AudioClip audio) {
        if (audio == instance.soundEffects.clip && instance.soundEffects.isPlaying)
            return;

        instance.soundEffects.clip = audio;
        instance.soundEffects.Play();
    }
}
