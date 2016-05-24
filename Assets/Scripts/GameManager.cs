using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int energy;
    int maxHealth = 100;
    int health = 100;
    public static GameManager instance = null;
    public Texture2D cursorTexture;
    public GameObject healthBar;
    public GameObject energyText;
    List<GameObject> healthBars = new List<GameObject>();
    public GameObject nucleus;


    // Used for virus spawn behaviour
    public GameObject virus;
    public GameObject virusParent;
    float timer = 0f;
    float spawnTime = 3f;
    public bool spawning = true;
    public int maxViruses = 5;
    public int noViruses = 0;
    public GameObject spawnArea;  // Not implemented yet

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
    void Start() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        energy = 100;

        foreach (Transform child in healthBar.transform)
        {
            healthBars.Add(child.gameObject);

        }
        //Debug.Log("No of healthbars" + healthBars.Count.ToString());
        //SpawnVirus();
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if(timer > spawnTime)
        {
            SpawnVirus();
            timer = 0;
        }
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

        Vector3 pos = new Vector3(
            Random.Range(-1.5f, -9.5f),
            Random.Range(3f, 3f),
            Random.Range(-1.5f, -5f));

        GameObject newVirus = Instantiate(virus);
        newVirus.transform.position = pos;
        newVirus.transform.parent = virusParent.transform;
        //Vector3 unitPos = nucleus.transform.position;
        //float range = 0.7f;
        //unitPos = new Vector3(
        //    unitPos.x + Random.Range(-range, range),
        //    unitPos.y,
        //    unitPos.z + Random.Range(-range, range));

        noViruses += 1;
        Debug.Log(noViruses);
        //newVirus.GetComponent<VirusMovement>().target = unitPos;
    }

    public bool GetEnergy(int amount)
    {
        if (amount > energy)
            return false;

        energy -= amount;
        UpdateEnergyUI();
        return true;
    }

    public void NucleusTakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthUI();
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
