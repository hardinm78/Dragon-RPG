using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
	bool isInDirectMovementMode = false;
	[SerializeField] float walkMoveStopRadius = 0.2f;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }
    private void Update(){
		if (Input.GetKeyDown(KeyCode.G)){			
			isInDirectMovementMode = !isInDirectMovementMode;
			currentClickTarget = transform.position;
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
			//print ("Cursor raycast hit" + cameraRaycaster.layerHit);
			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point;
				break;
			case Layer.Enemy:
				print ("not moving to enemy");
				break;
			case Layer.RaycastEndStop:
				break;
			default:
				print ("i dont think we are supposed to be here lady");
				return;
			}
		}
		var playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude > walkMoveStopRadius) {
			thirdPersonCharacter.Move (playerToClickPoint, false, false);
		}
		else {
			thirdPersonCharacter.Move (Vector3.zero, false, false);
		}
	}
}

