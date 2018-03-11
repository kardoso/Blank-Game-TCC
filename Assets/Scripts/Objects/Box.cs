using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	Vector3 initialPos;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
	}
	
	public void Respawn(){
		FindObjectOfType<Fade>().FadeGameObject(this.gameObject, 0.5f, 1, 0);
		transform.position = initialPos;
		FindObjectOfType<Fade>().FadeGameObject(this.gameObject, 0.5f, 0, 1);
	}
}
