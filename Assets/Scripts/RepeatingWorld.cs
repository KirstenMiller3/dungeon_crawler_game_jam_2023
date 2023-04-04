using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepeatingWorld : MonoBehaviour {
    [SerializeField] private Transform _corridorStart;
    [SerializeField] private Transform _pickUpLocation;
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private GameObject[] _rocks;

    [SerializeField] private Collider[] _triggers;
    [SerializeField] private string[] _quote;

    public Vector3 StartPosition => _corridorStart.position;
    
    private bool _triggerIndex;
    private int _textIndex;

    public Observable<int> Loops = new Observable<int>();

    public void Init() {
        SetTriggers();
        SetText();
        for(int i = 0; i < _rocks.Length; i++) {
            _rocks[i].SetActive(false);
        }
    }

    public PickUpObj ShowPickUp(PickUpObj pickUpObj) {
        return Instantiate(pickUpObj, _pickUpLocation.position, Quaternion.identity);
    }

    public void Complete() {
        _triggers[0].enabled = false;
        _triggers[1].enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.tag != "Player") {
            return;
        }

        _triggerIndex = !_triggerIndex;
        SetTriggers();

        _textIndex++;
        SetText();

        SetRock();

        if(!_triggerIndex) {
            Loops.Value++;
        }
    }

    private void SetRock() {
        for(int i = 0; i < _rocks.Length; i++) {
            _rocks[i].SetActive(false);
        }

        if(Loops.Value > _rocks.Length - 1) {
            return;
        }

        _rocks[Loops.Value].SetActive(true);
    }

    private void SetText() {
        if(_textIndex > _quote.Length - 1) {
            return;
        }
        _text1.text = _quote[_textIndex];
        _text2.text = _quote[_textIndex];
    }

    private void SetTriggers() {
        _triggers[0].enabled = _triggerIndex;
        _triggers[1].enabled = !_triggerIndex;
    }
}
