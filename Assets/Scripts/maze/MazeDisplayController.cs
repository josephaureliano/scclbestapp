﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * This script:
 * Generates 25 tiles for the game
 * Generates turning points or "nodes" so that the player gets a randomised path each time he/she plays
 * Generates the starting and ending bridges at fixed points. If changes to length of sentence(currently 12) are made, the positions of the bridges and the number of nodes may have to change
 * Currently, the tiles are "Destroy() ed" after each round. We recognise this is undesirable due to large memory used. Due to time constraint, we have yet to implement an object pool. It would greatly optimise the game and speed it up. 
 * 
 */
public class MazeDisplayController : MonoBehaviour {
    private int length = 12;
    private List<GameObject> spaces;
    private List<Vector2> correctNodes;
    private List<GameObject> correctSpaces;
    public GameObject space;
    public GameObject bridge;
    public GameObject endBridge;
    public GameObject xmas;
    public string[] corSentence = new string[12];
    public string[] randSentence = new string[25];
    private MazeDataController dataController;
    private OverallGameManager ogm;
	private GameObject runtimeXmas;

    void Start()
    {
        dataController = GetComponent<MazeDataController>();
        ogm = GetComponent<OverallGameManager>();
		//StartNewRound ();
    }
   
	public void StartNewRound(){
		dataController.Start();
		Refresh();
	}
    public void Refresh()
    {
        ReceiveData(dataController.corSentence, dataController.randSentence);
        Debug.Log(corSentence[0]);
        if (corSentence == null || randSentence == null)
        {
            return;
        }
        else
        {
            InstantiateTiles();
            correctNodes = GetNodes(12);
            GenerateBridge(MatchAndGenerate(correctNodes));
        }
    }
    void Update()
    {


        
    }

    public void ReceiveData(List<string> corSentence, List<string> randSentence)
    {
        Debug.Log("receiving data");
        this.corSentence = new string[12];
        this.randSentence = new string[25];
        int count1 = 0;
        int count2 = 0;
        foreach (string chara in corSentence)
        {
            this.corSentence[count1] = chara;
            Debug.Log("chara= " + this.corSentence[count1]);
            count1++;
        }
        foreach (string chara in randSentence)
        {
            this.randSentence[count2] = chara;
            count2++;
        }
    }
    private void InstantiateTiles()
    {
        spaces = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject tempGo = Instantiate(space);
                spaces.Add(tempGo);
                tempGo.transform.position = new Vector3(-1.04f * j, 0, 1.04f * i);
                tempGo.SetActive(true);
                tempGo.GetComponentInChildren<TextMesh>().text = randSentence[5 * i + j];
            }
        }
    }
    private List<Vector2> GetNodes(int number)
    {
        int row = 0;
        int col = 2;
        Vector2 newNode = new Vector2(row, col);
        List<Vector2> nodes = new List<Vector2>();
        nodes.Add(newNode);

        while (row < 5)
        {
            int noOfSteps = 0;
            int direction = 999;//0 forward. -1 left. 1 right.
            while (direction != 0)
            {
                if (direction > 4)
                {//Toss a five-sided coin to decide where to go[first time only]
                    int coin = Random.Range(0, 5);//0 forward, 1,2 left, 3,4 right
                    if(coin > 3)
                    {
                        direction = 1;
                    }
                    if (coin >1 && coin < 4)
                    {
                        direction = -1;
                    }
                    if (coin == 1)
                    {
                        direction = 0;
                    }
                   /* switch (coin)
                    {
                        case 0:
                            direction = 0;
                            break;
                        case 1:
                            direction = -1;
                            break;
                        case 2:
                            direction = -1;
                            break;
                        case 3:
                            direction = 1;
                            break;
                        case 4:
                            direction = 1;
                            break;
                    }*/
                }

                if ((col + direction) > 4 || (col + direction) < 0)
                {//decide whether moving horizontally is possible
                    direction = 0;
                }
                float anotherCoin = Random.Range(0f, 1f);
                // decide whether to move horizontally
                if (anotherCoin > 0.3f)
                {

                }
                else
                {
                    direction = 0;
                }

                if (noOfSteps >= 4)
                {// decide whether Xmas has been at this row for too long
                    direction = 0;
                }

                if (direction != 0)
                {
                    col += direction;
                    noOfSteps += 1;
                    newNode = new Vector2(row, col);
                    nodes.Add(newNode);
                }

            }

            row += 1;
            newNode = new Vector2(row, col);
            if (row < 5)
            {
                nodes.Add(newNode);
            }
        }

        foreach (Vector2 thisNode in nodes)
        {
        }
        //Debug.Log("------------" + nodes.Count + "-----------------");
        while(nodes.Count != number)
        {
            nodes = GetNodes(number);
        }
        
        return nodes;
    }

    private List<Vector3> MatchAndGenerate(List<Vector2> positions)
    {
        List<Vector3> startNEnd = new List<Vector3>();// records the starting and ending positions of tiles;
        int count = 1;
        for(int i = 0; i < positions.Count; i++) {
            spaces[(int)positions[i].x + ((int)positions[i].y * 5)].GetComponent<MazeTileController>().serialNumber = count;
            spaces[(int)positions[i].x + ((int)positions[i].y * 5)].GetComponentInChildren<TextMesh>().text = corSentence[i]; 
            count++;
        }

        startNEnd.Add(spaces[(int)positions[0].x + ((int)positions[0].y * 5)].transform.position);
        startNEnd.Add(spaces[(int)positions[length - 1].x + ((int)positions[length - 1].y * 5)].transform.position);
        return startNEnd;
    }

    private void GenerateBridge(List<Vector3> adjacentTiles)
    {
        GameObject tempBridge = new GameObject();
        tempBridge = Instantiate(bridge);
        tempBridge.SetActive(true);
        tempBridge.transform.position = new Vector3(2.67f, -0.47f, adjacentTiles[0].z-0.18f);
        GameObject tempXmas = new GameObject();
        tempXmas = Instantiate(xmas);
		runtimeXmas = tempXmas;
        tempXmas.SetActive(true);
        tempXmas.GetComponent<NavMeshAgent>().enabled = false ;
        tempXmas.transform.position = new Vector3(1.24f, 0.019f, 2.013f);
        tempXmas.GetComponent<NavMeshAgent>().enabled = true;
        GameObject tempEnd = new GameObject();
        tempEnd = Instantiate(endBridge);
        tempEnd.transform.position = new Vector3(-6.68f, -0.7f, adjacentTiles[1].z + 0.18f);
        tempEnd.transform.rotation = Quaternion.Euler(0f, 0f , 0f);
        Debug.Log(tempEnd.name);
    }

    private void ShowMessageBox(bool win)
    {
        Debug.Log("message box shown");



    }
   
    public void RoundEnd(bool win)
    {
		foreach (CapsuleCollider cc in runtimeXmas.GetComponents<CapsuleCollider>()) {
			cc.enabled = false;
		}
    }

    public void Annihilte()
    {
        foreach(MazeTileController tile in FindObjectsOfType<MazeTileController>())
        {
            Destroy(tile.gameObject);
        }
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("regenerate"))
        {
            Destroy(obj);
        }
    }

    public void Repeat()
    {
        Annihilte();
        Refresh();
    }

    public void NewRound()
    {
        Annihilte();
        Refresh();
    }

	public IEnumerator RevealAnswer(){
		foreach (GameObject go in spaces) {
			if (go.GetComponent<MazeTileController> ().serialNumber == 0) {
                go.GetComponentInChildren<TextMesh>().text = "";
				go.GetComponentInChildren<Rigidbody> ().useGravity = true;
                yield return new WaitForSeconds(0.05f);
				
			}

		}
        yield return new WaitForSeconds(1f);

        foreach (GameObject go in spaces)
        {
            if(go.GetComponent<MazeTileController>().serialNumber == 0)
            {
                go.SetActive(false);
            }
        }
	}
}
