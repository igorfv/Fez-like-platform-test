using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float movementSpeed;
	public float jumpSpeed;

	private bool grounded;
	private Rigidbody rb;
	private GameObject[] platforms;
	private bool zPosition = true;

	// Use this for initialization
	void Start () {
		grounded = true;
		rb = GetComponent<Rigidbody> ();

		platforms = GameObject.FindGameObjectsWithTag("Platform");

		UpdatePlatformBoxCollider ();
	}
	
	// Update is called once per frame
	void Update () {
//		UpdatePlatformBoxCollider ();

		float horizontalMovement = Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime;
		float verticallMovement = 0f;

		if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
			verticallMovement = 1f * jumpSpeed;
			grounded = false;
		}
			
		transform.Translate (new Vector3 (horizontalMovement, 0, 0), Space.World);
		rb.AddForce(new Vector3 (0, verticallMovement, 0));
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Platform"))
		{
			grounded = true;
		}
	}

	void UpdatePlatformBoxCollider () {
		for (int i = 0; i < platforms.Length; i++) {
			GameObject platform = platforms[i];
			BoxCollider collider = platform.GetComponent<BoxCollider>();
			collider.center = Vector3.zero;
			Vector3 colliderPosition = collider.transform.TransformPoint(collider.center);


			Vector3 colliderCenter;

			if (zPosition) {
				colliderCenter = new Vector3 (colliderPosition.x, colliderPosition.y, transform.position.z);
			} else {
				colliderCenter = new Vector3 (transform.position.x, colliderPosition.y, colliderPosition.z);
			}

			collider.center = collider.transform.InverseTransformPoint(colliderCenter);

		}
	}
}
