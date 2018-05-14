using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDasher : Enemy {

	protected States state;
	protected Animator anim;
	
	private Transform player;
	private float _noticeDistance = 150f;
	private bool seeingPlayer;
	private float _velocity = 25;
	private Rigidbody2D rb;

	private float timeInStun = 4;

	public LayerMask wallLayerMask;
	public LayerMask floorLayerMask;
	public GameObject checkWallL, checkWallR, checkFloorL, checkFloorR;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		player = FindObjectOfType<Player>().transform;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		noticeDistance = _noticeDistance;
		movingRight = true;
		state = States.walk;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		CheckStates();
		if(rb.velocity.y == 0){
			CheckFloorAndWall();
		}
	}

	protected override void CheckStates(){
		RaycastHit2D infoHit;
		if(movingRight){
			infoHit = Physics2D.Linecast(new Vector2(transform.position.x+15, transform.position.y-10), new Vector2(transform.position.x + noticeDistance, transform.position.y-10));
			Debug.DrawLine(new Vector2(transform.position.x+15, transform.position.y-10), new Vector2(transform.position.x + noticeDistance, transform.position.y-10), Color.red);
		}
		else{
			infoHit = Physics2D.Linecast(new Vector2(transform.position.x-15, transform.position.y-10), new Vector2(transform.position.x - noticeDistance, transform.position.y-10));
			Debug.DrawLine(new Vector2(transform.position.x-15, transform.position.y-10), new Vector2(transform.position.x - noticeDistance, transform.position.y-10), Color.red);
		}

		if(infoHit.collider != null){
			seeingPlayer = infoHit.collider.gameObject.layer.Equals(11);
		}
		else{
			seeingPlayer = false;
		}

		if(seeingPlayer && state != States.idle){
			state = States.attack;
		}
		else{
			if(state != States.idle){
				state = States.walk;
			}
		}

		anim.SetBool("Dash", state == States.attack);
		
		if(state == States.walk){
			Walk();
		}
		else if(state == States.attack){
			Attack(player.position);
		}
	}

	private void CheckFloorAndWall(){
		var wallRight = Physics2D.OverlapCircle(checkWallR.transform.position, 0.1f, wallLayerMask);
		var wallLeft = Physics2D.OverlapCircle(checkWallL.transform.position, -0.1f, wallLayerMask);
		var floorRight = Physics2D.OverlapCircle(checkFloorR.transform.position, 0.1f, floorLayerMask);
		var floorLeft = Physics2D.OverlapCircle(checkFloorL.transform.position, -0.1f, floorLayerMask);
		if(movingRight){
			if(wallRight || !floorRight){
				movingRight = false;
			}
		}
		else{
			if(wallLeft || !floorLeft){
				movingRight = true;
			}
		}
	}

	protected override void Idle(){
		state = States.idle;
		StartCoroutine("Stun");
	}

	IEnumerator Stun(){
		anim.SetBool("Stun", true);
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(timeInStun);
		anim.SetBool("Stun", false);
		GetComponent<BoxCollider2D>().enabled = true;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		state = States.walk;
	}

	protected override void Walk(){
		velocity = _velocity;
		rb.AddForce((Vector2.right * velocity * 500) * (movingRight?1:-1) * Time.timeScale);
		if(rb.velocity.x > velocity){
			rb.velocity = new Vector2(velocity, rb.velocity.y);
		}
		if(rb.velocity.x < -velocity){
			rb.velocity = new Vector2(-velocity, rb.velocity.y);
		}
	}

	protected override void Attack(Vector2 whereThePlayerIs){
		velocity = _velocity*3;
		rb.AddForce((Vector2.right * velocity * 500) * (movingRight?1:-1) * Time.timeScale);
		if(rb.velocity.x > velocity){
			rb.velocity = new Vector2(velocity, rb.velocity.y);
		}
		if(rb.velocity.x < -velocity){
			rb.velocity = new Vector2(-velocity, rb.velocity.y);
		}
	}

	public override void MakeDamage(){
		base.MakeDamage();
		Idle();
	}

	public override void Respawn(){
		EnableGameObject();
	}
}
