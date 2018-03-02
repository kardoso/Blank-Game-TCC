using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryPlatform : MonoBehaviour {

	public float timeActive;
	public Sprite sprEnabled;
	public Sprite sprDisabled;
	public bool iniciarAtiva;
	private bool isActive;

	// Use this for initialization
	void Start () {
		isActive = iniciarAtiva;
		StartCoroutine("ChangeActivity");
	}
	
	IEnumerator ChangeActivity(){
		isActive = !isActive;
		if(isActive){
			GetComponent<SpriteRenderer>().sprite = sprEnabled;
			GetComponent<BoxCollider2D>().enabled = true;
		}
		else{
			GetComponent<SpriteRenderer>().sprite = sprDisabled;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		yield return new WaitForSeconds(timeActive);
		StartCoroutine("ChangeActivity");
	}
}
