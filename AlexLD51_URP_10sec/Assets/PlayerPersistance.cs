using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistance : MonoBehaviour
{
    public static PlayerPersistance instance;
    public float difficulty;
    public int score;
    public int highScore;
    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
