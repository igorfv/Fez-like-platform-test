using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float movementSpeed;
	public float jumpSpeed;
	public string platformTagName;
	public GameObject level;

	private bool grounded;
	private Rigidbody rb;
	private GameObject[] platforms;
	private bool zPosition = true;
	private bool turning = false;

	private Vector3 positionBeforeJump;

	public float animationTime;
	private float animationEndTime;
	private float animationAngle;
	private Vector3 animationStartPosition;
	private Vector3 animationEndPosition;

	// Use this for initialization
	void Start () {
		grounded = true;
		rb = GetComponent<Rigidbody> ();

		platforms = GameObject.FindGameObjectsWithTag(platformTagName);
	}
	
	// Update is called once per frame
	void Update () {

		if (turning) {
			RotateLevel ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.Z) && grounded) {
			animationAngle = 90f;
			RotateLevel ();
			return;
		}

		if (Input.GetKeyDown (KeyCode.X) && grounded) {
			animationAngle = -90f;
			RotateLevel ();
			return;
		}


		UpdatePlatformBoxCollider ();

		float horizontalMovement = Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime;
		float verticallMovement = 0f;

		if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
			positionBeforeJump = transform.position;
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
			UpdatePlayerPositionInTheWorld (other);
		}

		if (other.gameObject.CompareTag ("Respawn"))
		{
			transform.position = positionBeforeJump;
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


	void UpdatePlayerPositionInTheWorld (Collider platform) {
		Vector3 playerPosition;

		if (zPosition) {
			playerPosition = new Vector3 (transform.position.x, transform.position.y, platform.transform.position.z);
		} else {
			playerPosition = new Vector3 (platform.transform.position.x, transform.position.y, transform.position.z);
		}

		transform.position = playerPosition;
	}


	void RotateLevel() {
		if (!turning) {
			animationEndTime = Time.time + animationTime;
			animationStartPosition = level.transform.rotation.eulerAngles;
			animationEndPosition = new Vector3 (0, animationStartPosition.y + animationAngle, 0);
			turning = true;
		}
			
		float distCovered = ((animationEndTime - Time.time - animationTime)/animationTime) * -1;

		if (distCovered >= 1) {
			distCovered = 1f;
			turning = false;
		}
	
		level.transform.eulerAngles = Vector3.Lerp (animationStartPosition, animationEndPosition, distCovered);
	}
}
