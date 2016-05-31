using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour {

    private Unit selectedPlayer;
    public List<Unit> selectedPlayers = new List<Unit>();
    public Golgi gol;
	public GameObject golb;
	public GameObject mito;
	bool building = false;

	public Transform currentBuilding;

    bool isSelecting = false;
    Vector3 mousePosition1;
	public static MouseManager instance = null;

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
		
    // Update is called once per frame
    void Update () {

		if (building && currentBuilding != null) {
			Vector3 m = Input.mousePosition;
			m = new Vector3 (m.x, m.y, transform.position.y);
			Vector3 p = Camera.main.ScreenToWorldPoint (m);
			currentBuilding.position = new Vector3 (p.x, currentBuilding.position.y, p.z);

			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast (ray, out hitInfo)) {
					GameObject ourHitObject = hitInfo.collider.transform.gameObject;
					if (ourHitObject.tag == "Cell" || ourHitObject.tag == "Golgi") {
						building = false;
						currentBuilding = null;
					} else {
						Debug.Log ("Cant place the mito here " + ourHitObject.tag);
					}
				}

			}
		} else {
		 
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hitInfo;

	        if(Physics.Raycast(ray, out hitInfo)) {
	            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

	            if(Input.GetMouseButtonDown(0))
	            {
	                //Debug.Log("Test 2", ourHitObject);
	                Debug.Log(ourHitObject.tag);
	                Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

	                if(ourHitObject.tag == "Unit") {
	                    HitPlayer(ourHitObject);
	                } else if(ourHitObject.tag == "Golgi")
	                {
	                    HitGolgi(ourHitObject, Input.mousePosition);
					} else if(ourHitObject.tag == "MitoBuild") {
						HitMito (ourHitObject, Input.mousePosition);
					} else if(ourHitObject.tag == "GolgiBuild") {
						HitGolgiBuild (ourHitObject, Input.mousePosition);
					} 

	            } else if (Input.GetMouseButtonDown(1))
	            {
	                if (ourHitObject.tag == "Cell")
	                {
	                    HitCell(ourHitObject, Input.mousePosition);
	                    //HitCell(ourHitObject, hitInfo.collider.transform);
	                }
	            }
	        }


	        // If we press the left mouse button, save mouse location and begin selection
			if (Input.GetMouseButtonDown(0) && !building)
	        {
	            isSelecting = true;
	            mousePosition1 = Input.mousePosition;
	        }
	        // If we let go of the left mouse button, end selection
	        if (Input.GetMouseButtonUp(0))
	        {
	            selectedPlayers.Clear();
	            foreach (GameObject unit in gol.units)
	            {
	                if(IsWithinSelectionBounds(unit))
	                {
	                    selectedPlayers.Add(unit.GetComponent<Unit>());
	                    unit.GetComponent<Unit>().SetOutline(true);
	                } else
	                {
	                    unit.GetComponent<Unit>().SetOutline(false);
	                }
	            }
	            isSelecting = false;
	        }
		}

    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void HitCell(GameObject hitObject, Vector3 pos) {
        //Debug.Log("Hit cell");

        //if (selectedPlayer != null)
        //{
            foreach (Unit unit in selectedPlayers) {
                unit.SetDestination(Camera.main.ScreenToWorldPoint(pos));
            }
            //selectedPlayer.SetDestination(Camera.main.ScreenToWorldPoint(pos));
        //}
    }

    void HitPlayer(GameObject hitObject) {
        selectedPlayer = hitObject.GetComponent<Unit>();

        foreach (GameObject unit in gol.units)
        {
            unit.GetComponent<Unit>().SetOutline(false);
        }

        selectedPlayer.SetOutline(true);
        Debug.Log("Hit Unit");
    }

    void HitGolgi(GameObject hitObject, Vector3 pos)
    {
        Golgi gol = hitObject.GetComponent<Golgi>();
        gol.CreateUnit(pos);
    }

	void HitMito(GameObject hitObject, Vector3 pos) {

		if (GameManager.instance.GetEnergy (mito.GetComponent<Mitochondria> ().buildCost)) {
			currentBuilding = ((GameObject)Instantiate (mito)).transform;
			building = true;
			Debug.Log ("Instantiate mito");
		} else {
			Debug.Log ("Not enough energy");
		}

	}

	void HitGolgiBuild(GameObject hitObject, Vector3 pos) {

		if (GameManager.instance.GetEnergy (golb.GetComponent<Golgi> ().buildCost)) {
			currentBuilding = ((GameObject)Instantiate (golb)).transform;
			building = true;
			Debug.Log ("Instantiate golgi");
		} else {
			Debug.Log ("Not enough energy");
		}

	}
}
