using UnityEngine;
using System.Collections.Generic;

public class RopeManager : FiveSingleton<RopeManager>
{
    [SerializeField]
    private List<Rope> _ropes = new List<Rope>();
    public List<Rope> rods => _ropes;
}
