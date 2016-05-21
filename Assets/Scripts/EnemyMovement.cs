using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    Transform target;
    NavMeshAgent nav;

	// Use this for initialization
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();

    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        nav.SetDestination(target.position);
	}
}
