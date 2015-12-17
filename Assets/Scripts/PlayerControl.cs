using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float movementSpeed = 10f;

	private Rigidbody rigi;

	// Use this for initialization
	void Start () {
		rigi = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontalMovement = Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime;
		float verticallMovement = Input.GetAxis ("Vertical") * movementSpeed * Time.deltaTime;

		Vector3 movement = new Vector3 (horizontalMovement, verticallMovement, 0);
		transform.Translate (movement);
	}
}
