using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentMirror : Mirror {
    [SerializeField] private PickUpType _solution;

    public PickUpType Solution => _solution;
}
