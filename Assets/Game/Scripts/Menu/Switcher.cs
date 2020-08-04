using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    private GameObject _onGobj;
    [SerializeField]
    private GameObject _offGobj;
    [SerializeField]
    private Image _fader;
    [SerializeField]
    private Animator _faderAnim;
    [SerializeField]
    private RectTransform _swicherRT;
    [SerializeField]
    private Vector2 _swicherOnPos;
    [SerializeField]
    private Vector2 _swicherOffPos;

    private bool _curState;
    public bool curState => _curState;

    private bool _isBusy;
    private Coroutine _switchCorou;

    public void TurnSwitch(bool switchState, float inTime, Action onDone)
    {
        if (_isBusy) return;
        if (_switchCorou != null) StopCoroutine(_switchCorou);
        _switchCorou = StartCoroutine(SwitchIE(switchState, inTime, onDone));
        _faderAnim.speed = 1 / inTime;
        _faderAnim.SetTrigger("SwitchToggle");
    }

    private IEnumerator SwitchIE(bool switchState, float inTime, Action onDone)
    {
        _curState = switchState;
        _onGobj.gameObject.SetActive(!switchState);
        _offGobj.gameObject.SetActive(switchState);

        Vector2 curSwitchPos = _swicherRT.transform.position;
        Vector2 tarSwitchPos = switchState ? _swicherOnPos : _swicherOffPos;

        float t = 0f;
        while (t < 1f)
        {
            _swicherRT.anchoredPosition = Vector2.Lerp(curSwitchPos, tarSwitchPos, t);
            t += Time.deltaTime * (1 / inTime);
            yield return GameManager.WaitForEndOfFrame;
        }
        _swicherRT.anchoredPosition = tarSwitchPos;
        onDone?.Invoke();
        yield break;
    }

    public void OnMid()
    {
        _onGobj.gameObject.SetActive(_curState);
        _offGobj.gameObject.SetActive(!_curState);
    }
}
