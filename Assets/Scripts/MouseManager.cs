using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour {

    private Unit selectedPlayer;
    public List<Unit> selectedPlayers = new List<Unit>();
    public Golgi gol;

    bool isSelecting = false;
    Vector3 mousePosition1;

    // Update is called once per frame
    void Update () {
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
        if (Input.GetMouseButtonDown(0))
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
}
