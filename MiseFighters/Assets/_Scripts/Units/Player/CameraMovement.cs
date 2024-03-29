using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;

public class CameraMovement : NetworkBehaviour 
{
    private Vector3 velocity = Vector3.zero;
    // maybe store this somewhere so its not hardcoded
    private readonly float smoothTime = 0.50f;
    
    private Camera playerCamera;
    public override void OnStartClient()
    {
        base.OnStartClient();
        SceneManager.OnLoadEnd += AssignCamera;
    }

    private void AssignCamera(FishNet.Managing.Scened.SceneLoadEndEventArgs obj)
    {
        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.orthographic = true;

            playerCamera.orthographicSize = 6.5f;
        }
        else
        {
            return;
        }
    }

    private void Update()
    {
        // Nullchecks just to account for delays, should find a proper fix for this
        if (transform == null || playerCamera == null) return;

        Vector3 targetPosition = transform.position + new Vector3(0, 0, -10);
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, targetPosition, ref velocity, smoothTime);

    }

}
