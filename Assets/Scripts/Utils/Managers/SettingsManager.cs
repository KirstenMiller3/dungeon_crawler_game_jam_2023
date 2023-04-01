using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Milo.Tools;

public class SettingsManager : Singleton<SettingsManager> {

    [SerializeField] private UnityEngine.Audio.AudioMixer _mixer;
    [SerializeField] private Sprite _audioUpSprite;
    [SerializeField] private Sprite _audioDownSprite;
    [SerializeField] private GameObject _ppVolume;
    [SerializeField] private TextMeshProUGUI _qualityText;

    [Header("Cursor")]
    [SerializeField] private Texture2D _menuCursorTexture;
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private CursorMode _cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 _hotSpot = Vector2.zero;

    public bool _isHighQuality = true;
    private bool _muted = false;
    private bool _isCursorVisible = false;

    private void Start() {
        DontDestroyOnLoad(this);
    }

    private void Update() {
        //if(Input.GetKeyDown(KeyCode.Escape)) {
        //    _isCursorVisible = !_isCursorVisible;
        //    Cursor.visible = _isCursorVisible;
        //}
        //if(Input.GetMouseButtonDown(0)) {
        //    _isCursorVisible = false;
        //    Cursor.visible = _isCursorVisible;
        //}
    }

    public void SetMenuCursor() {
        Cursor.SetCursor(_menuCursorTexture, _hotSpot, _cursorMode);
    }

    public void SetGameCursor() {
        Cursor.SetCursor(_cursorTexture, _hotSpot, _cursorMode);
    }

    public Sprite GetAudioSetting() {
        if(_muted) {
            return _audioDownSprite;
        }
        else {
            return _audioUpSprite;
        }
    }

    public void ToggleAudio(out Sprite sprite) {
        _muted = !_muted;

        if(_muted) {
            _mixer.SetFloat("Volume", -80f);
            sprite = _audioDownSprite;
        }
        else {
            _mixer.SetFloat("Volume", 0f);
            sprite = _audioUpSprite;
        }
    }

    public void ToggleQuality() {
        _isHighQuality = !_isHighQuality;

        if(_isHighQuality) {
            _ppVolume.SetActive(true);
            _qualityText.text = "Quality: [HIGH]";
        }
        else {
            _ppVolume.SetActive(false);
            _qualityText.text = "Quality: [LOW]";
        }
    }
}
