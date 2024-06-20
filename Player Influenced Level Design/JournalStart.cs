using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script asks the player what they believe happened in the first level of the game
public class JournalStart : MonoBehaviour
{
    GameObject[] lines;
    Transform lineholder;
    public static int answer;
    [SerializeField] string nextScene;

    void Start()
    {
        lineholder = GameObject.Find("LineHolder").transform;
        lines = new GameObject[lineholder.childCount];
        for (int i = 0; i < lineholder.childCount; i++)
        {
            lines[i] = lineholder.GetChild(i).gameObject;
            lines[i].SetActive(false);
        }

        lines[0].SetActive(true);
    }

    public void Dropdown1()
    {
        answer = lines[0].transform.GetChild(0).GetComponent<TMP_Dropdown>().value;

        lines[1].SetActive(true);
    }

    public void CloseJournal()
    {
        bool answered = true;
        if (answer == 0)
            answered = false;

        if (answered)
        {
            Debug.Log("LoadingNextLevel");
            SceneManager.LoadScene(nextScene);
        }

    }
}
