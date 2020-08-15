using UnityEngine;
using System;
using System.Collections;
using Obi;
using System.Collections.Generic;
using DG.Tweening;

public enum RopeState
{
    plugged,
    unplugged,
}

public class Rope : MonoBehaviour
{
    [SerializeField]
    private ObiRope _obiRope;
    public ObiRope obiRope => _obiRope;

    [SerializeField]
    private RodPlugger _rodPlugger;
    public RodPlugger rodPlugger => _rodPlugger;

    
    //[SerializeField]
    //private ObiRopeExtrudedRenderer _obiRopeExtrudedRenderer;

    [SerializeField]
    private RodChecker _rodChecker;
    public RodChecker rodChecker => _rodChecker;

    public IEnumerable<Mesh> ParticleMeshes => _rodChecker.ParticleMeshes;

  

    public List<Mesh> listMeshes
    {
        get => _rodChecker.listMeshes;
        set => _rodChecker.listMeshes = value;
    }

    private PlugPlace _curPlugPlace;
    public PlugPlace curPlugPlace
    {
        get => _curPlugPlace;
        set => _curPlugPlace = value;
    }

    private RopeState _curRodState;
    public RopeState curRodState
    {
        get => _curRodState;
        //set => _curRopeState = value;
    }

    private bool _isPluggerBusy;
    public bool isPluggerBusy => _isPluggerBusy;

    private Coroutine _ropePluggerCorou;

    private bool _isFree;
    public bool isFree => _isFree;

    [SerializeField] private RopeLengthController ropeLengthController;
    [SerializeField] private ParticleSystem tangleParticleEffectKnut;
    [SerializeField] private ParticleSystem tangleParticleEffectPlugger;
    [SerializeField] private MeshRenderer knut;
    [SerializeField] private NutController nut;

    //  [SerializeField] private GameObject ropePref;



    private Coroutine _freeCorou;

 
    public void SetPluggedAbs()
    {
        rodPlugger.transform.position = curPlugPlace.pluggedPlace.position;
        _curRodState = RopeState.plugged;
        _curPlugPlace.SetPlugged();
    }

    public void SetPlugged()
    {
        if (_ropePluggerCorou != null) StopCoroutine(_ropePluggerCorou);
        _ropePluggerCorou = StartCoroutine(LerpTo(rodPlugger.transform, curPlugPlace.pluggedPlace.position, 0.25f,
            onStart: () =>
            {
                _isPluggerBusy = true;
                _curRodState = RopeState.plugged;
            },
            onDone: () =>
            {
                _isPluggerBusy = false;
                _curPlugPlace.SetPlugged();
            }
            ));
    }

    public void SetUnPlugged()
    {
        if (_ropePluggerCorou != null) StopCoroutine(_ropePluggerCorou);
        _ropePluggerCorou = StartCoroutine(LerpTo(rodPlugger.transform, curPlugPlace.unplugPlace.position, 0.25f,
            onStart: () =>
            {
                _isPluggerBusy = true;
                _curRodState = RopeState.unplugged;
                _curPlugPlace.SetUnPlugged();
            },
            onDone: () =>
            {
                _isPluggerBusy = false;
            }
            ));
    }

    public void Reset()
    {
        _curPlugPlace = null;
        //_ropParent.localScale = Vector3.one;
        _curRodState = RopeState.unplugged;
        _isPluggerBusy = false;
        _isFree = false;
    }

    public void IsFree()
    {
        TangleMasterGame.instance.IsFree(this);
    }

    public void SetFree(Action onDone)
    {
        _isFree = true;
        rodChecker.isCheckFree = false;
        if (_freeCorou != null) StopCoroutine(_freeCorou);
        _freeCorou = StartCoroutine(SetFreeIE(onDone));
    }

    public void OnNextLevel()
    {
      //  ropeLengthController.ResetLength();
      
        knut.enabled = true;
        rodPlugger.GetComponent<MeshRenderer>().enabled = true;
        rodChecker.StartCheck();

    }
    private IEnumerator SetFreeIE(Action onDone)
    {
        while (_isPluggerBusy) yield return GameManager.WaitForEndOfFrame;
        if (_ropePluggerCorou != null) StopCoroutine(_ropePluggerCorou);
        //_ropParent.DOScaleY(0, 0.9f);
        GameController.instance.AddConfetti(transform.position + new Vector3(0, 1.8f, 0));
        FreeEffect();
          yield return new WaitWhile(() => obiRope.restLength > 0f);
        Debug.Log("@LOG SetFreeIE");
      
    //   yield return new WaitForSeconds(1f);
        _curPlugPlace.curRodPlugger = null;
        onDone?.Invoke();
         gameObject.SetActive(false);
     //   Destroy(gameObject,2);
       
    }

    private void FreeEffect()
    {
       ropeLengthController.ShortenRope();
        nut.TurnNut();
        _curPlugPlace.SetUnPlugged();
        tangleParticleEffectKnut.Play();
        tangleParticleEffectPlugger.Play();
        knut.enabled = false;
        rodPlugger.GetComponent<MeshRenderer>().enabled = false;
        
    }

    public void ResetRopeLength()
    {
        gameObject.SetActive(true);
        ropeLengthController.ResetLength();
    }

    private IEnumerator LerpTo(Transform tarTF, Vector3 tarPos, float inTime, Action onStart, Action onDone = null)
    {
        onStart?.Invoke();
        Vector3 curPos = tarTF.transform.position;
        float t = 0f;
        while (t < 1f)
        {
            tarTF.transform.position = Vector3.Lerp(curPos, tarPos, t);
            t += Time.deltaTime * (1 / inTime);
            yield return GameManager.WaitForEndOfFrame;
        }
        tarTF.transform.position = tarPos;
        onDone?.Invoke();
        yield break;
    }

    private IEnumerator DelayDoIE(float delay, Action toDo)
    {
        yield return new WaitForSeconds(delay);
        toDo?.Invoke();
    }
}
