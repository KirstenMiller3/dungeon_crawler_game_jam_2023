using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] private MirroredPerson _person;

    [TextArea][SerializeField] private string _hintQuote;

    [SerializeField] private Transform _badMirror;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;
   
    private MirroredPlayer _mirrorPerson;

    public MirroredPerson MirroredPerson => _person;

    public Observable<bool> IsComplete = new Observable<bool>();

    private void Start() {
        _mirrorPerson = FindObjectOfType<MirroredPlayer>();

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

    public virtual void Interact() {
        if(IsComplete.Value) {
            return;
        }

        _mirrorPerson.ShowMirroredPerson(_person);

        SkyText.Instance.SetText(_hintQuote);
    }

    public virtual void StopInteract() {
        if(IsComplete.Value) {
            return;
        }
    }


    [ContextMenu("Complete")]
    public void CompleteMirror() {
        IsComplete.Value = true;
        _mirrorPerson.TransformPlayer();
    }
}

