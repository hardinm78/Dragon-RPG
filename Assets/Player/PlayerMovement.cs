using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
public class PlayerMovement : MonoBehaviour
{
	
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
	GameObject walkTarget = null;

	AICharacterControl aiCharacterControl = null;
	[SerializeField] const int walkableLayerNumber = 8;
	[SerializeField] const int enemyLayerNumber = 9;

	bool isInDirectMovementMode = false;



     void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
		aiCharacterControl = GetComponent<AICharacterControl> ();
		walkTarget = new GameObject("walkTarget");

		cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;

    }


	void ProcessMouseClick(RaycastHit raycastHit, int layerHit){
		switch(layerHit){
		case enemyLayerNumber:
			GameObject enemy = raycastHit.collider.gameObject;
			aiCharacterControl.SetTarget (enemy.transform);
				break;
			case walkableLayerNumber:
			walkTarget.transform.position = raycastHit.point;
			aiCharacterControl.SetTarget(walkTarget.transform);
				break;
			default:
			Debug.Log("dont know how to handle click");
				return;
		}
	}

	//TODO fix
	void ProcessDirectMovement (){
		float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

       Vector3	cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
       Vector3 movement = v*cameraForward + h*Camera.main.transform.right;

		thirdPersonCharacter.Move (movement, false, false);
	}



}

