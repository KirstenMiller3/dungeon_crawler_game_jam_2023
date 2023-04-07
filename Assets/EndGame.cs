using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _target;
    [SerializeField] private AnimationCurve _lightCurve;
    [SerializeField] private AnimationCurve _skyCurve;
    [SerializeField] private Light _light;
    [SerializeField] private Vector2 _lightRange;
    [SerializeField] private Material _material;
    [SerializeField] private Vector2 _skyExposure;

    private float _margin;

    private void Start() {
        _margin = Vector3.Distance(_spawn.position, _target.position);
    }

    public void Update() {
        if(!PlayerManager.Instance.GameFinished) {
            return;
        }

        float dist = Vector3.Distance(PlayerManager.Instance.PlayerTransform.position, _target.position);

        float calc = (_margin - dist) / _margin;

        _light.intensity = Mathf.Lerp(_lightRange.x, _lightRange.y, _lightCurve.Evaluate(calc));

        _material.SetFloat("_Exposure", Mathf.Lerp(_skyExposure.x, _skyExposure.y, _skyCurve.Evaluate(calc)));
    }

    private void OnDisable() {
        _material.SetFloat("_Exposure", 1);
    }
}
