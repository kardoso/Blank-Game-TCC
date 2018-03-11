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
		GetComponent<Animator>().SetBool("Active", isEnabled);
		yield return new WaitForSeconds(secondsIfItsTemporary);
		StartCoroutine("Change");
	}

	void EnableCollider(){
		GetComponent<BoxCollider2D>().enabled = true;
	}

	void DisableCollider(){
		GetComponent<BoxCollider2D>().enabled = false;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			other.GetComponent<Player>().MakeDamage();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			other.GetComponent<Player>().MakeDamage();
		}
	}
}
