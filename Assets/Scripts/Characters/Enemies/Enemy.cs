using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour{

	//Estados da IA
	protected enum States{
		idle,
		walk,
		attack
	}
	//Distância para o inimigo perceber o player
	protected float noticeDistance;
	//Movendo para a direita ou para a esquerda
	//Usado para girar sprite e para mudar o raio de detecção
	protected bool movingRight;
	//Velocidade que o inimigo se moverá
	protected float velocity;
	public bool canDie;
	protected bool isDead = false;

	protected virtual void Update()
	{
		FlipSprite();
	}

	//Função para checar se o player está proximo ou se o inimigo está vendo o player
	protected abstract void CheckStates();
	//Função do que o inimigo fará quando estiver parado
	protected abstract void Idle();
	//o que o inimigo fará quando estiver andando
	protected abstract void Walk();
	//Função de ataque, tem como parâmetro a posição do player
	protected abstract void Attack(Vector2 whereThePlayerIs);
	//Função de dano no inimigo
	public abstract void MakeDamage();

	public void Respawn(){
		EnableGameObject();
	}

	protected void FlipSprite(){
		if (!movingRight)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
		else if (movingRight)
		{
			//inverter sprite na horizontal
			GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	private void DisableGameObject(){
		gameObject.SetActive(false);
	}

	public void EnableGameObject(){
		gameObject.SetActive(true);
	}

	void OnEnable()
	{
		if(isDead){
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			isDead = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag.Equals("Player")){
			col.gameObject.GetComponent<Player>().MakeDamage();
		}
	}
}
