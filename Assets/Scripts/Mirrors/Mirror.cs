using Milo.Tools;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] private MirroredPerson _person;

    [TextArea][SerializeField] private string _hintQuote;

    [SerializeField] private Transform _badMirror;
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

        _badMirror.transform.position = transform.position + _mirrorPerson.Offset;

        IsComplete.Value = false;
    }

    public void ShowPerson() {
        if(IsComplete.Value) {
            return;
        }

        _mirrorPerson.ShowMirroredPerson(_person);
        SkyText.Instance.SetText(_hintQuote);
    }

    public virtual void Interact() {
        if(IsComplete.Value) {
            return;
        }
    }

    public virtual void StopInteract() {
        if(IsComplete.Value) {
            return;
        }
    }


    [ContextMenu("Complete")]
    public void CompleteMirror() {
        if(IsComplete.Value) {
            return;
        }

        IsComplete.Value = true;
        _mirrorPerson.TransformPlayer();
        PlayerManager.Instance.AddCondition(50);
        PlayerManager.Instance.CompleteMirror();
    }
}

