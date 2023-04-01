using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] private Transform _badMirror;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;
    [SerializeField] private float _val1 = 100f;
    [SerializeField] private float _val2 = 5f;

    Transform _mirrorPlayer;

    private void Start() {
        _mirrorPlayer = GameObject.Find("Mirror_Player").transform;

        RenderTexture rt = new RenderTexture((int)transform.localScale.x * 100, (int)transform.localScale.y * 100, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        rt.name = "Mirror_RenderTexture";
        Material newMat = new Material(_material);
        newMat.mainTexture = rt;

        GetComponent<Renderer>().material = newMat;

        _camera.targetTexture = rt;

        _badMirror.transform.position = transform.position + _offset;
    }

    private void Update() {
        Vector3 dir = _badMirror.position - _mirrorPlayer.position;
        float dist = Vector3.Distance(_badMirror.position, _mirrorPlayer.position);
        Vector3 pos = _badMirror.position + (dir.normalized * dist);
        //_camera.transform.position = new Vector3(pos.x, _camera.transform.position.y, pos.z);
        //_camera.fieldOfView = Mathf.Clamp(_val1 - (dist * _val2), 0, 100);
    }
}

