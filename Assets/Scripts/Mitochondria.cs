using UnityEngine;
using System.Collections;

public class Mitochondria : MonoBehaviour {

	public int buildCost = 20;
	int health = 100;
	float timer = 0f;
	float energyFrequency = 1f;
	int productionAmount = 1;


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
