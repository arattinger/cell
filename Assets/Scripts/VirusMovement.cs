using UnityEngine;
using System.Collections;

public class VirusMovement : MonoBehaviour {

    Transform target;
    GameObject gameTarget;
    NavMeshAgent nav;
    int attackDamage = 10;
    int attackFrequency = 2;
    float timer = 0f;

	// Use this for initialization
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Nucleus").transform;
        nav = GetComponent<NavMeshAgent>();
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () { 
        nav.SetDestination(target.position);

        timer += Time.deltaTime;
        if (timer > attackFrequency && nav.remainingDistance < 0.1f)
        {
            Attack();
            timer = 0;
        }
        //Debug.Log(nav.remainingDistance);
	}

    void Attack() {
        GameManager.instance.NucleusTakeDamage(attackDamage);
    }

}
