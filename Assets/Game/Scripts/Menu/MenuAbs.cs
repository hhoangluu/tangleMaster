using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public interface IMenu
{
    bool isOpened { get; }
    //bool isClickable { get; }

    void Open();
    void Close();
}

public abstract class MenuAbs<T> : FiveSingleton<T>, IMenu
{
    [SerializeField]
    protected GameObject components;

    protected bool _isOpened;
    public bool isOpened { get { return _isOpened; } }

    //protected bool _isClickable;
    //public bool isClickable { get { return _isClickable; } }

    public virtual void Open()
    {
        _isOpened = true;
        //_isClickable = true;
        components.SetActive(true);
    }

    public virtual void Close()
    {
        _isOpened = false;
        //_isClickable = false;
        components.SetActive(false);
    }

    protected void ColorLerp(ref Coroutine corou, Image image, Color startColor, Color tarColor, float inTime, Action onStart = null, Action onDone = null)
    {
        if (corou != null) StopCoroutine(corou);
        corou = StartCoroutine(ColorLerpIE(image, startColor, tarColor, inTime, onStart, onDone));
    }

    private IEnumerator ColorLerpIE(Image image, Color startColor, Color tarColor, float inTime, Action onStart = null, Action onDone = null)
    {
        image.color = startColor;
        onStart?.Invoke();
        float t = 0f;
        while (t < 1f)
        {
            image.color = Color.Lerp(startColor, tarColor, t);
            t += Time.deltaTime * (1 / inTime);
            yield return GameManager.WaitForEndOfFrame;
        }
        image.color = tarColor;
        onDone?.Invoke();
        yield break;
    }
}
