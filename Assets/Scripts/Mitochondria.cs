using UnityEngine;
using System.Collections;

public class Mitochondria : MonoBehaviour {

	int health = 100;
	float timer = 0f;
	float energyFrequency = 5f;
	int productionAmount = 5;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > energyFrequency) {
			GameManager.instance.AddEnergy(productionAmount);
			timer = 0;
		}
	}
}
