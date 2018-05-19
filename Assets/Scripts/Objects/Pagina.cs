using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pagina : MonoBehaviour {

	bool active = false;
	public Image imgPagina;
	public GameObject textoPagina;
	public int numeroPagina;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel") && active){
			Time.timeScale = 1;
			//player.ImBack();
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.SepiaTone>().enabled = false;
			imgPagina.gameObject.SetActive(false);
			FindObjectOfType<LevelManager>().CanPause = true;
			FindObjectOfType<LevelManager>().DeactivePause();
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player") && !active){

			Lang.Instance.setLanguage(1, GameManager.Instance.Language);
			textoPagina.GetComponent<PageTextWriter>().fullText = Lang.Instance.getString("page"+numeroPagina);

			Time.timeScale = 0;
			imgPagina.gameObject.SetActive(true);
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.SepiaTone>().enabled = true;
			FindObjectOfType<LevelManager>().CanPause = false;
			active = true;
			//player.StopMovement();//imback to return
		}
	}
}
