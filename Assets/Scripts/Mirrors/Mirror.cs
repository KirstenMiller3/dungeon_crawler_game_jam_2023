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

    private RenderTexture rt;

    protected virtual void Start() {
        _mirrorPerson = FindObjectOfType<MirroredPlayer>();

        rt = new RenderTexture((int)transform.localScale.x * 400, (int)transform.localScale.y * 400, 16, RenderTextureFormat.DefaultHDR);
        rt.Create();
        rt.name = "Mirror_RenderTexture";
        Material newMat = new Material(_material);
        newMat.mainTexture = rt;

        GetComponent<Renderer>().material = newMat;

        _camera.targetTexture = rt;

        _badMirror.transform.position = transform.position + _mirrorPerson.Offset;

        IsComplete.Value = false;

        _camera.enabled = false;
    }

    protected virtual void Update() {
        //if(PlayerManager.Instance == null) {
        //    return;
        //}

        //float dist = Vector3.Distance(PlayerManager.Instance.PlayerTransform.position, transform.position);

        //_camera.enabled = dist < 10f;
    }

    public void ShowPerson() {
        _camera.enabled = true;

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

        _camera.enabled = true;
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

    public void Hide() {
        _camera.enabled = false;
        rt.Release();
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

        //PlayerManager.Instance.AddCondition(1);
        PlayerManager.Instance.CompleteMirror();


        _mirrorPerson.TransformPlayer();
        if(PlayerManager.Instance.GameFinished) {
            PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().DisableMovement(true);
            _mirrorPerson.OnCompleteTransform = OnTransformCompleteEndGame;
        }
    }

    private void OnTransformCompleteEndGame() {
        PlayerManager.Instance.EndGame();
    }
}

