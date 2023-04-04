using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcceptanceMessage : PickUpObj {

    public enum AcceptanceType {
        Normal,
        Weak,
        Slow,
        Clumsy,
        Reset
    }

    [SerializeField] private AcceptanceType _atype;

    public AcceptanceType Type => _atype;

    protected override void Start() {
        _dontAddToHand = true;
    }
}
