using UnityEngine;
using System.Collections;

public class OrbitObject : MonoBehaviour {
	public GameObject target;
	public bool RotateAsClock;
	private float RotateSpeed = -5f;
	private float Radius = 15f;

	private Vector2 _centre;
	private float _angle;

	private void Start()
	{
		RotateSpeed = RotateAsClock? 5f:-5f;
	}

	private void Update()
	{

		Vector2 targetPosition = target.transform.position;
		targetPosition.y -= 5;
		_centre = targetPosition;

		_angle += RotateSpeed * Time.deltaTime;

		var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
		Vector3 newPosition = _centre + offset;
		newPosition.z = -2;
		transform.position = newPosition;
	}

}