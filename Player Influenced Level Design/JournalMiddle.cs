using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script asks the player what they believe happened in the second level of the game
public class JournalMiddle : MonoBehaviour
{
    GameObject[] lines;
    Transform lineholder;
    public static int[] answers;
    int lineOffset = 25;
    [SerializeField] string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        lineholder = GameObject.Find("LineHolder").transform;
        lines = new GameObject[lineholder.childCount];
        answers = new int[2];
        for (int i = 0; i < lineholder.childCount; i++)
        {
            lines[i] = lineholder.GetChild(i).gameObject;
            lines[i].SetActive(false);
        }

        lines[0].SetActive(true);
    }

    public void Dropdown1()
    {
        answers[0] = lines[0].transform.GetChild(0).GetComponent<TMP_Dropdown>().value;

        if (answers[0] == 2)
        {
            lines[1].SetActive(true);
            lines[1].transform.position = new Vector3(lines[1].transform.position.x, lines[0].transform.position.y - lineOffset);
        }
        else
        {
            lines[1].SetActive(false);
            lines[2].SetActive(true);
        }
    }

    public void Dropdown2()
    {
        answers[1] = lines[1].transform.GetChild(0).GetComponent<TMP_Dropdown>().value;
        lines[2].SetActive(true);
    }

    public void CloseJournal()
    {
        bool answered = true;
        for (int i = 0; i < answers.Length; i++)
        {
            if (lines[i].activeSelf && answers[i] == 0)
                answered = false;
        }

        if (answered)
        {
            Debug.Log("LoadingNextLevel");
            SceneManager.LoadScene(nextScene);
        }

    }
}
