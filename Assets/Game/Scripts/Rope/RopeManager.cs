using UnityEngine;
using System.Collections.Generic;

public class RopeManager : FiveSingleton<RopeManager>
{
    [SerializeField]
    private List<Rope> _ropes = new List<Rope>();
    public List<Rope> ropes => _ropes;
    [SerializeField] private List<GameObject> ropePref;
    [SerializeField] private GameObject solver;


    public void InstantiateNewRope()
    {
        foreach (var item in ropes)
        {
             Destroy(item, 2);
        //    item.ResetRopeLength();
        }
        ropes.Clear();
       
        for (int i = 0; i < ropePref.Count; i++)
        {
            ropes.Add(Instantiate(ropePref[i],solver.transform).GetComponent<Rope>());
          //  ropes[i] = Instantiate(ropePref[i], solver.transform).GetComponent<Rope>();
         //   ropes[i].gameObject.SetActive(false);
           ropes[i].transform.position = new Vector3(-0.75f + 0.4f*i, 0, 2.11f);
            //ropes[i].ResetRopeLength();
            
            
        }
    }
}
