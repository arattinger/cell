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

    int attackDamage = 10;
    int attackFrequency = 3;
    float timer = 0f;

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

    // Update is called once per frame
    void Update()
    {
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
            if(attackTarget && !attackTarget.GetComponent<VirusMovement>().TakeDamage(attackDamage))
            {
                // Virus has zero health, destroy it
                Destroy(attackTarget);
                GameManager.instance.viruses.Remove(attackTarget);
                attackTarget = null;

                // Destroy the lysosome as well when the virus is defeated
                // Destroy(gameObject);
            }
        }

    }

}
