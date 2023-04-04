using Milo.Tools;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] protected MirroredPerson _person;
    [SerializeField] private bool _isInteractable;

    [TextArea][SerializeField] private string _hintQuote;

    [SerializeField] private Transform _badMirror;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;
   
    private MirroredPlayer _mirrorPerson;
    private bool _hasStarted;

    public MirroredPerson MirroredPerson => _person;
    public bool IsInteractable => _isInteractable;

    public Observable<bool> IsComplete = new Observable<bool>();

    protected virtual void Start() {
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

    public virtual void OnPressStartPuzzle() {
        if(PlayerManager.Instance.ActiveMirror != null && PlayerManager.Instance.ActiveMirror != this) {
            PlayerManager.Instance.ActiveMirror.CancelPuzzle();
        }

        PlayerManager.Instance.SetActiveMirror(this);

        _hasStarted = true;
    }

    public virtual void Interact() {
        if(IsComplete.Value) {
            return;
        }

        if(_isInteractable) {
            UIManager.Instance.ShowMirrorInteractPrompt(!_hasStarted);
        }
    }

    public virtual void StopInteract() {
        if(IsComplete.Value) {
            return;
        }

        UIManager.Instance.ShowMirrorInteractPrompt(false);
    }

    public virtual void CancelPuzzle() {
        _hasStarted = false;
    }


    public virtual void CompleteMirror() {
        if(IsComplete.Value) {
            return;
        }

        IsComplete.Value = true;
        _mirrorPerson.TransformPlayer();
        PlayerManager.Instance.AddCondition(1);
        PlayerManager.Instance.CompleteMirror();
    }
}

