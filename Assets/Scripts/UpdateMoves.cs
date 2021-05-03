using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoves : MonoBehaviour
{
    public Text numMoves;
    // Update is called once per frame
    void Update()
    {
        numMoves.text = "Moves : " + GameLogic.moveCount.ToString(); //Update current move count to screen
    }
}
