using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Dictionary<string, GameObject> uiGameObjects = new Dictionary<string, GameObject>();

    public static UIController instance = null;

	void Start ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(instance);

        PopulateUIVariables();
    }

    private void PopulateUIVariables()
    {
        string errorMsg = "Missing UI Components:";

        foreach ( var value in Enum.GetValues( typeof( UIObjects ) ) )
        {
            string s = value.ToString();

            if (this.gameObject.transform.Find(s).gameObject != null)
            {
                uiGameObjects.Add(s, transform.Find(s).gameObject);
            }
            else
            {
                errorMsg += " " + s + " :";
                return;
            }

            //Debug.Log( s + " has been added to UI." );
        }

        if(GameController.isGameDebug && errorMsg != "Missing UI Components:") Debug.LogWarning(errorMsg);
    }

    public void UpdateSpeedUI()
    {
        Vector2 speedVector = Player.instance.playerBody.velocity;
        float speed = Mathf.Round( Mathf.Abs(speedVector.x) + Mathf.Abs(speedVector.y) );

        //uiGameObjects["SpeedText"].GetComponent<Text>().text = "Speed: " + speed;
        uiGameObjects[UIObjects.SpeedText.ToString()].GetComponent<Text>().text = "Speed: " + speed;
    }

    public void UpdateHealthUI()
    {
        float health = Player.instance.playerHealth;

        //uiGameObjects["SpeedText"].GetComponent<Text>().text = "Speed: " + speed;
        uiGameObjects[UIObjects.HealthText.ToString()].GetComponent<Text>().text = "Health: " + health;
    }

    void Update ()
    {
        UpdateSpeedUI();
        UpdateHealthUI();
    }
}

enum UIObjects
{
    SpeedText,
    HealthText
}
