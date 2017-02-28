using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
	bool isInDirectMovementMode = false;
	[SerializeField] float walkMoveStopRadius = 0.2f;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }
    private void Update(){
		if (Input.GetKeyDown(KeyCode.G)){			
			isInDirectMovementMode = !isInDirectMovementMode;
			print (isInDirectMovementMode);
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

       Vector3	m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
       Vector3 m_Move = v*m_CamForward + h*Camera.main.transform.right;

		m_Character.Move (m_Move, false, false);
	}



	void ProcessMouseMovement () {
		if (Input.GetMouseButton (0)) {
			print ("Cursor raycast hit" + cameraRaycaster.layerHit);
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
			m_Character.Move (playerToClickPoint, false, false);
		}
		else {
			m_Character.Move (Vector3.zero, false, false);
		}
	}
}

