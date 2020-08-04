using UnityEngine;

public class PlugPlace : MonoBehaviour, IClickable
{
    [SerializeField]
    private Transform _pluggedPlace;
    public Transform pluggedPlace => _pluggedPlace;

    [SerializeField]
    private Transform _unplugPlace;
    public Transform unplugPlace => _unplugPlace;

    private RodPlugger _curRodPlugger;
    public RodPlugger curRodPlugger
    {
        get => _curRodPlugger;
        set => _curRodPlugger = value;
    }

    public void SetPlugged()
    {
        gameObject.SetActive(false);
    }

    public void SetUnPlugged()
    {
        gameObject.SetActive(true);
    }

    public void OnClicked()
    {
        //FiveDebug.Log("OnClicked-PlugPlace: " + name);
        TangleMasterGame.instance.OnPlugSlotClicked(this);
    }
}
