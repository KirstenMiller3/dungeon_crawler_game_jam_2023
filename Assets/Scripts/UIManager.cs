using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _conditionMeter;
    [SerializeField] private TextMeshProUGUI _pickUpText;
    [SerializeField] private GameObject _giveText;
    [SerializeField] private GameObject _dropText;

    [SerializeField] private PickUpDetection _pickUpDetection;
    [SerializeField] private PlayerMirrorDetection _mirrorDetection;

    private string _pickUpBase;
    private bool _onMirror;

    public void Start() {
        _pickUpBase = _pickUpText.text;

        _pickUpDetection = FindObjectOfType<PickUpDetection>();
        _mirrorDetection = FindObjectOfType<PlayerMirrorDetection>();

        PlayerManager.Instance.ConditionLevel.Subscribe(OnUpdateCondition, true);
        _pickUpDetection.LookAtObjName.Subscribe(OnLookAtObjName, true);
        _pickUpDetection.LookingAtObj.Subscribe(OnLookAtObj, true);
        _mirrorDetection.LookingAtMirror.Subscribe(OnMirror, true);
    }

    public void OnDisable() {
        PlayerManager.Instance.ConditionLevel.Unsubscribe(OnUpdateCondition);
        _pickUpDetection.LookAtObjName.Unsubscribe(OnLookAtObjName);
        _pickUpDetection.LookingAtObj.Unsubscribe(OnLookAtObj);
        _mirrorDetection.LookingAtMirror.Unsubscribe(OnMirror);
    }

    private void Update() {
        if(!_pickUpDetection.HasHeldObj) {
            _giveText.gameObject.SetActive(false);
            _dropText.gameObject.SetActive(false);
        } 
        else {
            _giveText.gameObject.SetActive(_onMirror);
            _dropText.gameObject.SetActive(!_onMirror);
        }
    }

    private void OnUpdateCondition(int prev, int curr) {
        _conditionMeter.text = $"{curr}%";
    }

    private void OnLookAtObj(bool prev, bool curr) {
        _pickUpText.gameObject.SetActive(curr);
    }

    private void OnMirror(bool prev, bool curr) {
        _onMirror = curr;
    }

    private void OnLookAtObjName(string prev, string curr) {
        _pickUpText.text = string.Format(_pickUpBase, curr);
    }
}
