using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLengthController : MonoBehaviour
{
    public float speed = 1;
    public ObiRopeCursor cursor;
    public ObiRope rope;

    void Start()
    {
        cursor = GetComponent<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
    }

    void Update()
    {
        if(cursor && rope)
        {
            if (Input.GetKey(KeyCode.W))
                cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
                cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
        }
    }
}
