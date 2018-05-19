using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public float jumpVelocity;

	public AudioClip trampFX;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			SoundManager.PlaySFX(trampFX);
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;/*400f */
			GetComponent<Animator>().SetTrigger("Active");
		}
	}
}
