using UnityEngine;
using System.Collections.Generic;

public class RopeManager : FiveSingleton<RopeManager>
{
    [SerializeField]
    private List<Rope> _ropes = new List<Rope>();
    public List<Rope> ropes => _ropes;
    [SerializeField] private List<GameObject> ropePref;
    [SerializeField] private GameObject solver;


    public void InstantiateNewRope(int numberRope)
    {
        foreach (var item in ropes)
        {
             Destroy(item, 2);
        }
        ropes.Clear();
       
        for (int i = 0; i < numberRope; i++)
        {
            ropes.Add(Instantiate(ropePref[i],solver.transform).GetComponent<Rope>());
          
           ropes[i].transform.position = new Vector3(-0.75f + 0.4f*i, 0, 2.11f);
            
            
        }
    }

    public void AddRope()
    {
        ropes.Add(Instantiate(ropePref[ropes.Count], solver.transform).GetComponent<Rope>());
        ropes[ropes.Count-1].transform.position = new Vector3(-0.75f + 0.4f * (ropes.Count - 1), 0, 2.11f);
        TangleMasterGame.instance.ResetOriginRopes();
    }
    public void RemoveRope()
    {
        foreach (var item in ropes)
        {
            item.curPlugPlace.curRodPlugger = null;
            item.curPlugPlace.gameObject.SetActive(true);
            //    item.ResetRopeLength();
        }
        Destroy(ropes[ropes.Count - 1].gameObject);
        ropes.RemoveAt(ropes.Count - 1);
        TangleMasterGame.instance.ResetOriginRopes();

    }
}
