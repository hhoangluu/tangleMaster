﻿using UnityEngine;

public class RodCheckBoxCollider2D : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _bc2D;
    public BoxCollider2D bc2D => _bc2D;

    private Rod _hostRope;
    public Rod hostRope
    {
        get => _hostRope;
        set => _hostRope = value;
    }
}
