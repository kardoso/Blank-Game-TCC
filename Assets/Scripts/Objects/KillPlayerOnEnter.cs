using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnEnter : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.layer.Equals(11)){
			other.GetComponent<Player>().MakeDamage();
		}
	}
}
