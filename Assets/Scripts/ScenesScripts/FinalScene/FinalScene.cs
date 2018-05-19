using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScene : MonoBehaviour {

	public string levelToLoad;
	public Player player;

	private Fade fade;

	public void StartEndGame(){
		//lightInPlayer.SetActive(true);
		//RenderSettings.ambientLight = Color.black;
		//Player Variables
		player.StopMovement();
		player.GetComponent<SpriteRenderer>().sortingOrder = 3;
		player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
		player.GetComponent<Animator>().SetFloat("yVelocity", -1);
		player.enabled = false;
		FindObjectOfType<LevelManager>().CanPause = false;
		StartCoroutine("PleaseJustFinish");
	}

	IEnumerator PleaseJustFinish(){
		yield return new WaitForSeconds(3);
		EndGame();
	}

	private void EndGame(){
		//Player Variables
		player.enabled = true;
		TransitionManager.Instance.SetColor(Color.white);
		TransitionManager.Instance.LoadLevel(levelToLoad, 2);
	}
}
