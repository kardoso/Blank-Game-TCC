using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurprise : Enemy {

	protected States state;
	protected Animator anim;

	private Transform player;
	public GameObject bala;
	private float _velocity = 25;
	private Rigidbody2D rb;

	public float velocidade;
	public float seguir;
	public float voltar;

	private float tempotiro;
	public float podeatirar;

	protected override void Start () {
		base.Start();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		tempotiro = podeatirar;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	protected override void Update () {
		base.Update ();
		CheckStates ();
		if (player.position.x > transform.position.x) {
			transform.localScale = new Vector3 (100, 100, 1);
		} else if (player.position.x < transform.position.x) {
			transform.localScale = new Vector3 (-100, 100, 1);
		}
		if (Vector2.Distance (transform.position, player.position) > seguir) {
			transform.position = Vector2.MoveTowards (transform.position, player.position, velocidade * Time.deltaTime);

		} else if (Vector2.Distance (transform.position, player.position) < seguir && Vector2.Distance (transform.position, player.position) > voltar) {
			transform.position = this.transform.position;

		} else if (Vector2.Distance (transform.position, player.position) < voltar) {
			transform.position = Vector2.MoveTowards (transform.position, player.position, -velocidade * Time.deltaTime);
		}

		if (tempotiro <= 0) {
			Instantiate (bala, transform.position, Quaternion.identity);
			tempotiro = podeatirar;

		} else {
			tempotiro -= Time.deltaTime;
		}
	}

	protected override void CheckStates () {
		
	}

	protected override void Idle () {
		state = States.idle;
	}

	protected override void Walk () {
		state = States.walk;
	}

	protected override void Attack(Vector2 whereThePlayerIs){
		velocity = _velocity*2;
		rb.AddForce((Vector2.right * velocity * 500) * (movingRight?1:-1) * Time.timeScale);
		if(rb.velocity.x > velocity){
			rb.velocity = new Vector2(velocity, rb.velocity.y);
		}
		if(rb.velocity.x < -velocity){
			rb.velocity = new Vector2(-velocity, rb.velocity.y);
		}
	}
		
	public override void MakeDamage() {
		Debug.Log("Damage");
	}

	public override void Respawn(){
		EnableGameObject();
	}

}
