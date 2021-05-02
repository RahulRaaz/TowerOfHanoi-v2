using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLogic : MonoBehaviour
{
    //initalizing multi function variables
    List<GameObject> allDiscs = new List<GameObject>(); //Hold All disks
    List<GameObject> rodADisk = new List<GameObject>(); //Hold disks in A rod
    List<GameObject> rodBDisk = new List<GameObject>(); //Hold disks in A rod
    List<GameObject> rodCDisk = new List<GameObject>(); //Hold disks in A rod
    Vector3 currentLocation = new Vector3();
    Vector3 finalLocation = new Vector3();
    public int moveCount = 0;  //Game score calculation using scores
    public float gameTime = 0.0f; //Time taken to complete game
    bool diskMoving = false;
    bool diskLifting = false;
    bool diskSelected = false;
    GameObject activeDisk = null;
    float liftHeight = 0;

    void Start()
    {
        //Get Details of all Disks here
        allDiscs.AddRange(GameObject.FindGameObjectsWithTag("AllDisks"));
        updateLayerGroups();
        checkRodHeight();
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        if (Time.timeScale == 0) //check for pause and disk moving cases
            return;
        else if(diskMoving || diskLifting) //if disk moving, go to disk move option
            smoothMove();
        else if (Input.GetMouseButtonUp(0)) //check for user input
            selectObject();
        else //check for game completion
            checkGameOver();
   }

    void selectObject() //identify clicked object
    {
        GameObject selection;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform != null)
            selection = hit.transform.gameObject;
        else
        {
            errMessage("Please select the disk or rod");
            return;
        }
        identifySelection(selection);
        //print(currentSelection.layer);
        //check for layer of current selection and run appropriate function
    }

    void identifySelection(GameObject currentSelection) //decide on next action based on selected object
    {
        if (currentSelection.tag == "AllDisks" && !diskSelected) //only if no disks are already selected
        {
            //function for disk actions
            activeDisk = currentSelection;
            pickDisk();
        }
        else if (diskSelected && currentSelection.layer == 6) //checks if move is to place selected disk
            selectDestination(currentSelection);
        else
        {
            errMessage("select disk first then select rod to place it in");
            return;
        }
    }

    void selectDestination(GameObject destination) //check destination disk for legal move
    {
        //compare tags to identify rod
        string rodTag = destination.tag;
        //print ("tag = " + rodTag);
        List<GameObject> activeRod;
        int tempLayer = 0;
        if (rodTag == "RodA") //check if ternary operator can be used
        {
            activeRod = rodADisk;
            tempLayer = 7;
        }
        else if (rodTag == "RodB")
        {
            activeRod = rodBDisk;
            tempLayer = 8;
        }
        else if (rodTag == "RodC")
        {
            activeRod = rodCDisk;
            tempLayer = 9;
        }
        else
        {
            errMessage("Unknown error, try again");
            return;
        }
        //Recognize the rod selected to drop by click
        if (checkIfSmaller(activeRod))
        {
            moveToDestination(destination);
            activeDisk.layer = tempLayer;
        }
        else
        {
            errMessage("unable to place here");
            return;
        }
    }

    void moveToDestination(GameObject destination) //move selected disk to new rod location
    {
        Vector3 destLocation = destination.transform.position;
        currentLocation = activeDisk.transform.position;
        finalLocation = new Vector3(destLocation.x, currentLocation.y, destLocation.z);
        diskMoving = true;
        moveCount++;
    }

    bool checkIfSmaller(List<GameObject> disksInNewRod) //check if selected disk is smaller than disks in destination
    {
        //check if the disk to drop is smaller than disks at the destination 
        bool smaller = true;
        float sizeActiveDisk = checkSizeObj(activeDisk);
        foreach (GameObject disk in disksInNewRod)
            if (sizeActiveDisk > checkSizeObj(disk))
            {
                smaller = false;
                break;
            }
            else
                continue;
        return smaller;
    }

    void pickDisk() //process selected disk for legal move
    {
        //get disk rod group
        int diskLayer = activeDisk.layer;
        List<GameObject> currRod = new List<GameObject>(); //Hold disks in rod for processing
        if (diskLayer == 7)
            currRod = rodADisk;
        else if (diskLayer == 8)
            currRod = rodBDisk;
        else if (diskLayer == 9)
            currRod = rodCDisk;
        else
        {
            errMessage("Error in disk selection Try again");
            return;
        }
        if (checkIfTop(currRod))
            //Lift disk and process
            liftDisk();
        else
            errMessage("Disk is not on the top, please try again");
    }

    bool checkIfTop(List<GameObject> disksInRod)
    {
        //check if the disk selected is at the top of the rod then run moveUp();
        float activeDiskPos = activeDisk.transform.position.y;
        bool isTop = true;
        foreach (GameObject disk in disksInRod)
        {
            float comparePos = disk.transform.position.y;
            if (activeDiskPos < comparePos)
            {
                isTop = false;
                break;
            }
            else
                continue;
        }
        return isTop;
    }

    void liftDisk() //set raised destination for the disk
    {
        //Move selected disk upwards to a higher point than columns
        currentLocation = activeDisk.transform.position;
        finalLocation = new Vector3(currentLocation.x, liftHeight, currentLocation.z); //change to final lift location
        diskLifting = true;
        diskSelected = true;
        activeDisk.layer = 0;
        updateLayerGroups(); 
    }

    void smoothMove() //move smoothly to destination or reset movement params
    {
        //move disk smoothly to destination
        float moveSpeed = 20.0f;//standard speed of disk movement
        float step = moveSpeed * Time.deltaTime;
        if(currentLocation != finalLocation)
        {
            activeDisk.transform.position = Vector3.MoveTowards(currentLocation, finalLocation, step);
            currentLocation = activeDisk.transform.position;
        }
        else if (diskLifting)
            diskLifting = false;
        else if (diskMoving)
        {
            //reset the moving, disk selected and active disk variables and enable gravity for selected object 
            diskMoving = false;
            setGravity(true);
            activeDisk = null;
            diskSelected = false;
            updateLayerGroups();
        }
        else
            errMessage("Unknown error, reset recommended");
    }

    void updateLayerGroups() //update the layer lists on startup and on 
    {
        clearLayerGroups();
        foreach (GameObject disk in allDiscs)
        {
            if (disk.layer == 7)
                rodADisk.Add(disk);
            else if (disk.layer == 8)
                rodBDisk.Add(disk);
            else if (disk.layer == 9)
                rodCDisk.Add(disk);
            else
                errMessage(disk + "not on any rod/moving"); //or continue;
        }
    }

    void clearLayerGroups() //clear all disk layer lists 
    {
        print("clearing disk layers");
        rodADisk.Clear();
        rodBDisk.Clear();
        rodCDisk.Clear();
    }

    void checkRodHeight() //check rod height for lifting
    {
        GameObject refRod = GameObject.FindGameObjectsWithTag("RodA")[0];
        liftHeight = (refRod.transform.position.y + (checkSizeObj(refRod)) / 1.5f);
    }

    float checkSizeObj(GameObject objToCheck) //check size of any disks to compare
    {
        //Find and return the size of the object to compare
        Collider objCollider = objToCheck.GetComponent<Collider>();
        float size = objCollider.bounds.size.y;
        return size;
    }

    void setGravity(bool toggle) //set default and gravity and kinematic to disk
    {
        activeDisk.GetComponent<Rigidbody>().useGravity = toggle;
        activeDisk.GetComponent<Rigidbody>().isKinematic = !toggle;
        StartCoroutine("wait");
    }

    void errMessage(string msg) //print error message to screen
    {
        //print error message to UI
        print("error :" + msg);
    }

    void checkGameOver() // end game if all disks in single rod
    {
        if (rodBDisk.Count == 5 || rodCDisk.Count == 5)
            //endGame
            print("GameOver");
        else
            return;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }

}