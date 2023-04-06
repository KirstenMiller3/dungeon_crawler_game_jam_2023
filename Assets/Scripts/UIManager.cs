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

    [Header("Sky Message")]
    [SerializeField] private Image _skyMessageOutline;
    [SerializeField] private Image _skyMessageMail;
    [SerializeField] private Image _skyMessageStars;
    [SerializeField] private Sprite _skyMessageMailStars;
    [SerializeField] private Sprite _skyMessageNormalStars;

    [Header("Text")]
    [SerializeField] private GameObject _giveText;
    [SerializeField] private GameObject _dropText;
    [SerializeField] private GameObject _feedText;
    [SerializeField] private GameObject _mirrorInteractPrompt;

    [Header("Fades")]
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private CanvasGroup _presenceGroup;

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
        ClearSkyMessageNotification();
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


    public void SetPresence() {
        _presenceGroup.alpha = 1f;
    }

    public void EndPresence() {
        _presenceGroup.alpha = 0f;
    }

    public void Transition(System.Action onReady) {
        _fadeCanvasGroup.DOFade(1f, 1f / _fadeSpeed).OnComplete(() => TransitionBack(onReady));
    }

    private void TransitionBack(System.Action onReady) {
        onReady?.Invoke();
        _fadeCanvasGroup.DOFade(0f, 1f / _fadeSpeed);
    }

    [ContextMenu("Show New Sky Message")]
    public void ShowNewSkyMessageNotification() {
        Vector3 scale = _skyMessageOutline.rectTransform.localScale;
        _skyMessageStars.sprite = _skyMessageMailStars;
        _skyMessageMail.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_skyMessageOutline.rectTransform.DOScale(scale * 1.1f, 0.5f))
            .Append(_skyMessageOutline.rectTransform.DOLocalRotate(Vector3.zero, 0.5f))
            .Join(_skyMessageMail.rectTransform.DOScale(Vector3.one, 0.5f))
            .Append(_skyMessageOutline.rectTransform.DOScale(scale, 0.5f));
    }

    [ContextMenu("Hide Sky Message")]
    public void ClearSkyMessageNotification() {
        Vector3 scale = _skyMessageOutline.rectTransform.localScale;
        _skyMessageStars.sprite = _skyMessageNormalStars;
        Sequence seq = DOTween.Sequence();
        seq.Append(_skyMessageOutline.rectTransform.DOScale(scale * 1.1f, 0.5f))
            .Append(_skyMessageOutline.rectTransform.DOLocalRotate(Vector3.forward * -90f, 0.5f))
            .Join(_skyMessageMail.rectTransform.DOScale(Vector3.zero, 0.5f))
            .Append(_skyMessageOutline.rectTransform.DOScale(scale, 0.5f));
    }
}
