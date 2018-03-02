using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	Vector2 direction;
	float velocity = 80;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(direction * Time.deltaTime * velocity);
	}

	public void SetDirection(Vector2 dir){
		direction = dir;
	}

	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}
}
