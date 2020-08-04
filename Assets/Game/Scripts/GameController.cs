using UnityEngine;

public interface IClickable
{
    void OnClicked();
}

public class GameController : FiveSingleton<GameController>
{
    private bool _isControllable;
    public bool isControllable
    {
        get { return _isControllable; }
        set { _isControllable = value; }
    }

    private Ray _ray;
    private RaycastHit _hit;

    private void Update()
    {
        if (isControllable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit))
                {
                    _hit.collider.gameObject.GetComponent<IClickable>()?.OnClicked();
                }
            }
        }
    }
}
