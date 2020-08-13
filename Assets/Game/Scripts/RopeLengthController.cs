using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLengthController : MonoBehaviour
{
    private float speed =1.2f;
    private ObiRopeCursor cursor;
    private ObiRope rope;

    void Start()
    {
        cursor = GetComponent<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
    }

    void Update()
    {
        //if(cursor && rope)
        //{
        //    if ( rope.restLength > 0)
        //    {

        //    if (Input.GetKey(KeyCode.W))
        //        cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
        //    }

        //    if (Input.GetKey(KeyCode.S))
        //        cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
        //}
    }

    public void ShortenRope()
    {
        StartCoroutine(CRShortenRope());
    }

    IEnumerator CRShortenRope()
    {
        while (true)
        {
            if (rope.restLength - speed * Time.deltaTime < 0)
            {
                cursor.ChangeLength(0);
                break;
            }
            cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

    }
}
