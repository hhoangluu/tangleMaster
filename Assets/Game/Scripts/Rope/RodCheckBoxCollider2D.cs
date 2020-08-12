using UnityEngine;

public class RodCheckBoxCollider2D : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _bc2D;
    public BoxCollider2D bc2D => _bc2D;

    private Rope _hostRope;
    public Rope hostRope
    {
        get => _hostRope;
        set => _hostRope = value;
    }
}
