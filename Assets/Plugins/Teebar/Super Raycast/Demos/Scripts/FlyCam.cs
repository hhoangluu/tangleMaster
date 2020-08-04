using UnityEngine;
using System.Collections;

namespace SuperRaycastDemo
{
	public class FlyCam : MonoBehaviour
	{
		public float speed = .5f;
		Vector3 dragStart;
		Vector3 lastMP;
		Vector3 rot;

		void Start()
		{
			rot = transform.eulerAngles;
		}

		void Update ()
		{
			var delta = Input.mousePosition - lastMP;
			lastMP = Input.mousePosition;

			if (Input.GetMouseButton(0))
			{
				rot.x -= delta.y * .1f;
				rot.y += delta.x * .1f;
				transform.rotation = Quaternion.Euler (rot);
			}

			if (Input.GetKey (KeyCode.A)) transform.position -= transform.right * speed;
			if (Input.GetKey (KeyCode.D)) transform.position += transform.right * speed;
			if (Input.GetKey (KeyCode.S)) transform.position -= transform.forward * speed;
			if (Input.GetKey (KeyCode.W)) transform.position += transform.forward * speed;
			if (Input.GetKey (KeyCode.Q)) transform.position -= transform.up * speed;
			if (Input.GetKey (KeyCode.E)) transform.position += transform.up * speed;
		}
	}
}