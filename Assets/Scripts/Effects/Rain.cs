using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

	float inY = 0;
	float inX = 0;

	Color initColor;
	Color actualColor;

	// Use this for initialization
	void Start () {
		initColor = GetComponent<MeshRenderer>().material.GetColor("_CustomColor");
	}
	
	// Update is called once per frame
	void Update () {
		inY += Time.deltaTime/5;
		inX += Time.deltaTime/5;
		GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(inX, inY));

		if(Time.timeScale >= 1){
			actualColor = initColor;
		}
		else{
			actualColor = new Color(50, 50, 50, 255);
		}
		GetComponent<MeshRenderer>().material.SetColor("_CustomColor", actualColor);
	}
}
