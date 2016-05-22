using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public Vector3 destination;
    public float speed = 5;
    NavMeshAgent nav;
    Transform target;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        destination = transform.position;
    }

    public void SetDestination(Vector3 dest)
    {
        destination = new Vector3(dest.x, destination.y, dest.z);
        nav.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(destination);
        //Debug.Log(destination);
        //Vector3 dir = destination - transform.position;
        //Vector3 velocity = dir.normalized * speed * Time.deltaTime;

        ////velocity = Vector3.ClampMagnitude(velocity, dir.magnitude);
        //transform.Translate(velocity);
    }

}
