using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class continueScreen : MonoBehaviour
{
    PlayerPersistance persistance;
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text highScoreTxt;

    // Start is called before the first frame update
    void Start()
    {
        persistance = PlayerPersistance.instance;
        persistance.difficulty += 0.5f;
        persistance.score += 6;
        if (persistance.highScore < persistance.score) persistance.highScore = persistance.score;

        scoreTxt.SetText("score: " + (persistance.score * 100));
        highScoreTxt.SetText("high score: " + (persistance.highScore * 100));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
    }
}
