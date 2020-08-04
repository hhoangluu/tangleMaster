using UnityEngine;
using System.Collections.Generic;

public class PlugPlacesManager : FiveSingleton<PlugPlacesManager>
{
    [SerializeField]
    private List<PlugPlace> _plugPlaces = new List<PlugPlace>();
    public List<PlugPlace> plugPlaces => _plugPlaces;

    public int IndexOfPlugPlace(PlugPlace pPlace)
    {
        return plugPlaces.IndexOf(pPlace);
    }
}
