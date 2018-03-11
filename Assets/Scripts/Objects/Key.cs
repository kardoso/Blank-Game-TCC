using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			//input
			other.gameObject.GetComponent<Inventory>().AddKey();
			Destroy(this.gameObject);
		}
	}
}
