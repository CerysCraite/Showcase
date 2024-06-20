using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is present in each level and is responsible for changing which variant of the level is shown, dependant on the player's previous choices
public class CaseObjectSwitch : MonoBehaviour
{
    //All objects that appear for this case
    [SerializeField] GameObject[] objectPlace;

    //All objects taht do not appear in this case
    [SerializeField] GameObject[] objectRemove;

    private void Awake()
    {
        //makes sure all case specific objects are not enabled
        foreach (GameObject obj in objectPlace)
        {          
            obj.SetActive(false);
        }
    }

    //Hides all unneeded objects for this case and enables the needed ones
    public void ExchangeObjects()
    {
        foreach(GameObject obj in objectPlace)
        {
            Debug.Log(obj.name + " added");
            obj.SetActive(true);
        }

        foreach (GameObject obj in objectRemove)
        {
            Debug.Log(obj.name + " removed");
            obj.SetActive(false);
        }
    }
}
