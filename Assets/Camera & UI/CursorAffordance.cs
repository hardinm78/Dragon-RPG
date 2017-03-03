using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D targetCursor = null;
	[SerializeField] Texture2D unknownCursor = null;

	[SerializeField] Vector2 cursorHotSpot = new Vector2 (0, 0);
	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
		cameraRaycaster = GetComponent<CameraRaycaster> ();
		cameraRaycaster.layerChangeObservers += OnLayerChange; //registering
	}
	
	//only called when layer changes
	void OnLayerChange() {
		
		switch(cameraRaycaster.layerHit){
		case Layer.Walkable:
			Cursor.SetCursor (walkCursor, cursorHotSpot, CursorMode.ForceSoftware);
			break;
		case Layer.Enemy:
			Cursor.SetCursor (targetCursor, cursorHotSpot, CursorMode.ForceSoftware);
			break;
		case Layer.RaycastEndStop:
			Cursor.SetCursor (unknownCursor, cursorHotSpot, CursorMode.ForceSoftware);
			break;
		default:
			Debug.Log ("error");
			return;
		}

		print (cameraRaycaster.layerHit);


	}
}
