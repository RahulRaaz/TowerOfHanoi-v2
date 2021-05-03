using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHideError : MonoBehaviour
{
    bool previousState = false;
    float ticker = 0.0f;
    float toggleTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        ticker += Time.deltaTime;
        bool currentState = gameObject.activeSelf;
        if (currentState && !previousState)
        {
            toggleTime = ticker;
            previousState = true;
        }
        else if (currentState && (ticker - toggleTime) >= 1.0) //check if 1s has elapsed since messages enabled
        {
            previousState = false;
            gameObject.SetActive(false);
        }
        else
            return;
    }
}
