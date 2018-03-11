using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public float jumpVelocity = 400f;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * 400f;
			GetComponent<Animator>().SetTrigger("Active");
		}
	}
}
