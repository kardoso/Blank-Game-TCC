using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurprise : Enemy {

	protected States state;
	protected Animator anim;

	private Transform player;
	public GameObject bala;
	private Rigidbody2D rb;

	public float velocidade;
	public float seguir;
	public float voltar;

	private float tempotiro;
	public float podeatirar;

	public bool canShootAndMove;

	protected override void Start () {
		base.Start();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		tempotiro = podeatirar;
		canShootAndMove = false;
		anim = GetComponent<Animator>();
		StartCoroutine("AutoDestroy");
	}

	protected override void Update () {
		base.Update ();
		if(canShootAndMove){
			LookAtPlayer();
			Walk();
			Attack(player.position);
		}
	}

	protected override void CheckStates () {
	}

	protected override void Idle () {
	}

	protected override void Walk () {
		if (Vector2.Distance (transform.position, player.position) > seguir) {
			transform.position = Vector2.MoveTowards (transform.position, player.position, velocidade * Time.deltaTime);
		} 
		else if (Vector2.Distance (transform.position, player.position) < seguir && Vector2.Distance (transform.position, player.position) > voltar) {
			transform.position = this.transform.position;
		} 
		else if (Vector2.Distance (transform.position, player.position) < voltar) {
			transform.position = Vector2.MoveTowards (transform.position, player.position, -velocidade * Time.deltaTime);
		}
	}

	protected override void Attack(Vector2 whereThePlayerIs){
		if (tempotiro <= 0) {
			//Instantiate (bala, transform.position, Quaternion.identity);
			Instantiate(bala, transform.position, transform.GetChild(0).rotation);
			tempotiro = podeatirar;
		} 
		else {
			tempotiro -= Time.deltaTime;
		}
	}

	void LookAtPlayer(){
        Vector3 lookPos = player.position;
        lookPos = lookPos - transform.GetChild(0).position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void ActiveEnemy(){
		//This is called in animator event, last frame of "born" animation
		canShootAndMove = true;
	}
		
	public override void MakeDamage() {
		base.MakeDamage();
		canShootAndMove = false;
		GetComponent<BoxCollider2D>().enabled = false;
		anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		anim.SetTrigger("Die");
	}

	public override void Respawn(){
		canShootAndMove = false;
		GetComponent<BoxCollider2D>().enabled = false;
		Destroy(this.gameObject);
	}

	IEnumerator AutoDestroy(){
		yield return new WaitForSeconds(1f);
		canShootAndMove = true;
		yield return new WaitForSeconds(4f);
		MakeDamage();
	}
}
