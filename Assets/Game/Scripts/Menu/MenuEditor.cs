using UnityEngine;
using UnityEngine.UI;

public class MenuEditor : MenuAbs<MenuEditor>
{
    [SerializeField]
    private GameObject _bg;
    [SerializeField]
    private InputField _levelDirectoryInput;

    public override void Open()
    {
        base.Open();
        _bg.gameObject.SetActive(true);
        _levelDirectoryInput.gameObject.SetActive(true);
        TangleMasterGame.instance.StartGame();
    }

    public override void Close()
    {
        base.Close();
        _bg.gameObject.SetActive(false);
        _levelDirectoryInput.gameObject.SetActive(false);
        MenuMain.instance.Open();
    }

    public void OnValueChanged(string numString)
    {
        if (!isDigits(numString))
        {
            _levelDirectoryInput.text = _levelDirectoryInput.text.Remove(_levelDirectoryInput.text.Length - 1);
        }
    }

    public void SaveCurMeshState()
    {
        LevelsManager.instance.SaveCurMeshState(int.Parse(_levelDirectoryInput.text));
    }

    private bool isDigits(string s)
    {
        if (s == null || s == "") return false;

        for (int i = 0; i < s.Length; i++)
            if ((s[i] ^ '0') > 9)
                return false;

        return true;
    }
}
