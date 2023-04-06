using Milo.Tools;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField] protected MirroredPerson _person;
    [SerializeField] private bool _isInteractable;
    [SerializeField] private int _numberOfSouls = 3;
    [SerializeField] private float _soulSpeed = 3f;

    [TextArea][SerializeField] private string _hintQuote;

    [SerializeField] private Transform _badMirror;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _material;
   
    private MirroredPlayer _mirrorPerson;
    protected bool _fightCompleted;
    protected bool _hasStarted;

    public MirroredPerson MirroredPerson => _person;
    public bool IsInteractable => _isInteractable;
    public bool HasStarted => _hasStarted;
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
            _mirrorPerson.ShowMirroredPerson(MirroredPerson.None);
        }
        else {
            _mirrorPerson.ShowMirroredPerson(_person);
        }

    }

    public virtual void StartFight() {
        if(PlayerManager.Instance.ActiveMirror != null && PlayerManager.Instance.ActiveMirror != this) {
            PlayerManager.Instance.ActiveMirror.CancelPuzzle();
        }

        
        PlayerManager.Instance.SetActiveMirror(this);

        _hasStarted = true;

        if(_fightCompleted) {
            OnPressStartPuzzle();
        }
        else {
            FightController.Instance.StartFight(_numberOfSouls, _soulSpeed);
            FightController.Instance.OnComplete = OnPressStartPuzzle;
        }
    }

    public virtual void OnPressStartPuzzle() {
        _fightCompleted = true;

        switch(_person) {
            case MirroredPerson.Calm:
                AudioManager.instance.Play("fire");
                break;
            case MirroredPerson.Health:
                AudioManager.instance.Play("health");
                break;
            case MirroredPerson.Peace:
                AudioManager.instance.Play("peace");
                break;
            case MirroredPerson.Acceptance:
                AudioManager.instance.Play("perfection");
                break;
            case MirroredPerson.Presence:
                AudioManager.instance.Play("presence");
                break;
            case MirroredPerson.Restore:
                AudioManager.instance.Play("restore");
                break;
            case MirroredPerson.Time:
                AudioManager.instance.Play("time");
                break;
        }

        SkyText.Instance.SetText(_hintQuote);
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

        if(_isInteractable) {
            UIManager.Instance.ShowMirrorInteractPrompt(false);
        }

        IsComplete.Value = true;
        _mirrorPerson.TransformPlayer();
        //PlayerManager.Instance.AddCondition(1);
        PlayerManager.Instance.CompleteMirror();
    }
}

