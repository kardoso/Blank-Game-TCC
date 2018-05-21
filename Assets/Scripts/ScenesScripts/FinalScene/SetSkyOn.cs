using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkyOn : MonoBehaviour {

	public GameObject sky;
	public AudioClip fallingFX;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			SoundManager.PlayBGM(fallingFX, true, 1);
			sky.SetActive(true);
			FindObjectOfType<Fade>().FadeGameObject(sky, .5f, 0, 1);
			FindObjectOfType<FinalScene>().StartEndGame();
			Destroy(gameObject);
		}
	}
}
