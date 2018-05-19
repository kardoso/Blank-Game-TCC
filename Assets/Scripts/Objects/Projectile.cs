using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour {

	Vector2 direction;
	float velocity = 100;
	bool collided; 

	// Use this for initialization
	void Start () {
		collided = false;
		if(direction == Vector2.right){
			transform.localRotation = Quaternion.Euler(0,0,0);
		}
		else if(direction == Vector2.left){
			transform.localRotation = Quaternion.Euler(0,180,0);
		}
		else if(direction == Vector2.up){
			transform.localRotation = Quaternion.Euler(0,0,90);
		}
		else if(direction == Vector2.down){
			transform.localRotation = Quaternion.Euler(0,0,-90);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!collided){
			transform.Translate(Vector2.right * Time.deltaTime * velocity);
		}
		if(Time.timeScale < 1 && Time.timeScale > 0){
			Destroy(this.gameObject);
		}
	}

	public void SetDirection(Vector2 dir){
		direction = dir;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		collided = true;
		Destroy(transform.GetChild(0).gameObject);
		if(other.gameObject.tag == "Player"){
			other.GetComponent<Player>().MakeDamage();
		}
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Animator>().SetTrigger("Explode");
	}

	void OnTriggerEnter2D(TilemapCollider2D other)
	{
		collided = true;
		Destroy(transform.GetChild(0).gameObject);
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Animator>().SetTrigger("Explode");
	}

	void Explode(){
		Destroy(this.gameObject);
	}

	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}
}
