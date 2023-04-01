using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Data/AudioConfig", order = 1)]
public class AudioConfig : ScriptableObject {
    public enum SFXType {
        Standard_Button = 0,
    }

    [Header("Music")]
    public AudioClip MenuMusic;
    public AudioClip GameMusic;

    [Header("SFX")]
    public AudioClip ButtonSFX;


    public AudioClip GetSFXClip(SFXType type) {
        switch(type) {
            case SFXType.Standard_Button:
                return ButtonSFX;
            default:
                return null;
        }
    }
}