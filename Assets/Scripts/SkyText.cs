using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkyText : Singleton<SkyText> {
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _fadeTime = 2f;

    private float _targetAlpha;

    protected override void Awake() {
        base.Awake();
        _canvasGroup.alpha = 0f;
        _targetAlpha = 0f;
    }

    private void Start() {
        CameraController.Instance.IsLookingUp.Subscribe(OnIsLookingUp, true);
    }

    private void OnDisable() {
        CameraController.Instance.IsLookingUp.Unsubscribe(OnIsLookingUp);
    }

    private void Update() {
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _targetAlpha, Time.deltaTime * _fadeTime);
    }

    public void OnIsLookingUp(bool prev, bool cur) {
        if(cur) {
            ShowText();
        }
        else {
            HideText();
        }
    }

    public void ShowText() {
        _targetAlpha = 1f;
    }

    public void HideText() {
        _targetAlpha = 0f;
    }

    public void SetText(string text, bool isDefault = false) {
        if(isDefault) {
            UIManager.Instance.ClearSkyMessageNotification();
        }
        else {
            UIManager.Instance.ShowNewSkyMessageNotification();
        }
        
        _text.text = text;
    }
}
