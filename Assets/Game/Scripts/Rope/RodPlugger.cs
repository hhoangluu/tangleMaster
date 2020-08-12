using UnityEngine;

public class RodPlugger : MonoBehaviour, IClickable
{
    [SerializeField]
    private Rope _rodHost;
    public Rope rodHost => _rodHost;

    public void OnClicked()
    {
        //FiveDebug.Log("OnClicked-RodPlugger: " + name);
        TangleMasterGame.instance.OnRodPluggerClicked(this);
    }
}
