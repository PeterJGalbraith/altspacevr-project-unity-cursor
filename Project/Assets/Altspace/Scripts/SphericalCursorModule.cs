using UnityEngine;

public class SphericalCursorModule : MonoBehaviour {
	// This is a sensitivity parameter that should adjust how sensitive the mouse control is.
	public float Sensitivity;

	// This is a scale factor that determines how much to scale down the cursor based on its collision distance.
	public float DistanceScaleFactor;
	
	// This is the layer mask to use when performing the ray cast for the objects.
	// The furniture in the room is in layer 8, everything else is not.
	private const int ColliderMask = (1 << 8);

	// This is the Cursor game object. Your job is to update its transform on each frame.
	private GameObject Cursor;

	// This is the Cursor mesh. (The sphere.)
	private MeshRenderer CursorMeshRenderer;

	// This is the scale to set the cursor to if no ray hit is found.
	private Vector3 DefaultCursorScale = new Vector3(10.0f, 10.0f, 10.0f);

	// Maximum distance to ray cast.
	private const float MaxDistance = 100.0f;

	// Sphere radius to project cursor onto if no raycast hit.
	private const float SphereRadius = 10.0f;

    void Awake() {
		Cursor = transform.Find("Cursor").gameObject;
		CursorMeshRenderer = Cursor.transform.GetComponentInChildren<MeshRenderer>();
        CursorMeshRenderer.GetComponent<Renderer>().material.color = new Color(0.0f, 0.8f, 1.0f);
    }	

	void Update()
	{
		//**********************************************************************************************************************
		//Handle mouse movement to update cursor position.
		//**********************************************************************************************************************

		//Read in input from the mouse (and scale them by the defined sensitivity)
		float mouseX = Input.GetAxis ("Mouse X") * Sensitivity;
		float mouseY = Input.GetAxis ("Mouse Y") * Sensitivity;

		//Figure out the distance from the camera object to the cursor
		float camToCursorDistance = Vector3.Distance (transform.position, Cursor.transform.position);

		//Move the cursor by scaling X and Y movements and applying those movements to the cursor's transform
		Vector3 amountToMoveCursor = new Vector3 (mouseX * camToCursorDistance, mouseY * camToCursorDistance);
		Cursor.transform.Translate (amountToMoveCursor);

		//Find the new direction to the cursor (as a unit vector) and ensure cursor is facing that direction so it behaves as though it is on a sphere
		Vector3 camToCursorDirection = (Cursor.transform.position - transform.position).normalized;
		Cursor.transform.forward = camToCursorDirection;

		//**********************************************************************************************************************
		//Perform ray cast to find object cursor is pointing at.
		//Update cursor transform.
		//**********************************************************************************************************************
		var cursorHit = new RaycastHit();

		//Perform raycast from the camera position in the direction of the cursor, but limit the interaction distance and interactable objects to our variables
		Physics.Raycast (transform.position, camToCursorDirection, out cursorHit, MaxDistance, ColliderMask);

		// Update highlighted object based upon the raycast.
		if (cursorHit.collider != null)
		{
			Selectable.CurrentSelection = cursorHit.collider.gameObject;

			//Set the cursor's position to be the point on the surface of the collider where the raycast hit
			Cursor.transform.position = cursorHit.point;

			//Calculate the cursor's scale (based on the formula given in the instructions) and apply it to the cursor (after flattening the cursor based on the surface)
			float calulatedCusorScale = (cursorHit.distance * DistanceScaleFactor + 1.0f) / 2.0f;
			Cursor.transform.localScale = Vector3.one * calulatedCusorScale;
		}
		else
		{
			Selectable.CurrentSelection = null;

			//Set the cursor's position to the local origin and then move it forward/out
			Cursor.transform.localPosition = Vector3.zero;
			Cursor.transform.Translate(Vector3.forward * SphereRadius);

			//Set the cursor's scale to be the default scale defined in our application
			Cursor.transform.localScale = DefaultCursorScale;
		}
	}
}
