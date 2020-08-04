using UnityEngine;

public class RodPlugger : MonoBehaviour, IClickable
{
    [SerializeField]
    private Rod _rodHost;
    public Rod rodHost => _rodHost;

    public void OnClicked()
    {
        //FiveDebug.Log("OnClicked-RodPlugger: " + name);
        TangleMasterGame.instance.OnRodPluggerClicked(this);
    }
}
