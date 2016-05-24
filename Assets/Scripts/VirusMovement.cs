using UnityEngine;
using System.Collections;

public class VirusMovement : MonoBehaviour {

    public Vector3 target;
    public GameObject gameTarget;
    NavMeshAgent nav;
    int attackDamage = 10;
    int attackFrequency = 2;
    float timer = 0f;

    int health = 100;

    // Use this for initialization
    void Awake()
    {
        
    }

	void Start () {
        //target = GameObject.FindGameObjectWithTag("Nucleus").transform;
        //target = gameTarget.transform.position;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        if (target != null)
        {
            nav.SetDestination(target);
            //Debug.Log(nav.remainingDistance);
            //Debug.Log(gameTarget.transform.position);
        }
        timer += Time.deltaTime;
        if (timer > attackFrequency && nav.remainingDistance < 0.1f)
        {
            Attack();
            //Debug.Log(id + ": ");
            //Debug.Log(nav.remainingDistance);

            timer = 0;
        }

    }

    void Attack() {
        GameManager.instance.NucleusTakeDamage(attackDamage);
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
