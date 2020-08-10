using UnityEngine;
using System.Collections;

public class MenuTheme : MenuAbs<MenuTheme>
{
    [SerializeField]
    private GameObject _curtainGO;

    public Material[] _materialRope;

    public override void Open()
    {
        _curtainGO.SetActive(true);
        components.SetActive(true);
    }

    public void HandleSelectMaterial_Click(int index)
    {
        DMCGameUtilities.MaterialRopeCurrent = index;
    }

    public void HandleBtnClose_Click()
    {
        _curtainGO.SetActive(false);
        components.SetActive(false);
    }
}
