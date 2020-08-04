using UnityEngine;
using System.Collections.Generic;

public class RodsManager : FiveSingleton<RodsManager>
{
    [SerializeField]
    private List<Rod> _rods = new List<Rod>();
    public List<Rod> rods => _rods;
}
