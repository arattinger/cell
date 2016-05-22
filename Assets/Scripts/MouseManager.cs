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
                //Debug.Log("Test 2", ourHitObject);
                //Debug.Log(ourHitObject.tag);

                
                //SpriteRenderer spr = ourHitObject.GetComponentInChildren<SpriteRenderer>();
                //SpriteRenderer spr = ourHitObject.GetComponent<SpriteRenderer>();
                //spr.color = Color.red;
                
                if (ourHitObject.tag == "Cell") {
                    HitCell(ourHitObject, Input.mousePosition);
                    //HitCell(ourHitObject, hitInfo.collider.transform);
                } else if(ourHitObject.tag == "Unit") {
                    HitPlayer(ourHitObject);
                }

            }
        }
	}

    void HitCell(GameObject hitObject, Vector3 pos) {
        //Debug.Log("Hit cell");
        /*MeshRenderer mr = hitObject.GetComponentInChildren<MeshRenderer>();
        if (mr.material.color == Color.red)
        {
            mr.material.color = Color.white;
        }
        else
        {
            mr.material.color = Color.red;
        }*/
        Debug.Log("New Destination:");
        Debug.Log(Camera.main.ScreenToWorldPoint(pos));
        if (selectedPlayer != null)
        {
            selectedPlayer.SetDestination(Camera.main.ScreenToWorldPoint(pos));
            //selectedPlayer.destination = hitObject.transform.position;
        }
    }

    void HitPlayer(GameObject hitObject) {
        selectedPlayer = hitObject.GetComponent<Unit>();
        Debug.Log("Hit Unit");
    }
}
