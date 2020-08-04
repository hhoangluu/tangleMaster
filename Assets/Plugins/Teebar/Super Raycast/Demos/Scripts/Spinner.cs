using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperRaycastDemo
{
	public class Spinner : MonoBehaviour
	{
		float x, y, z;

		void Start ()
		{
			x = Random.Range (-1, 1);
			y = Random.Range (-1, 1);
			z = Random.Range (-1, 1);
		}
		
		void Update ()
		{
			transform.Rotate (x, y, z);
		}
	}
}
