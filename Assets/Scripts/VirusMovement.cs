using UnityEngine;
using System.Collections;

public class VirusMovement : MonoBehaviour {

    Vector3 target;
    public GameObject gameTarget;
    NavMeshAgent nav;
    int attackDamage = 10;
    int attackFrequency = 1;
    float timer = 0f;

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

		if (currentMode == mode.Moving) {

			// Check if the targetGate is still "alive", otherwise we target the nucleus
			if (currentPhase == phases.Gate && gameTarget.GetComponent<Gate>().IsDestroyed()) {
				currentPhase = phases.Nucleus;
				SetTarget(GameManager.instance.nucleus);
			}

			if (target != null)
			{
				nav.SetDestination(target);
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
				SetTarget(GameManager.instance.nucleus);
			}
		} else if (currentMode == mode.Fighting) {
		
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
				gameTarget.GetComponent<Gate>().TakeDamage (attackDamage);
			}
			timer = 0;
		}
	}

    public bool TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            //Destroy(gameObject);
            return false;
        }
        return true;
    }
}
