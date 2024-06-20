using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This script presents a full overview of the story, according to what events the player thought took place and their actions
public class Epilogue : MonoBehaviour
{
    public static int ending;
    string epilogue;

    void Start()
    {
        Debug.Log(JournalStart.answer + " " + Journal.answers[0] + " " + Journal.answers[1] + " " + ending);

        //Mine Disrepair cause
        if(JournalStart.answer == 1)
        {
            epilogue += "They dammed off the river to mine resources from the lakeside, but they abandoned it as soon as the resources dried up. ";

            //Powerplant disrepair
            if (Journal.answers[0] == 1)
            {
                epilogue += "And as soon the mine wasn't making money there was no insentive to keep the plant and dam in working order. ";
            }

            //Animal attack
            if (Journal.answers[0] == 2 && Journal.answers[1] == 1)
            {
                epilogue += "Then nature just retook what the people had left behind. ";
            }

            //Soldier attack
            else if (Journal.answers[0] == 2 && Journal.answers[1] == 2)
            {
                epilogue += "Soon after some armed bandits came to loot whatever was left behind. ";
            }

            //Powerplant sabotage
            if (Journal.answers[0] == 3)
            {
                epilogue += "Perhaps somebody was not happy with the mine being active and tried to sabotage the plant to stop it. ";
            }
        }

        //Sandstorm importance
        else if(JournalStart.answer == 2)
        {
            epilogue += "A storm burried everything under sand and forced everyone to leave. ";

            //Powerplant disrepair
            if (Journal.answers[0] == 1)
            {
                epilogue += "The storm probably did too much damage for repairs to be worth it so they just gave up. ";
            }

            //Animal attack
            if (Journal.answers[0] == 2 && Journal.answers[1] == 1)
            {
                epilogue += "Wild dogs attacking also didn't help. Kinda seems like nature didn't want them here. ";
            }

            //Soldier attack
            else if (Journal.answers[0] == 2 && Journal.answers[1] == 2)
            {
                epilogue += "Soldiers attacking the staff at the same time is weird, but perhaps not entirely coincidental. ";
            }

            //Powerplant sabotage
            if (Journal.answers[0] == 3)
            {
                epilogue += "Whoever sabotaged the plant could have used the cover of the storm to explain the breakdown. ";
            }
        }

        //Mine Sabotage extent
        else if(JournalStart.answer == 3)
        {
            epilogue += "The cableway was shut off and the gate welded shut. Whoever did it did not intend for the mine to restart work. ";

            //Powerplant disrepair
            if (Journal.answers[0] == 1)
            {
                epilogue += "There was probably not enough pressure to reopen the mine and with no demand the plant and dam were not maintained. ";
            }

            //Animal attack
            if (Journal.answers[0] == 2 && Journal.answers[1] == 1)
            {
                epilogue += "The animals attacking is probably a coincidence, but I can't help but wonder if it was somehow instigated by the saboteur. ";
            }

            //Soldier attack
            else if (Journal.answers[0] == 2 && Journal.answers[1] == 2)
            {
                epilogue += "Maybe the bandits had an insider who sabotaged the mine to create confusion so they could take over easier. ";
            }

            //Powerplant sabotage
            if (Journal.answers[0] == 3)
            {
                epilogue += "There were a lot of people, unhappy with the construction of the dam and the mining, so its possible they tried to prevent their opening. ";
            }
        }

        //Floodgates opened
        if(ending == 0)
        {
            epilogue += "\nI managed to fix the mechanism and reopen the floodgates, so hopefuly water will flow the lake bottom once again. ";
        }

        //Floodgates destroyed
        else if(ending == 1)
        {
            epilogue += "\nI made sure the gates could never be closed again by destroying them, hopefuly at least some part of the lake is restored. ";
        }

        //Dam destroyed
        else if(ending == 2)
        {
            epilogue += "\nI destroyed the dam to let the river refill the lake again and undo the harm people caused by damming it off. ";
        }

        GameObject.Find("EpilogueText").GetComponent<TextMeshProUGUI>().text = epilogue;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
