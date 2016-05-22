using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Golgi : MonoBehaviour {

    public List<GameObject> units = new List<GameObject>();
    public static Golgi instance = null;
    public GameObject unit;
    public GameObject unitParent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start () {
        //GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateUnit(Vector3 pos)
    {
        Debug.Log("Create Unit");
        
        GameObject newUnit = Instantiate(unit);
        newUnit.transform.parent = unitParent.transform;
        
        Vector3 unitPos = newUnit.transform.position;
        float range = 0.7f;
        newUnit.transform.position = new Vector3(
            unitPos.x + Random.Range(-range, range), 
            unitPos.y, 
            unitPos.z + Random.Range(-range, range));
        units.Add(newUnit);
    }
}
