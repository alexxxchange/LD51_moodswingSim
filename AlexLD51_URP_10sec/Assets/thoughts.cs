using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class thoughts : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] positiveThoughts;
    [SerializeField] string[] negativeThoughts;
    [SerializeField] TMP_Text text;
    int positiveThoughtIndex;
    int negativeThoughtIndex;

    public void DisplayPositiveThought()
    {
        positiveThoughtIndex++;
        if (positiveThoughtIndex >= positiveThoughts.Length - 1) positiveThoughtIndex = 0;
        text.SetText("current thought: " + positiveThoughts[positiveThoughtIndex]);
    }

    public void DisplayNegativeThought()
    {
        negativeThoughtIndex++;
        if (negativeThoughtIndex  >= negativeThoughts.Length - 1) negativeThoughtIndex = 0;
        text.SetText("current thought: " + negativeThoughts[negativeThoughtIndex]);
    }

}
