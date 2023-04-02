using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] private MirroredPerson _person;
    [SerializeField] private Transform _badMirror;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;
    [SerializeField] private MirroredPlayer _mirrorPerson;

    public MirroredPerson MirroredPerson => _person;
    Transform _mirrorPlayer;

    public Observable<bool> IsComplete = new Observable<bool>();

    private void Start() {
        _mirrorPlayer = GameObject.Find("Mirror_Player").transform;

        RenderTexture rt = new RenderTexture((int)transform.localScale.x * 400, (int)transform.localScale.y * 400, 16, RenderTextureFormat.DefaultHDR);
        rt.Create();
        rt.name = "Mirror_RenderTexture";
        Material newMat = new Material(_material);
        newMat.mainTexture = rt;

        GetComponent<Renderer>().material = newMat;

        _camera.targetTexture = rt;

        _badMirror.transform.position = transform.position + _offset;

        IsComplete.Value = false;
    }

    public void Interact() {
        if(IsComplete.Value) {
            return;
        }

        _mirrorPerson.ShowMirroredPerson(_person);
    }


    [ContextMenu("Complete")]
    public void CompleteMirror() {
        IsComplete.Value = true;
        _mirrorPerson.TransformPlayer();
    }
}

