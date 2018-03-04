using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFromEnemy : MonoBehaviour {

	public float velocidade;
	private Transform player;
	private Vector2 alvo;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		alvo = new Vector2 (player.position.x, player.position.y);
	}
	
	void Update () {
		transform.position = Vector2.MoveTowards (transform.position, alvo, velocidade * Time.deltaTime);
		if (transform.position.x == alvo.x && transform.position.y == alvo.y) {
			Destroybala ();
		}
	}

	void OnTriggerEnter2D (Collider2D Other){
		if (Other.CompareTag ("Player")) {
			Destroybala ();
		}
	
	}

	void Destroybala(){
		Destroy (gameObject);
	
	}
}
