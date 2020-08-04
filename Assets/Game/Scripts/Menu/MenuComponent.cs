using UnityEngine;
using System;
using System.Collections;

public interface IDynamicMenu
{
    void SetAsClosed();
    void Open(float inTime);
    void Close(float inTime);
    void Sub(float inTime);
}

public enum MenuState
{
    none,
    close,
    open,
    sub,
}

public class MenuComponent : MonoBehaviour, IDynamicMenu
{
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Vector2 _openedPos;
    [SerializeField]
    private Vector2 _closedPos;
    [SerializeField]
    private Vector2 _subPos;

    private bool _isLocked;
    public bool isLocked
    {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

    private Coroutine menuCorou;

    private MenuState _curMenuState;
    public MenuState curMenuState => _curMenuState;

    public void SetAsClosed()
    {
        _rectTransform.anchoredPosition = _closedPos;
    }

    public void Open(float inTime)
    {
        if (isLocked) return;
        _curMenuState = MenuState.open;
        if (menuCorou != null) StopCoroutine(menuCorou);
        if (gameObject.activeInHierarchy)
            menuCorou = StartCoroutine(LerpToIE(_openedPos, inTime));
    }

    public void Close(float inTime)
    {
        if (isLocked) return;
        _curMenuState = MenuState.close;
        if (menuCorou != null) StopCoroutine(menuCorou);
        if (gameObject.activeInHierarchy)
            menuCorou = StartCoroutine(LerpToIE(_closedPos, inTime));
    }

    public void Sub(float inTime)
    {
        if (isLocked) return;
        _curMenuState = MenuState.sub;
        if (menuCorou != null) StopCoroutine(menuCorou);
        if (gameObject.activeInHierarchy)
            menuCorou = StartCoroutine(LerpToIE(_subPos, inTime));
    }

    private IEnumerator LerpToIE(Vector2 tarAnchoredPos, float inTime, Action onLerpStart = null, Action onLerpDone = null)
    {
        onLerpStart?.Invoke();
        Vector2 curAnchoredPos = _rectTransform.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(curAnchoredPos, tarAnchoredPos, t);
            t += Time.deltaTime * (1 / inTime);
            yield return GameManager.WaitForEndOfFrame;
        }
        _rectTransform.anchoredPosition = tarAnchoredPos;
        onLerpDone?.Invoke();
    }
}
