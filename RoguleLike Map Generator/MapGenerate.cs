using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station
{
    public int[] connections;
    public GameObject stationObject;
    public bool[] lines;

    public Station(int[] connections, GameObject stationNode)
    {
        this.connections = connections;
        this.stationObject = stationNode;
    }
}

    //This script was written for a rogue like game, where the player traverses a metro system
public class MapGenerate : MonoBehaviour
{
    [SerializeField] int numLines = 3;
    [SerializeField] int stationsPerLine = 6;
    [SerializeField] GameObject stationPrefab;
    [SerializeField] TextAsset stationNamesList;
    public Station[] stations;
    string[] stationNames;

    // Start is called before the first frame update
    void Start()
    {
        int lastStation = 0;
        stations = new Station[numLines * stationsPerLine + 2];

        stationNames = stationNamesList.text.Split("\n");

        //Making metro lines
        for (int i = 0; i < numLines; i++)
        {
            for (int j = 0; j < stationsPerLine; j++)
            {
                //Creating each station
                int currentStation = i * stationsPerLine + j;

                GameObject newStation = Instantiate(stationPrefab, transform);
                newStation.name = i + " : " + j;

                //Picking name for station from list
                int chosenName;
                if (numLines * stationsPerLine > stationNames.Length)
                    Debug.LogError("Not ENough Names");
                else
                {
                    do
                        chosenName = Random.Range(0, stationNames.Length);
                    while (stationNames[chosenName] == null);
                    newStation.transform.GetComponentInChildren<TextMeshProUGUI>().text = stationNames[chosenName];
                    stationNames[chosenName] = null;
                }

                //Setting default values
                stations[currentStation] = new Station(null, newStation);
                stations[currentStation].connections = new int[numLines * 2];
                for (int f = 0; f < numLines * 2; f++)
                {
                    stations[currentStation].connections[f] = -1;
                }

               
                if (j == 0)
                {
                    //Moving to next row if starting a new line
                    stations[currentStation].stationObject.transform.localPosition = new Vector3((i - numLines / 2) * 100, 200, 0);
                }
                else
                {
                    //Changing location on map and making connection to previous station in line
                    stations[currentStation].stationObject.transform.localPosition = new Vector3(stations[lastStation].stationObject.transform.localPosition.x, stations[lastStation].stationObject.transform.localPosition.y - 50, 0);
                    stations[lastStation].connections[i] = currentStation;
                    stations[currentStation].connections[numLines + i] = lastStation;
                }

                //Giving each station node information about itself and its line
                stations[currentStation].lines = new bool[numLines];
                stations[currentStation].lines[i] = true;
                stations[currentStation].stationObject.GetComponent<StationNode>().thisNode = stations[currentStation];
                stations[currentStation].stationObject.GetComponent<StationNode>().master = gameObject.GetComponent<MapGenerate>();

                lastStation = currentStation;
            }
        }

        //Merging some stations
        int oldRandomLine = -1;
        int oldRandomStation = -1;
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            //Get random station
            int randomLine = oldRandomLine;
            //Avoid using the same line or "level" as previous merge
            while (oldRandomLine == randomLine)
            {
                randomLine = Random.Range(0, numLines);
            }
            int randomStation = oldRandomStation;
            while (randomStation == oldRandomStation)
            {
                randomStation = Random.Range(0, stationsPerLine);
            }

            int combinedStation = randomLine * stationsPerLine + randomStation;

            //Get coresponding station node on diferent line
            int mergedStationLine = randomLine;
            while (mergedStationLine == randomLine && stations[mergedStationLine * stationsPerLine + randomStation] != null)
            {
                mergedStationLine = Random.Range(0, numLines);
            }

            int mergedStation = mergedStationLine * stationsPerLine + randomStation;


            Debug.Log("trying to merge " + combinedStation + " " + mergedStation);

            //Attempt to merge the two station nodes
            if (stations[mergedStation] != null && stations[combinedStation] != null)
            {
                //Go over each connection to transfer all possible information
                for (int j = 0; j < stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.connections.Length; j++)
                {
                    //give combined node info from merged node
                    if (stations[mergedStation].stationObject.GetComponent<StationNode>().thisNode.connections[j] != -1 && stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.connections[j] == -1)
                    {
                        stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.connections[j] = stations[mergedStation].stationObject.GetComponent<StationNode>().thisNode.connections[j];

                        if (j < stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.connections.Length / 2)
                            stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.lines[j] = stations[mergedStation].stationObject.GetComponent<StationNode>().thisNode.lines[j];
                        else
                            stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.lines[j / 2] = stations[mergedStation].stationObject.GetComponent<StationNode>().thisNode.lines[j / 2];
                    }

                    //change info for earlier station nodes in line
                    if (j < stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.connections.Length / 2 && randomStation - 1 >= 0 && stations[mergedStation - 1] != null && stations[combinedStation - 1] != null)
                    {
                        if (stations[combinedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections[j] != -1)
                        {
                            stations[mergedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections[j] =
                            stations[combinedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections[j];
                        }

                        //Tell next node if both lines continue to it
                        bool sameLine = false;
                        for (int k = 0; k < stations[mergedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections.Length / 2; k++)
                            if (stations[mergedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections[k] != -1)
                                sameLine = !sameLine;

                        for (int k = 0; k < stations[mergedStation - 1].stationObject.GetComponent<StationNode>().thisNode.connections.Length / 2; k++)
                            if (sameLine && stations[combinedStation + 1] != null && !stations[combinedStation + 1].stationObject.GetComponent<StationNode>().thisNode.lines[k])
                                stations[combinedStation + 1].stationObject.GetComponent<StationNode>().thisNode.lines[k] =
                                stations[combinedStation].stationObject.GetComponent<StationNode>().thisNode.lines[k];


                    }
                }

                Debug.Log("merged " + stations[mergedStation].stationObject.name + " into " + stations[combinedStation].stationObject.name);

                //removing merged node
                Object.Destroy(stations[mergedStation].stationObject);
                stations[mergedStation] = null;
            }
            else
                Debug.Log("nvm");

        }

        //Adding start node
        CreateStart();

        //Adding end node
        CreateEnd();

        //Adding connections to all starting line nodes and to end node
        for (int i = 0; i < numLines; i++)
        {
            //Adding connections to start node
            if (stations[i * stationsPerLine] != null)
            {
                stations[numLines * stationsPerLine].connections[i] = i * stationsPerLine;
                stations[i * stationsPerLine].connections[i + numLines] = numLines * stationsPerLine;
            }

            //Adding connections to end node
            if (stations[(i + 1) * (stationsPerLine) - 1] != null)
            {
                stations[(i + 1) * (stationsPerLine) - 1].connections[i] = numLines * stationsPerLine + 1;
            }

            //Making start and end nodes part of every line
            stations[numLines * stationsPerLine].lines[i] = true;
            stations[numLines * stationsPerLine + 1].lines[i] = true;

            //Setting end node connections to none
            stations[numLines * stationsPerLine + 1].connections[i] = -1;
        }

        //Making connections between all nodes
        for (int i = 0; i < stations.Length; i++)
        {
            if (stations[i] != null)
                stations[i].stationObject.GetComponent<StationNode>().Connect();
        }
    }

    //Adding start node
    void CreateStart()
    {
        //Setting up node and default values
        GameObject startStation = Instantiate(stationPrefab, transform);
        startStation.name = "StartStation";
        stations[numLines * stationsPerLine] = new Station(null, startStation);
        stations[numLines * stationsPerLine].connections = new int[numLines * 2];
        stations[numLines * stationsPerLine].lines = new bool[numLines];
        stations[numLines * stationsPerLine].stationObject.transform.localPosition = new Vector2(0, 250);
        stations[numLines * stationsPerLine].stationObject.GetComponent<StationNode>().thisNode = stations[numLines * stationsPerLine];
        stations[numLines * stationsPerLine].stationObject.GetComponent<StationNode>().master = gameObject.GetComponent<MapGenerate>();
        stations[numLines * stationsPerLine].stationObject.GetComponent<StationNode>().playerPresent = true;

        //Picking name for station
        int chosenName;
        if (numLines * stationsPerLine > stationNames.Length)
            Debug.LogError("Not ENough Names");
        else
        {
            do
                chosenName = Random.Range(0, stationNames.Length);
            while (stationNames[chosenName] == null);
            startStation.transform.GetComponentInChildren<TextMeshProUGUI>().text = stationNames[chosenName];
            stationNames[chosenName] = null;
        }
    }

    //Adding end node
    void CreateEnd()
    {
        //Setting up node and default values
        GameObject endStation = Instantiate(stationPrefab, transform);
        endStation.name = "EndStation";
        stations[numLines * stationsPerLine + 1] = new Station(null, endStation);
        stations[numLines * stationsPerLine + 1].connections = new int[numLines * 2];
        stations[numLines * stationsPerLine + 1].lines = new bool[numLines];
        stations[numLines * stationsPerLine + 1].stationObject.transform.localPosition = new Vector2(0, 200 - (stationsPerLine * 50));
        stations[numLines * stationsPerLine + 1].stationObject.GetComponent<StationNode>().thisNode = stations[numLines * stationsPerLine + 1];
        stations[numLines * stationsPerLine + 1].stationObject.GetComponent<StationNode>().master = gameObject.GetComponent<MapGenerate>();

        //Picking name for station
        int chosenName;
        if (numLines * stationsPerLine > stationNames.Length)
            Debug.LogError("Not ENough Names");
        else
        {
            do
                chosenName = Random.Range(0, stationNames.Length);
            while (stationNames[chosenName] == null);
            endStation.transform.GetComponentInChildren<TextMeshProUGUI>().text = stationNames[chosenName];
            stationNames[chosenName] = null;
        }
    }
}
