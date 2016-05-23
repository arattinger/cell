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
        Debug.Log("No of healthbars" + healthBars.Count.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
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
