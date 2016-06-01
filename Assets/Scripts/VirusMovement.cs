using UnityEngine;
using System.Collections;

public class VirusMovement : MonoBehaviour {

    Vector3 target;
    public GameObject gameTarget;
	GameObject attackTarget;
    NavMeshAgent nav;
    int attackDamage = 10;
    int attackFrequency = 1;
    float timer = 0f;
	float aggressionRange = 1f;

	public GameObject anim;
	public GameObject still;

    int health = 100;

	public enum mode { Moving, GateAttack, NucleusAttack, Fighting };
	mode currentMode = mode.Moving;

	public enum phases { Gate, Nucleus};
	phases currentPhase = phases.Gate;

    // Use this for initialization
    void Awake()
    {
        
    }

	void Start () {
//        target = GameObject.FindGameObjectWithTag("Nucleus").transform;
        target = gameTarget.transform.position;
        nav = GetComponent<NavMeshAgent>();
    }

	public void SetTarget(GameObject attackTarget) {
		gameTarget = attackTarget;
		target = gameTarget.transform.position;
	}

    // Update is called once per frame
    void Update () {

		if (GameManager.instance.doingSetup)
			return;

		foreach(GameObject spaceship in GameManager.instance.spaceships)
		{
			//Debug.Log("Distance from virus:" + (virus.transform.position - transform.position).sqrMagnitude.ToString());
			if ((spaceship.transform.position - transform.position).sqrMagnitude < aggressionRange)
			{
//				Debug.Log ("New mode is fighting");
				currentMode = mode.Fighting;
				attackTarget = spaceship;
		
			}
		}

		if (currentMode == mode.Moving) {

			// Check if the targetGate is still "alive", otherwise we target the nucleus
			if (currentPhase == phases.Gate && gameTarget.GetComponent<Gate> ().IsDestroyed ()) {
				currentPhase = phases.Nucleus;
				SetTarget (GameManager.instance.nucleus);
			}

			if (target != null) {
				nav.SetDestination (target);
				// Debug.Log(nav.remainingDistance);
				// Debug.Log(nav.pathStatus);
				//Debug.Log(gameTarget.transform.position);
			}
			if (nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance < 0.1f) {
				if (currentPhase == phases.Gate) {
					currentMode = mode.GateAttack;
				} else if (currentPhase == phases.Nucleus) {
					currentMode = mode.NucleusAttack;
				}
			}

		} else if (currentMode == mode.GateAttack) {
			
			if (nav.remainingDistance < 0.1f) {
				Attack ();
			}

			if (gameTarget.GetComponent<Gate> ().IsDestroyed ()) {
				currentMode = mode.Moving;
				currentPhase = phases.Nucleus;
				SetTarget (GameManager.instance.nucleus);
			}
		} else if (currentMode == mode.Fighting) {
			nav.Stop ();
			if (attackTarget) {
				Attack ();
			}
		} else if (currentMode == mode.NucleusAttack) {
//			Debug.Log (nav.remainingDistance);
			if (nav.remainingDistance < 0.1f) {
				Attack ();
			}
		}
			
//        timer += Time.deltaTime;	
//		if (timer > attackFrequency && nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance < 0.1f)
//        {
////            Attack();
//            //Debug.Log(id + ": ");
//            Debug.Log(nav.remainingDistance);
//            timer = 0;
//        }

    }

    void Attack() {
		timer += Time.deltaTime;
		if (timer > attackFrequency) {
			if (currentMode == mode.NucleusAttack) {
				GameManager.instance.NucleusTakeDamage (attackDamage);
			} else if (currentMode == mode.GateAttack) {
				gameTarget.GetComponent<Gate> ().TakeDamage (attackDamage);
			} else if (currentMode == mode.Fighting) {
				attackTarget.GetComponent<Unit> ().TakeDamage (attackDamage);
				if (attackTarget.GetComponent<Unit> ().IsDestroyed ()) {
					GameManager.instance.spaceships.Remove (attackTarget);
					MouseManager.instance.gol.units.Remove (attackTarget);
					MouseManager.instance.selectedPlayers.Remove (attackTarget.GetComponent<Unit>());
					Destroy (attackTarget);
					currentMode = mode.Moving;
					nav.Resume ();
					attackTarget = null;
				}
			}
			timer = 0;
		}
	}

	public bool IsDestroyed() {
		if (health <= 0) {
			return true;
		}
		return false;
	}

    public bool TakeDamage(int amount)
    {
		//Debug.Log (health);
        health -= amount;
        if(health <= 0)
        {
			anim.SetActive(true);
			still.SetActive(false);
            //Destroy(gameObject);
            return false;
        }
        return true;
    }
}
