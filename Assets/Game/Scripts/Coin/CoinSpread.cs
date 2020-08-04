using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CoinSpread : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTransform;
    public RectTransform rectTransform => _rectTransform;
    [SerializeField]
    private Image _coinImage;
    public Image coinImage => _coinImage;

    private Coroutine lerpCorou;

    private bool _isDone;
    public bool isDone => _isDone;

    public void LerpTo(Vector3 tarPos, float timeMul, Action onDone = null)
    {
        if (lerpCorou != null) StopCoroutine(lerpCorou);
        lerpCorou = StartCoroutine(LerpToIE(tarPos, timeMul, onDone));
    }

    public void LerpToGoldIcon(Transform goldIcon, float timeMul, Action onDone = null)
    {
        if (lerpCorou != null) StopCoroutine(lerpCorou);
        lerpCorou = StartCoroutine(LerpToGoldIconIE(goldIcon, timeMul, onDone));
    }

    private IEnumerator LerpToIE(Vector3 tarPos, float timeMul, Action onDone = null)
    {
        _isDone = false;
        Vector3 curPos = transform.position;
        float t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(curPos, tarPos, t);
            t += Time.deltaTime * timeMul;
            yield return GameManager.WaitForEndOfFrame;
        }
        transform.position = tarPos;
        _isDone = true;
        onDone?.Invoke();
        yield break;
    }

    private IEnumerator LerpToGoldIconIE(Transform goldIcon, float timeMul, Action onDone = null)
    {
        _isDone = false;
        Vector3 curPos = transform.position;
        float t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(curPos, Camera.main.ScreenToWorldPoint(goldIcon.position), t);
            t += Time.deltaTime * timeMul;
            yield return GameManager.WaitForEndOfFrame;
        }
        transform.position = Camera.main.ScreenToWorldPoint(goldIcon.position);
        _isDone = true;
        onDone?.Invoke();
        yield break;
    }
}
