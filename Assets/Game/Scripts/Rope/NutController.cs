using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutController : MonoBehaviour
{
    private bool turn = false;
    private float moveSpeed = 2;

    private MeshRenderer conveyorMeshRenderer;
    private float curY = 0;
    private float curYHead = 0;
    // Start is called before the first frame update
    void Start()
    {
        conveyorMeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turn)
        {
            curY -= Time.fixedDeltaTime * moveSpeed;
            conveyorMeshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, curY));
            curYHead += Time.fixedDeltaTime * moveSpeed;
        }
    }

    public void TurnNut()
    {
        turn = true;
    }
}
