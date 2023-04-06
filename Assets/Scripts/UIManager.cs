using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Milo.Tools;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private ConditionMeter _conditionMeter;
    [SerializeField] private TextMeshProUGUI _pickUpText;
    [SerializeField] private Image _skyMessage;
    [SerializeField] private Sprite _skyMessageNone;
    [SerializeField] private Sprite _skyMessageNew;
    [SerializeField] private Gradient _skyMessageNewGradient;
    [SerializeField] private GameObject _giveText;
    [SerializeField] private GameObject _dropText;
    [SerializeField] private GameObject _feedText;
    [SerializeField] private GameObject _mirrorInteractPrompt;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeSpeed = 2f;

    [SerializeField] private PickUpDetection _pickUpDetection;
    [SerializeField] private PlayerMirrorDetection _mirrorDetection;

    private string _pickUpBase;
    private bool _onMirror;
    private bool _onFoodMonster;
    private int _currentCondition;
    private float _targetFadeAlpha = 0f;

    public void Start() {
        _fadeCanvasGroup.alpha = 1f;
        _pickUpBase = _pickUpText.text;

        _pickUpDetection = FindObjectOfType<PickUpDetection>();
        _mirrorDetection = FindObjectOfType<PlayerMirrorDetection>();

        PlayerManager.Instance.ConditionLevel.Subscribe(OnUpdateCondition, true);
        _pickUpDetection.LookAtObjName.Subscribe(OnLookAtObjName, true);
        _pickUpDetection.LookingAtObj.Subscribe(OnLookAtObj, true);
        _mirrorDetection.LookingAtMirror.Subscribe(OnMirror, true);

        ShowMirrorInteractPrompt(false);

        _fadeCanvasGroup.DOFade(0f, 1f / _fadeSpeed);
    }

    public void OnDisable() {
        PlayerManager.Instance.ConditionLevel.Unsubscribe(OnUpdateCondition);
        _pickUpDetection.LookAtObjName.Unsubscribe(OnLookAtObjName);
        _pickUpDetection.LookingAtObj.Unsubscribe(OnLookAtObj);
        _mirrorDetection.LookingAtMirror.Unsubscribe(OnMirror);
    }

    private void Update() {
        if(!_pickUpDetection.CanPresentToMirror) {
            _giveText.gameObject.SetActive(false);
            _dropText.gameObject.SetActive(false);
        } 
        else {
            _giveText.gameObject.SetActive(_onMirror);
            _dropText.gameObject.SetActive(!_onMirror);
        }

        _feedText.gameObject.SetActive(_pickUpDetection.CanPresentToMonster);
    }

    public void ShowMirrorInteractPrompt(bool show) {
        _mirrorInteractPrompt.SetActive(show);
    }

    private void OnUpdateCondition(int prev, int curr) {
        //_conditionMeter.text = $"{curr}%";
        _currentCondition = curr;
        _conditionMeter.SetLevel(curr);
    }

    private void OnLookAtObj(bool prev, bool curr) {
        _pickUpText.transform.parent.gameObject.SetActive(curr);
    }

    private void OnMirror(bool prev, bool curr) {
        _onMirror = curr;
    }

    private void OnLookAtObjName(string prev, string curr) {
        _pickUpText.text = string.Format(_pickUpBase, curr);
    }

    public void SetCanvasFade(float alpha) {
        _targetFadeAlpha = alpha;
    }

    public void Transition(System.Action onReady) {
        _fadeCanvasGroup.DOFade(1f, 1f / _fadeSpeed).OnComplete(() => TransitionBack(onReady));
        // = Mathf.Lerp(_fadeCanvasGroup.alpha, _targetFadeAlpha, Time.deltaTime * _fadeSpeed);
    }

    private void TransitionBack(System.Action onReady) {
        onReady?.Invoke();
        _fadeCanvasGroup.DOFade(0f, 1f / _fadeSpeed);
    }

    [ContextMenu("Show New Sky Message")]
    public void ShowNewSkyMessageNotification() {
        _skyMessage.sprite = _skyMessageNew;
        Sequence seq = DOTween.Sequence();
        seq.Append(_skyMessage.DOGradientColor(_skyMessageNewGradient, 0.7f))
            .Join(_skyMessage.rectTransform.DOPunchScale(Vector3.one * 1.3f, 0.7f, 1));
    }

    [ContextMenu("Hide Sky Message")]
    public void ClearSkyMessageNotification() {
        _skyMessage.sprite = _skyMessageNone;
        _skyMessage.rectTransform.DOPunchScale(Vector3.one * 1.05f, 1f, 1);
    }
}
