using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightOptimiser : MonoBehaviour {
    private Light _light;

    private void Awake() {
        _light = GetComponent<Light>();
    }


    private void Update() {
        float distToPlayer = Vector3.Distance(transform.position, PlayerManager.Instance.PlayerTransform.position);
        if(distToPlayer > 20) {
            if(_light.isActiveAndEnabled) {
                _light.enabled = false;
            }
        }
        else {
            if(!_light.isActiveAndEnabled) {
                _light.enabled = true;
            }
        }
    }
}
