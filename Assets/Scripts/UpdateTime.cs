using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour
{
    // Start is called before the first frame update
    public Text playTime;
    // Update is called once per frame
    void Update()
    {
        float timeTaken = Mathf.Round(GameLogic.gameTime);
        playTime.text = "Time : " + timeTaken.ToString() + "s"; //Update current time to screen
    }
}
