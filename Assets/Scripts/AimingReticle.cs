using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingReticle : MonoBehaviour
{
    public bool isVisible = false;
    public float maxAlpha = 0.75f;
    public float minAlpha = 0.0f;

    public static AimingReticle instance = null;

    private SpriteRenderer reticleSprite = null;
    private LineRenderer reticleLine = null;

	void Start ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        reticleSprite = this.GetComponent<SpriteRenderer>();
        if (reticleSprite == null)
            Debug.Log("Reticle Sprite Missing");

        reticleLine = this.GetComponentInChildren<LineRenderer>();
        if (reticleLine == null)
            Debug.Log("Reticle Line Missing");
    }

    public void UpdateReticle( bool isVisible, Transform aimTarget )
    {
        float rotation = 0.0f;

        if (aimTarget.localPosition.x != 0.0f && aimTarget.localPosition.y != 0.0f)
        {
            float hypotenuse = Vector3.Distance(transform.localPosition, aimTarget.localPosition);
            float adjacent = Vector3.Distance(transform.localPosition, new Vector3(aimTarget.localPosition.x, transform.localPosition.y, 0.0f));
            rotation = Mathf.Acos(adjacent / hypotenuse) * Mathf.Rad2Deg;
        }

        rotation = CorrectRotation(rotation, aimTarget.localPosition.x, aimTarget.localPosition.y);

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);

        DrawPowerLine(isVisible, transform.position, aimTarget.position);

        Color c = reticleSprite.color;

        if (isVisible && c.a != maxAlpha)
            c.a = maxAlpha;
        else if (!isVisible)
            c.a = minAlpha;

        reticleSprite.color = c;
    }

    private void DrawPowerLine( bool isVisible, Vector3 origin, Vector3 end )
    {
        if (isVisible)
        {
            reticleLine.gameObject.SetActive(isVisible);
            Vector3[] linePoints = new Vector3[] { origin, end };
            reticleLine.SetPositions(linePoints);
        }
        else
        {
            reticleLine.gameObject.SetActive(isVisible);
        }
    }

    private float CorrectRotation( float rotation, float x, float y )
    {
        if (x == 0 && y == 0)
            rotation += 0.0f;

        else if (x < 0 && y == 0)
            rotation = 0.0f;

        else if (x < 0 && y < 0)
            rotation += 0.0f;

        else if (x == 0 && y < 0)
            rotation = 90.0f;

        else if (x > 0 && y < 0)
            rotation = 180.0f - rotation;

        else if (x > 0 && y == 0)
            rotation = 180.0f;

        else if (x > 0 && y > 0)
            rotation = 180.0f + rotation;

        else if (x == 0 && y > 0)
            rotation = 270.0f;

        else if (x < 0 && y > 0)
            rotation = 360.0f - rotation;

        return rotation;
    }

    /*private IEnumerator FadeReticle()
    {


        yield return null;
    }*/

	void Update ()
    {
		
	}
}
