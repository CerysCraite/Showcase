using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script asks the player what they believe happened in the last level of the game
public class JournalEnd : MonoBehaviour
{
    [SerializeField] string nextScene;
    [SerializeField] GameObject[] cases;

    void Start()
    {
        foreach (GameObject text in cases) { text.SetActive(false); }

        //breakdown was caused by a lack of maintenance
        if (Journal.answers[0] == 1)
        {
            //dam machiery is broken and needs to be repaired
            cases[0].SetActive(true);
        }

        //breakdown was caused by an attack
        else if (Journal.answers[0] == 2)
        {
            //attack was by animals
            if (Journal.answers[1] == 1)
            {
                //wild dogs which need to be chased away
                cases[1].SetActive(true);
            }

            //attack was by soldiers
            else if (Journal.answers[1] == 2)
            {
                //soldiers which need to be avoided
                cases[2].SetActive(true);
            }
        }

        //breakdown was caused by sabotage
        else if (Journal.answers[0] == 3)
        {
            //entry to dam is blocked off and must be cleared - barricades to stop protesters
            cases[3].SetActive(true);
        }
    }

    public void CloseJournal()
    {
        Debug.Log("LoadingNextLevel");
        SceneManager.LoadScene(nextScene);

    }
}
