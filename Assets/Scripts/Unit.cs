using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public Vector3 destination;
    public float speed = 5;
    NavMeshAgent nav;
    Transform target;
    GameObject outline;
    float aggressionRange = 1f;
    GameObject attackTarget;

	int health = 100;
    int attackDamage = 10;
    int attackFrequency = 1;
    float timer = 0f;

	float energyTimer = 1.5f;
	float consumptionTimer = 0f;
	int consumption = 1;

    public enum mode { Controlled, Following, Attacking };
    mode currentMode = mode.Controlled;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        outline = transform.GetChild(0).GetChild(0).gameObject;
    }

    // Use this for initialization
    void Start()
    {
        destination = transform.position;
    }

    public void SetOutline(bool value)
    {
        outline.SetActive(value);
    }

    public void SetDestination(Vector3 dest)
    {
        destination = new Vector3(dest.x, destination.y, dest.z);
        nav.SetDestination(destination);
    }

	public void TakeDamage (int amount) {
		health -= amount;
	}

	public bool IsDestroyed() {
		if (health <= 0) {
			return true;
		}
		return false;
	}

    // Update is called once per frame
    void Update()
    {
		// Consume a bit of energy for operating the spaceship
		consumptionTimer += Time.deltaTime;
		if (consumptionTimer > energyTimer) {
			GameManager.instance.GetEnergy (consumption);
			consumptionTimer = 0;
		}


        foreach(GameObject virus in GameManager.instance.viruses)
        {
            //Debug.Log("Distance from virus:" + (virus.transform.position - transform.position).sqrMagnitude.ToString());
            if ((virus.transform.position - transform.position).sqrMagnitude < aggressionRange)
            {
                currentMode = mode.Attacking;
                attackTarget = virus;
            }
        }

        if(currentMode == mode.Controlled)
        {
            nav.SetDestination(destination);
        } else if(currentMode == mode.Attacking)
        {
			timer += Time.deltaTime;
			if(timer > attackFrequency && attackTarget)
            {	
				attackTarget.GetComponent<VirusMovement> ().TakeDamage (attackDamage);
                // Virus has zero health, destroy it
				if (attackTarget.GetComponent<VirusMovement> ().IsDestroyed ()) {
					Destroy(attackTarget);
					GameManager.instance.viruses.Remove(attackTarget);
					attackTarget = null;
					currentMode = mode.Controlled;
				}
                
                // Destroy the lysosome as well when the virus is defeated
                // Destroy(gameObject);
				timer = 0;
            }
        }

    }

}
