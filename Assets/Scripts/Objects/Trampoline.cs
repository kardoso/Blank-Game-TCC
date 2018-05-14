using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public float jumpVelocity = 400f;

	public AudioClip trampFX;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag.Equals("Player")){
			SoundManager.Instance.PlaySFX(trampFX);
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * 400f;
			GetComponent<Animator>().SetTrigger("Active");
		}
	}
}
