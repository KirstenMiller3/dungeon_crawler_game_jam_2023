using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExampleObservableUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _numberText;

    public void OnEnable() {
        ExampleData.Instance._test.Subscribe(OnUpdateInt, true);
    }

    public void OnDisable() {
        ExampleData.Instance._test.Unsubscribe(OnUpdateInt);
    }

    private void OnUpdateInt(int prev, int cur) {
        _numberText.text = cur.ToString();
    }
}
