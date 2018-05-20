using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool isGameDebug = true;

    public static GameController instance = null;

    void Start ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad( instance );
    }
	
	void Update ()
    {
		
	}
}
