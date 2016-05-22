using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    int energy;
    public static GameManager instance = null;
    public Texture2D cursorTexture;

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
            
        Cursor.SetCursor(cursorTexture, Vector2.zero,  CursorMode.ForceSoftware);
        energy = 100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
