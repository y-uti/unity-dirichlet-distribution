using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public float radius = 3f;
	public float height = 2f;
	public float speed  = 2f;

	void Update()
	{
		float t = Time.timeSinceLevelLoad;
		float xp = radius * Mathf.Cos(t * speed * Mathf.PI / 60f);
		float yp = height;
		float zp = radius * Mathf.Sin(t * speed * Mathf.PI / 60f);
		transform.position = new Vector3(xp, yp, zp);
		transform.LookAt(new Vector3(0f, 1f, 0f));
	}
}
