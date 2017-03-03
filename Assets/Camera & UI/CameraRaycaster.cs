﻿using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializeField]
    float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer currentLayerHit;

    public Layer layerHit
    {
        get { return currentLayerHit; }
    }


    public delegate void OnLayerChange(); //declare
    public OnLayerChange layerChangeObservers; //instantiate observer set





    void Start() 
    {
        viewCamera = Camera.main;
		

		layerChangeObservers ();

    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                if (currentLayerHit != layer){
					currentLayerHit = layer;
					layerChangeObservers ();
                }
                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        currentLayerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
