using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDisplacementOn : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag.Equals("Player")){
			FindObjectOfType<CustomImageEffect>().enabled = true;
			Destroy(gameObject);
		}
	}
}
