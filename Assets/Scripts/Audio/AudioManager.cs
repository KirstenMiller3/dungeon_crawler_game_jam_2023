using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    [SerializeField] private AudioMixer audioMixer;

    private float _citySoundsTimer;
    
    private float _citySoundsStopTimer;
    
    private float _citySoundsPlayTime;

    private string currCitySfxPlaying;

    private int lastCreatedSectionNum = 1;



    public static AudioManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
        

        Play("main");
        Play("main_2");

    }


    public void Update() {
        _citySoundsTimer += Time.deltaTime;
        _citySoundsStopTimer += Time.deltaTime;

        if (_citySoundsTimer >= 7f && CitySoundEffectIsNotPlaying()) {
            _citySoundsTimer = 0;
            _citySoundsStopTimer = 0;
            _citySoundsPlayTime = UnityEngine.Random.Range(2f, 6f);
            int num =GetUniqueRandomSectionNumber(5);
            currCitySfxPlaying = $"city_{num}";
            Debug.Log($"starting sfx {currCitySfxPlaying} {_citySoundsTimer} {_citySoundsPlayTime}");
            Play(currCitySfxPlaying);
        }

        if (_citySoundsStopTimer >= _citySoundsPlayTime && !CitySoundEffectIsNotPlaying()) {
            Debug.Log($"stopping sfx {_citySoundsTimer} {_citySoundsPlayTime}");
            Sound s = Array.Find(sounds, sound => sound.sName == currCitySfxPlaying);
            IEnumerator fadeSound1 = AudioFadeOut.FadeOut(s.source, 0.5f);
            StartCoroutine (fadeSound1);
            _citySoundsTimer = 0;
            _citySoundsStopTimer = 0;
        }
        
    }

    public bool IsSoundPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.sName == name);
        if (s == null) {
            return false;
        } else {
            return s.source.isPlaying;
        }
    }

    private static bool CitySoundEffectIsNotPlaying() {
        return !instance.sounds.Where(s => s.sName.Contains("city")).Any(x => x.source.isPlaying);
    }

    private bool _neverActivateAgain = false;
    public void LockHeartPuzzle() {
        _neverActivateAgain = true;
    }

    public void SetLowPassOn(bool isOn) {
        if(_neverActivateAgain) {
            audioMixer.SetFloat("LowPass", 5000);
            audioMixer.SetFloat("MusicVol", 0);
            return;
        }
        
        if(isOn) {
            audioMixer.SetFloat("LowPass", 555);
            audioMixer.SetFloat("MusicVol", -80);
        }
        else {
            audioMixer.SetFloat("LowPass", 5000);
            audioMixer.SetFloat("MusicVol", 0);
        }
    }

    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.sName == name);

        if(s != null)
        {
            Debug.Log($"playing sound {name}");
            s.source.Play();
        }
        else
        {
            Debug.LogError($"Sound {name} was not found.");
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.sName == name);

        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogError($"Sound {name} was not found.");
        }
    }
    
    
    public static class AudioFadeOut {
 
        public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
            float startVolume = audioSource.volume;
 
            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
                yield return null;
            }
 
            audioSource.Stop ();
            audioSource.volume = startVolume;
        }
 
    }
    
    int GetUniqueRandomSectionNumber(int totalSections){
        bool hasFoundUniqueRandomSelection = false;
        int tempSectionNum = 1;
 
        while (!hasFoundUniqueRandomSelection) {
            tempSectionNum  = UnityEngine.Random.Range(1, totalSections);
            if(tempSectionNum != lastCreatedSectionNum){
                hasFoundUniqueRandomSelection = true;
                lastCreatedSectionNum = tempSectionNum;
            }
        }
        return tempSectionNum;
    }

}