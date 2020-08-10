using DG.Tweening;
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

    [SerializeField]
    private GameObject _confettiPrefab;

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

    public void AddConfetti(Vector3 pos)
    {
        var confetti = GameObject.Instantiate(_confettiPrefab);
        confetti.SetActive(true);
        confetti.transform.localScale = Vector3.one * 2;
        confetti.transform.position = pos;
        confetti.GetComponent<ParticleSystem>().Play();
        DOVirtual.DelayedCall(4.5f, delegate 
        {
            GameObject.DestroyImmediate(confetti);
        });
    }
}
