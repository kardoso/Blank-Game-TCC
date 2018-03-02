using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

	float inY = 0;
	float inX = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inY += Time.deltaTime/5;
		inX += Time.deltaTime/5;
		GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(inX, inY));
	}
}
