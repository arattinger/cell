using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public int health = 100;
	SpriteRenderer spr;

	// Use this for initialization
	void Start () {
		spr = GetComponentInChildren<SpriteRenderer> ();
		spr.enabled = false;
	}


	public void TakeDamage(int amount) {
		Debug.Log (health);
		health -= amount;
		if (health <= 0) {
			spr.enabled = true;
			GetComponent<NavMeshObstacle> ().enabled = false;
		}
	}

	public bool IsDestroyed() {
		if (health <= 0)
			return true;
		return false;
	}
}
