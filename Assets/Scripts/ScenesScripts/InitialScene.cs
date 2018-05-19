using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialScene : MonoBehaviour {

	public Player player;
	public GameObject lightInPlayer;
	public GameObject skyLayer;
	public ParticleSystem stars;

	private Fade fade;
	private bool done;

	// Use this for initialization
	void Start () {
		fade = FindObjectOfType<Fade>();
		done = false;
		if(GameManager.Instance.initialScene){
			FindObjectOfType<LevelManager>().CanPause = false;
			//Important Scene Objects
			skyLayer.SetActive(true);
			//player position
			player.transform.position = new Vector3(0, 125, player.transform.position.z);
			//Player Variables
			player.GetComponent<SpriteRenderer>().sortingOrder = 3;
			player.fallMultiplier = 0;
			player.GetComponent<Rigidbody2D>().mass = 1;
			player.GetComponent<Rigidbody2D>().gravityScale = 2;
			player.GetComponent<Animator>().SetFloat("yVelocity", -1);
			player.enabled = false;
		}
		else{
			FindObjectOfType<LevelManager>().CanPause = true;
			RenderSettings.ambientLight = Color.white;
			skyLayer.SetActive(false);
			Destroy(lightInPlayer);
			Destroy(stars.gameObject);
			player.transform.position = new Vector3(0, 0, player.transform.position.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if((player.transform.position.y <= 0 && !done) && GameManager.Instance.initialScene){
			Debug.Log("ok");
			done = true;
			StartGame();
		}
	}

	void StartGame(){
		FindObjectOfType<LevelManager>().CanPause = true;
		GameManager.Instance.initialScene = false;
		fade.FadeGameObject(skyLayer, 1, 1, 0);
		stars.gameObject.SetActive(false);
		RenderSettings.ambientLight = Color.white;
		Destroy(lightInPlayer);
		//Player Variables
		player.GetComponent<SpriteRenderer>().sortingOrder = 0;
		player.enabled = true;
		player.fallMultiplier = 20;
		player.GetComponent<Rigidbody2D>().mass = 45;
		player.GetComponent<Rigidbody2D>().gravityScale = 30;
	}
}
