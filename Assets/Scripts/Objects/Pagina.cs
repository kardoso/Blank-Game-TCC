using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pagina : MonoBehaviour {

	Player player;
	bool active = false;
	public Image imgPagina;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel")){
			Time.timeScale = 1;
			player.ImBack();
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.SepiaTone>().enabled = false;
			imgPagina.gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player") && !active){
			Time.timeScale = 0;
			imgPagina.gameObject.SetActive(true);
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.SepiaTone>().enabled = true;
			//player.StopMovement();//imback to return
		}
	}
}
