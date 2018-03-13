using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : Enemy {

	protected States state;
	protected Animator anim;

	public float velseguir;
	public float seguir;
	private Transform alvo;

	public float velocidade;
	private float esperartempo;
	public float ficarparado;
	public Vector3 lugar;
	public float minx;
	public float maxx;
	public float miny;
	public float maxy;

	//private float hp = 2;


	protected override void Start () {
		base.Start();
		anim = GetComponent<Animator>();
		esperartempo = ficarparado;
		lugar = new Vector3 (Random.Range (minx, maxx), Random.Range (miny, maxy), transform.position.z);
		alvo = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
		state = States.idle;
	}

	protected override void Update () {
		base.Update ();
		CheckStates ();
		
		Idle();
		Walk();
		Attack(alvo.position);
		
		anim.SetFloat("walkTime", esperartempo);
		anim.SetBool("Die", isDead);
	}

	protected override void CheckStates () {
		if(esperartempo > 0){
			state = States.idle;
		}
		else{
			state = States.walk;
		}
		
		if(Vector2.Distance (transform.position, alvo.position) <= seguir){
			state = States.attack;
		}
	}

	protected override void Idle () {
		if(state == States.idle){
			if(esperartempo > 0){
				state = States.idle;
				esperartempo -= Time.deltaTime;
			}
			else{
				state = States.walk;
			}
		}
	}

	protected override void Walk () {
		if(state == States.walk){
			if (Vector2.Distance (transform.position, lugar) < 0.5f) {
				lugar = new Vector3 (Random.Range (minx, maxx), Random.Range (miny, maxy), 0);
				esperartempo = ficarparado;
			}
			transform.position = Vector3.MoveTowards (transform.position, lugar, velocidade * Time.deltaTime);
			
			if(lugar.x > transform.position.x){
				movingRight = true;
			}
			else if(lugar.x < transform.position.x){
				movingRight = false;
			}
		}
	}

	protected override void Attack(Vector2 whereThePlayerIs){
		if(state == States.attack){
			transform.position = Vector2.MoveTowards (transform.position, new Vector3(whereThePlayerIs.x, whereThePlayerIs.y, transform.position.z), velseguir * Time.deltaTime);
			if(whereThePlayerIs.x > transform.position.x){
				movingRight = true;
			}
			else if(whereThePlayerIs.x < transform.position.x){
				movingRight = false;
			}
		}
	}

	public void ResetHP(){
		//hp = 2;
	}

	public override void MakeDamage() {
		esperartempo = ficarparado;
		anim.SetTrigger("Damage");
		//hp--;
		//if(hp <= 0){
			GetComponent<BoxCollider2D>().enabled = false;
			isDead = true;
	}

    public override void Respawn(){
		EnableGameObject();
		//ResetHP();
	}

	public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seguir);
    }
}
