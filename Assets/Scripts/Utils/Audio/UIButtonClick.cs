using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIButtonClick : MonoBehaviour {
    [SerializeField] private AudioConfig _audioConfig;
    [SerializeField] private AudioConfig.SFXType _sfxType;

    private AudioSource _source;

    private void Awake() {
        if(GetComponent<Button>() == null) {
            return;
        }
        _source = GetComponent<AudioSource>();
        _source.clip = _audioConfig.GetSFXClip(_sfxType);
        GetComponent<Button>().onClick.AddListener(_source.Play);
    }
}
