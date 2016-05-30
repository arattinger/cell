using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public int health = 100;

	// Use this for initialization
	void Start () {
	
	}

	public void TakeDamage(int amount) {
		Debug.Log (health);
		health -= amount;
	}

	public bool IsDestroyed() {
		if (health <= 0)
			return true;
		return false;
	}
}
