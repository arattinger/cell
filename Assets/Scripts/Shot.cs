using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public Vector3 target;
	public GameObject targetV;
	public float speed = 5f;
	int damage = 10;

	// Use this for initialization
	void Start () {
//		float angle = Vector3.Angle(
//			new Vector3(transform.position.x, 0, transform.position.z), 
//			new Vector3(target.x, 0, target.z)
//		);
//		Debug.Log (transform.position + " - " + target);
//		transform.rotation = Quaternion.AngleAxis (angle, new Vector3(0, 1, 0));
//		Debug.Log("Angle:" + angle);
//		transform.rotation = 
	}

	// Update is called once per frame
	void Update () {
//		transform.rotation
//		float angle = Vector3.Angle(
//			new Vector3(transform.position.x, 0, transform.position.z), 
//			new Vector3(target.x, 0, target.z)
//		);
//		transform.rotation = Quaternion.AngleAxis (angle, new Vector3(0, 1, 0));
		Vector3 v = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
//		Debug.Log (Vector3.Distance(transform.position, target));
//		transform.rotation = Quaternion.FromToRotation (transform.position, v);

		if (Vector3.Distance (transform.position, target) < 0.1) {
			Destroy (gameObject);
			if (targetV != null) {
				targetV.GetComponent<VirusMovement> ().TakeDamage (damage);
			}
		}

		transform.position = v;
	}
		

}
