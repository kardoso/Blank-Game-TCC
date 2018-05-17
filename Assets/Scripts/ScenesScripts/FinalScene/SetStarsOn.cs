using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStarsOn : MonoBehaviour {

	public GameObject stars;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			stars.SetActive(true);
			Destroy(gameObject);
		}
	}
}
