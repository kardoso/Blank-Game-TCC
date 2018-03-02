using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag.Equals("Player")){
			//input
			other.gameObject.GetComponent<Inventory>().AddDoor();
			Destroy(this.gameObject);
		}
	}
}
