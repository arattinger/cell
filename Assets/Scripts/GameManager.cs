using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	public GameObject golb;

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
	public bool doingSetup = true;
	public GameObject transition;
	public GameObject explanation;

	public List<GameObject> steps = new List<GameObject>();
	public int fromStep = 0;
	public int toStep = 2;
	public int stepIndex = 0;
	public SpriteRenderer gol1;
	public SpriteRenderer gol2;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

		foreach (Transform child in explanation.transform) {
			steps.Add (child.gameObject);
		}

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

//		Debug.Log ("Update");
        timer += Time.deltaTime;
        if(timer > spawnTime)
        {
//			Debug.Log ("Spawn");
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
		if (level == -1) {
			doingSetup = false;
		} else if (level == 1) {
			Debug.Log ("Level 1 loaded");
			transition.SetActive (true);

			Invoke ("ExplanationPhase1", 2f);
		} else if (level == 2) {
			Debug.Log ("Level 2 loaded");
		}
	}

	void ExplanationPhase1() {
		transition.SetActive (false);
		explanation.SetActive (true);


		ShowStep ();
//		Invoke ("ShowStep", 0f);
	}


	void ShowStep() {
		if (stepIndex <= toStep) {
			if (stepIndex != 0) {
				steps [stepIndex - 1].SetActive (false);
			}
			steps [stepIndex].SetActive (true);
			stepIndex += 1;
			Invoke ("ShowStep", 3f);


		} else {
			steps [stepIndex].SetActive (false);
			Invoke ("Level" + level + "Setup", 5f);
			Invoke ("StartGame", 5f);
//			Application.LoadLevel (Application.loadedLevel);
		}
	}

	void Level1Setup() {
		Debug.Log("Level 1 Setup");
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
		Invoke ("Level1Over", 5f);
	}

	void Level2Setup() {
		Debug.Log("Level 2 Setup");


		AddEnergy (500);
		maxViruses = 4;
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
		golb.GetComponent<Golgi> ().CreateUnit (golb.transform.position);
//		ShowStep ();
		Invoke ("Level2Over", 5f);	
	}

	void Level3Setup() {
		Debug.Log("Level 3 Setup");

//		AddEnergy (500);
		maxViruses = 500;
		spawnTime = 5f;
//		golb.GetComponentInChildren<SpriteRenderer> ().enabled = true;
		gol1.enabled = true;		
//		gol2.enabled = true;
		gol2.GetComponent<Animator> ().Stop ();
		ShowStep ();
		Invoke ("Level3Over", 5f);	
	}

	void Level1Over() {
		if (GameManager.instance.viruses.Count == 0 && doingSetup == false) {
			// Start level 2
			level += 1;
			explanation.SetActive (true);
			doingSetup = true;
			fromStep = 3;
			toStep = 3;
			stepIndex = 3;

			ShowStep ();
//			Level2Setup();
		} else {
			Invoke ("Level1Over", 3f);
		}
	}

	void Level2Over() {
		if (GameManager.instance.viruses.Count == 0 && doingSetup == false) {
			// Start level 2
//			Level3Setup();
			level += 1;
			doingSetup = true;
			fromStep = 4;
			toStep = 4;
			stepIndex = 4;
			explanation.SetActive (true);

			ShowStep();
		} else {
			Invoke ("Level2Over", 3f);
		}
	}

	void Level3Over() {
		if (GameManager.instance.viruses.Count == 0 && doingSetup == false) {
			// Start level 2
//			Level3Setup();
		} else {
//			Invoke ("Level3Over", 3f);
		}
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
			Invoke ("RestartGame", 3f);
		}
        Debug.Log("Nucleus taking damage");
    }

	public void RestartGame() {
		Application.LoadLevel ("mainmenu");
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
