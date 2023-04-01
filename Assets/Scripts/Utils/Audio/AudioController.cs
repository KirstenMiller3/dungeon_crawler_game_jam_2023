using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;

public class AudioController : Singleton<AudioController> {
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _fadeOutSpeed = 5f;
    [SerializeField] private AudioSource _ambienceSource;

    private float _volumeTarget;
    private float _maxVolume;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start() {
        _maxVolume = _musicSource.volume;
        _volumeTarget = _musicSource.volume;

        _musicSource.volume = 0;
    }

    public void Update() {
        _musicSource.volume = Mathf.Lerp(_musicSource.volume, _volumeTarget, Time.deltaTime * _fadeOutSpeed);
    }

    public void SetMusicClip(AudioClip clip) {
        if(_musicSource.isPlaying && _musicSource.clip == clip) {
            return;
        }

        _musicSource.clip = clip;
        _musicSource.Play();
    }


    public void FadeOut() {
        _volumeTarget = 0f;
    }

    public void FadeIn() {
        _volumeTarget = _maxVolume;
    }

    public void SetAmbienceSource(bool isOn) {
        if(isOn) {
            _ambienceSource.Play();
        }
        else {
            _ambienceSource.Stop();
        }
    }
}
