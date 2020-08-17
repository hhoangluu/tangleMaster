using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLengthController : MonoBehaviour
{
    private float speed =4f;
    private ObiRopeCursor cursor;
    private ObiRope rope;
    private float originLength;
    void Start()
    {
        cursor = GetComponent<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
        originLength = rope.restLength;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
        Debug.Log(rope.restLength);
    }

    public void ShortenRope()
    {
        StartCoroutine(CRShortenRope());
    }

    public void ResetLength()
    {
        // Debug.Log("le"+ rope.restLength);
        rope.ResetParticles();
       cursor.ChangeLength(originLength);
        //while (rope.CalculateLength() < originLength)
        //    cursor.ChangeLength(rope.restLength + 0.01f); //do this until the Calculated Length matches your desired length

    }



    IEnumerator CRShortenRope()
    {

        while (true)
        {
            if (rope == null) { break; }
                if (rope.restLength - speed * Time.deltaTime <= 0)
                {
                    cursor.ChangeLength(0);
                    break;
                }
                else
                {
                    cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
                }
            
            yield return new WaitForEndOfFrame();
        }

    }
}
