using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spine : MonoBehaviour {

	public bool isTemporary;
	public float secondsIfItsTemporary;
	private bool isEnabled;

	// Use this for initialization
	void Start () {
		isEnabled = true;
		if(isTemporary){
			StartCoroutine("Change");
		}
	}

	IEnumerator Change(){
		isEnabled = !isEnabled;
		if(isEnabled){
			GetComponent<SpriteRenderer>().enabled = true;
		}
		else{
			GetComponent<SpriteRenderer>().enabled = false;
		}
		GetComponent<BoxCollider2D>().enabled = isEnabled;
		yield return new WaitForSeconds(secondsIfItsTemporary);
		StartCoroutine("Change");
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			SceneManager.LoadScene(0);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			SceneManager.LoadScene(0);
		}
	}
}
