using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;


	bool isInDirectMovementMode = false;
	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float attackMoveStopRadius = 5f;


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }
    private void Update(){
		if (Input.GetKeyDown(KeyCode.G)){			
			isInDirectMovementMode = !isInDirectMovementMode;
			Cursor.visible = !Cursor.visible;
			currentDestination = transform.position;
			//print (isInDirectMovementMode);
		}
    }
    // Fixed update is called in sync with physics
    private void FixedUpdate ()	{

		if (isInDirectMovementMode){
			ProcessDirectMovement ();
		}else {
			ProcessMouseMovement ();
		}		
    }

	void ProcessDirectMovement (){
		float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

       Vector3	cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
       Vector3 movement = v*cameraForward + h*Camera.main.transform.right;

		thirdPersonCharacter.Move (movement, false, false);
	}



	void ProcessMouseMovement () {
		if (Input.GetMouseButton (0)) {
			 clickPoint = cameraRaycaster.hit.point;
			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentDestination = ShortDestination(clickPoint,walkMoveStopRadius);
				break;
			case Layer.Enemy:
				currentDestination = ShortDestination(clickPoint,attackMoveStopRadius);
				break;
			case Layer.RaycastEndStop:
				break;
			default:
				print ("i dont think we are supposed to be here lady");
				return;
			}
		}
		WalkToDestination ();
	}

	Vector3 ShortDestination(Vector3 destination, float shortening){
		Vector3 reductionVector = (destination - transform.position).normalized * shortening;
		return destination - reductionVector;
	}

	void WalkToDestination ()
	{
		var playerToClickPoint = currentDestination - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
			thirdPersonCharacter.Move (playerToClickPoint, false, false);
		}
		else {
			thirdPersonCharacter.Move (Vector3.zero, false, false);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, currentDestination); 
		Gizmos.DrawSphere (currentDestination, 0.1f);
		Gizmos.DrawSphere (clickPoint, 0.15f);

		Gizmos.color = new Color (255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere (transform.position, attackMoveStopRadius);
		  	
    }
}

