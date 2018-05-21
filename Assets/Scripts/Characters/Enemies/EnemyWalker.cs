using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy {

	protected States state;
	protected Animator anim;
	
	private Transform player;
	private float _noticeDistance = 150f;
	private bool seeingPlayer;
	private float _velocity = 25;
	private Rigidbody2D rb;

	private float timeInIdle = 4;
	private float timeWalking = 3;

	public GameObject key;
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
		StartCoroutine(NotAttacking());
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!isDead){
			base.Update();
			CheckStates();
			if(rb.velocity.y == 0){
				CheckFloorAndWall();
			}
			anim.SetFloat("xVel", rb.velocity.x==0?0:1);
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

	protected override void CheckStates(){
		if(movingRight){
			seeingPlayer = Physics2D.Linecast(new Vector2(transform.position.x+15, transform.position.y-10), new Vector2(transform.position.x + noticeDistance, transform.position.y-10), 1 << LayerMask.NameToLayer("Player"));
			Debug.DrawLine(new Vector2(transform.position.x+15, transform.position.y-10), new Vector2(transform.position.x + noticeDistance, transform.position.y-10), Color.red);
		}
		else{
			seeingPlayer = Physics2D.Linecast(new Vector2(transform.position.x-15, transform.position.y-10), new Vector2(transform.position.x - noticeDistance, transform.position.y-10), 1 << LayerMask.NameToLayer("Player"));
			Debug.DrawLine(new Vector2(transform.position.x-15, transform.position.y-10), new Vector2(transform.position.x - noticeDistance, transform.position.y-10), Color.red);
		}

		if(seeingPlayer){
			state = States.attack;
			StopCoroutine(NotAttacking());
		}
		
		if(state == States.walk){
			Walk();
		}
		else if(state == States.idle){
			Idle();
		}
		else if(state == States.attack){
			Attack(player.position);
			if(!seeingPlayer){
				state = States.idle;
				StartCoroutine(NotAttacking());
			}
		}
	}

	protected override void Idle(){
		velocity = 0;
	}

	protected override void Walk(){
		velocity = _velocity;
		rb.AddForce((Vector2.right * (velocity * 500)) * ((movingRight?1:-1) * Time.timeScale));
		if(rb.velocity.x > velocity){
			rb.velocity = new Vector2(velocity, rb.velocity.y);
		}
		if(rb.velocity.x < -velocity){
			rb.velocity = new Vector2(-velocity, rb.velocity.y);
		}
	}

	protected override void Attack(Vector2 whereThePlayerIs){
		velocity = _velocity*2;
		rb.AddForce((Vector2.right * (velocity * 500)) * ((movingRight?1:-1) * Time.timeScale));
		if(rb.velocity.x > velocity){
			rb.velocity = new Vector2(velocity, rb.velocity.y);
		}
		if(rb.velocity.x < -velocity){
			rb.velocity = new Vector2(-velocity, rb.velocity.y);
		}
	}

	IEnumerator NotAttacking(){
		while(true) {
			state = States.idle;
			yield return new WaitForSeconds(timeInIdle);
			state = States.walk;
			yield return new WaitForSeconds(timeWalking);
		}
	}

	public override void MakeDamage(){
		base.MakeDamage();
		isDead = true;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		GetComponent<BoxCollider2D>().enabled = false;
		anim.SetTrigger("Die");
		DropKey();
	}

	void DropKey(){
		Instantiate(key, new Vector3(transform.position.x, transform.position.y-16, transform.position.y), transform.rotation);
	}

	public override void Respawn(){
		EnableGameObject();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		StartCoroutine(NotAttacking());
	}
}
