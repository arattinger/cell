using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

    private Unit selectedPlayer;

	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo)) {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Test 2", ourHitObject);
                Debug.Log(ourHitObject.tag);
                if (ourHitObject.tag == "Cell") {
                    
                    HitCell(ourHitObject);
                } else if(ourHitObject.tag == "Unit") {
                    HitPlayer(ourHitObject);
                }

            }
        }
	}

    void HitCell(GameObject hitObject) {
        Debug.Log("Hit cell");
        /*MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();
        if (mr.material.color == Color.red)
        {
            mr.material.color = Color.white;
        }
        else
        {
            mr.material.color = Color.red;
        }*/

        if (selectedPlayer != null)
        {
            selectedPlayer.destination = hitObject.transform.position;
        }
    }

    void HitPlayer(GameObject hitObject) {
        selectedPlayer = hitObject.GetComponent<Unit>();
        Debug.Log("Hit Unit");
    }
}
