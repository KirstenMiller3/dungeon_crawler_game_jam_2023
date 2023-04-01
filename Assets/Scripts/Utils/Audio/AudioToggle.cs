using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioToggle : MonoBehaviour {
    private Image _image;
    private void Awake() {
        _image = GetComponent<Image>();
        if(SettingsManager.Instance != null) {
            _image.sprite = SettingsManager.Instance.GetAudioSetting();
        }
    }

    public void OnClick() {
        if(SettingsManager.Instance != null) {
            SettingsManager.Instance.ToggleAudio(out Sprite sprite);
            _image.sprite = sprite;
        }
    }
}
