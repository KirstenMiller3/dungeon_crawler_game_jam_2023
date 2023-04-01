using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _conditionMeter;

    public void Start() {
        PlayerManager.Instance.ConditionLevel.Subscribe(OnUpdateCondition, true);
    }

    public void OnDisable() {
        PlayerManager.Instance.ConditionLevel.Unsubscribe(OnUpdateCondition);
    }

    private void OnUpdateCondition(int prev, int curr) {
        _conditionMeter.text = $"{curr}%";
    }
}
