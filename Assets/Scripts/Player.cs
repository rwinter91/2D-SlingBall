using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range (0.0f, 100.0f)]
    public float launchPowerPercent = 25.0f;
    [Range(0.0f, 100.0f)]
    public float playerHealth = 100.0f;
    public Sprite debugSprite = null;
    [HideInInspector]
    public Rigidbody2D playerBody = null;

    private float launchPowerMultiplier = 200.0f;
    private bool isTrackingMouse = false;
    private Vector3 startClick;
    private GameObject aimTarget = null;

    public static Player instance = null;

	void Start ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        playerBody = GetComponent<Rigidbody2D>();
	}

    public void LaunchPlayer ( Vector2 launchForce )
    {
        playerBody.AddForce (launchForce);
    }

    private GameObject SetupTarget ( Vector2 startLocation )
    {
        Destroy (aimTarget);

        GameObject newTarget = new GameObject("AimTarget");
        newTarget.transform.SetParent(this.transform);
        newTarget.transform.position = startLocation;

        //////////////////////
        //Debug Code
        if (GameController.isGameDebug && debugSprite != null)
        {
            SpriteRenderer debugSpot = newTarget.AddComponent<SpriteRenderer>();
            debugSpot.sprite = debugSprite;
            debugSpot.sortingLayerName = "Debug";
        }
        //Debug Code
        //////////////////////

        return newTarget;
    }

    private void CalculateNewTrajectory ()
    {
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchVector = (Vector2) (this.transform.position - currentMousePos);
        launchVector = Vector2.ClampMagnitude( launchVector, 3.0f);

        aimTarget.transform.position = this.transform.position + (Vector3)launchVector;

        launchVector.x *= (launchPowerPercent / 100.0f) * launchPowerMultiplier;
        launchVector.y *= (launchPowerPercent / 100.0f) * launchPowerMultiplier;

        AimingReticle.instance.UpdateReticle(true, aimTarget.transform);

        if (Input.GetButtonUp("Fire1"))
        {
            LaunchPlayer(launchVector);
            AimingReticle.instance.UpdateReticle(false, aimTarget.transform);
            isTrackingMouse = false;
        }
    }

	void Update ()
    {
        if (Input.GetButtonDown("Fire1") && !isTrackingMouse)
        {
            //startClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //startClick = this.transform;
            aimTarget = SetupTarget( this.transform.position );
            isTrackingMouse = true;
        }

        if (isTrackingMouse)
        {
            CalculateNewTrajectory();
        }
    }
}
