using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D targetCursor = null;
	[SerializeField] Texture2D unknownCursor = null;

	[SerializeField] Vector2 cursorHotSpot = new Vector2 (0, 0);
	[SerializeField] const int walkableLayerNumber = 8;
	[SerializeField] const int enemyLayerNumber = 9;

	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
		cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; //registering
	}
	
	//only called when layer changes
	void OnLayerChanged(int newLayer) {
		
		switch(newLayer){
		case walkableLayerNumber:
			Cursor.SetCursor (walkCursor, cursorHotSpot, CursorMode.ForceSoftware);
			break;
		case enemyLayerNumber:
			Cursor.SetCursor (targetCursor, cursorHotSpot, CursorMode.ForceSoftware);
			break;
		default:
			Cursor.SetCursor (unknownCursor, cursorHotSpot, CursorMode.ForceSoftware);
			return;
		}




	}
}
