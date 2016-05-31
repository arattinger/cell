using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	private Text levelText;
	private GameObject levelImage;
	private int level = 1;

    public int energy;
    int maxHealth = 100;
    int health = 100;
    public static GameManager instance = null;
    public Texture2D cursorTexture;
    public GameObject healthBar;
    public GameObject energyText;
    List<GameObject> healthBars = new List<GameObject>();
    public GameObject nucleus;

	public List<GameObject> spaceships = new List<GameObject>();

    // Used for virus spawn behaviour
    public List<GameObject> viruses = new List<GameObject>();
    public GameObject virus;
    public GameObject virusParent;
    float timer = 0f;
    public float spawnTime = 1f;
    public bool spawning = true;
    public int maxViruses = 5;
    public int noViruses = 0;
    public GameObject spawnArea;  // Not implemented yet

	// Used to handle gates
	public GameObject gateParentLeft;
	public GameObject gateParentRight;
	public List<GameObject> gates = new List<GameObject>();
	public List<GameObject> leftGates = new List<GameObject>();
	public List<GameObject> rightGates = new List<GameObject>();

	public GameObject gameover;
	private bool doingSetup = true;
	public GameObject transition;
	public GameObject explanation;



    void Awake() {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

		InitGame ();
    }

    // Use this for initialization
    void Start() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        energy = 100;

        foreach (Transform child in healthBar.transform)
        {
            healthBars.Add(child.gameObject);

        }

		foreach (Transform child in gateParentLeft.transform) {
			gates.Add (child.gameObject);
			leftGates.Add (child.gameObject);
		}
		foreach (Transform child in gateParentRight.transform) {
			gates.Add (child.gameObject);
			rightGates.Add (child.gameObject);
		}

        //Debug.Log("No of healthbars" + healthBars.Count.ToString());
//        SpawnVirus();
    }

    // Update is called once per frame
    void Update () {

		if (doingSetup)
			return;

        timer += Time.deltaTime;
        if(timer > spawnTime)
        {
            SpawnVirus();
            timer = 0;
        }
    }


	void OnLevelWasLoaded(int index) {
		level++;
		InitGame ();
	}

	void InitGame() {
		Debug.Log ("Init Game");

//		GameObject transition = GameObject.Find ("GameTransition");



		if (level == 1) {
			Debug.Log ("Level 1 loaded");
			transition.SetActive (true);

			Invoke ("ExplanationPhase1", 2f);
		}
	}


	void ExplanationPhase1() {
		transition.SetActive (false);
		explanation.SetActive (true);
		Invoke ("StartGame", 5f);
	}


	void StartGame() {
		transition.SetActive (false);
		explanation.SetActive (false);


		doingSetup = false;
	}

    public void SpawnVirus()
    {
        if (!spawning || noViruses >= maxViruses)
            return;

        // TODO: Use actual spawnArea for this
        //Vector3 pos = new Vector3(
        //    Random.Range(spawnArea.transform.position.x, spawnArea.transform.position.x + spawnArea.transform.localScale.x),
        //    Random.Range(spawnArea.transform.position.y, spawnArea.transform.position.x + spawnArea.transform.localScale.y),
        //    Random.Range(spawnArea.transform.position.z, spawnArea.transform.position.x + spawnArea.transform.localScale.z));
		
		Vector3[] pos = new [] {
			new Vector3(
				Random.Range(-1.5f, -9.5f),
				Random.Range(3f, 3f),
				Random.Range(-1.5f, -5f)),
			new Vector3(
				Random.Range(-1.5f, -9.5f),
				Random.Range(3f, 3f),
				Random.Range(-16f, -20f))
		};
			
		//Debug.Log (pos[1]);
		int spawnIndex = Random.Range (0, 2);
		//Debug.Log (spawnIndex);
		//int spawnIndex = 1;

		GameObject newVirus = (GameObject) Instantiate(virus, pos[spawnIndex], virus.transform.rotation);
		//newVirus.transform.position = pos[spawnIndex];
        newVirus.transform.parent = virusParent.transform;

		if (spawnIndex == 0) {
			int targetIndex = Random.Range (0, leftGates.Count);
			newVirus.GetComponent<VirusMovement> ().SetTarget (leftGates [targetIndex]);
		} else {
			int targetIndex = Random.Range (0, rightGates.Count);
			newVirus.GetComponent<VirusMovement> ().SetTarget (rightGates [targetIndex]);
		}

        noViruses += 1;
        //Debug.Log(noViruses);
        viruses.Add(newVirus);
    }

	public void AddEnergy(int amount) {
		energy += amount;
//		UpdateEnergyUI ();
	}

    public bool GetEnergy(int amount)
    {
        if (amount > energy)
            return false;

        energy -= amount;
//        UpdateEnergyUI();
        return true;
    }

	void OnGUI() {
		UpdateEnergyUI ();
		UpdateHealthUI ();
	}

    public void NucleusTakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthUI();
		if(health <= 0) {
			gameover.SetActive (true);
		}
        Debug.Log("Nucleus taking damage");
    }

    void UpdateEnergyUI()
    {
        energyText.GetComponent<Text>().text = energy.ToString();
        //energyText.text = energy.ToString();
    }

    void UpdateHealthUI()
    {
        int remaining = (healthBars.Count*health) / maxHealth;

        for(int i=0; i < healthBars.Count; i++)
        {
            if(i < remaining)
            {
                healthBars[i].SetActive(true);
            } else
            {
                healthBars[i].SetActive(false);
            }
        }
    }
}
