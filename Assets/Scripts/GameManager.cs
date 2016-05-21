using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    int energy;
    public static GameManager instance = null;

    public GameObject selection = null;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        energy = 100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
