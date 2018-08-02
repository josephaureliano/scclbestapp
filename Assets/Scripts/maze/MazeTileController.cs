using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * This script:
 * One of the most important scripts. 
 * Determines whether a player/the NPC(goose) steps onto a correct tile
 * A correct tile is defined as the tile containing the word in the correct sentence in exactly the order that appears in the sentence
 * For example, if A appears as the fourth character in the correct sentence, the tile is only considered correct if it is the fourth tile that the player or NPC steps on.
 * This is achieved by the serial number which tracks the progress of the player/NPC
 * Stepping onto the wrong tile causes gravity to be turned on and player/NPC to drop with the tile.
 * When the player reaches the largest serial number, the game ends and the player wins
 * To add to the fun, correct tile turns from barren land to fertile land once stepped on. 
 */
public class MazeTileController : MonoBehaviour {
    public int serialNumber = 0;
    private Rigidbody rgdBody;
    private Renderer rend;
    private NavMeshObstacle obs;
    private MazeDataController dataController;
    private OverallGameManager ogm;
    private GameObject gameManager;
    private GameObject gorakutile;

    private AudioController ogms;
    //public GameObject trees; 
	// Use this for initialization
	void Start () {
        rgdBody = GetComponentInChildren<Rigidbody>();
        gorakutile = GetComponentInChildren<Identifier>().gameObject;
        gameManager = GameObject.Find("GameController");

        gorakutile.SetActive(false);
        rend = GetComponentInChildren<Renderer>();
       // rgdBody.useGravity = false;
        rgdBody.isKinematic = false;
        obs = GetComponent<NavMeshObstacle>();
        obs.enabled = false;
        dataController = gameManager.GetComponent<MazeDataController>();
        ogm = gameManager.GetComponent<OverallGameManager>();
        ogms = gameManager.GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private IEnumerator OnTriggerEnter(Collider other)
    {
        Debug.Log("name is " + other.name);
        if (serialNumber == 0)
        {
            yield return new WaitForSeconds(0.3f);
            rend.material.color = Color.red;
            rgdBody.useGravity = true;
            obs.enabled = true;
            Debug.Log(other.ClosestPointOnBounds(transform.position).z);
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<Rigidbody>().useGravity = true;
            rgdBody.detectCollisions = false;
            ogms.PlaySound(false);
            if (other.name == "Mas1(Clone)")
            {
                ogm.SendMessage("Result", false);
            }
            yield return new WaitForSeconds(5f);
           
        }
        if (other.name == "Mas1(Clone)")
        {
            if (other.GetComponent<ModelInfo>().count < serialNumber)
            {
                yield return new WaitForSeconds(0.3f);
                rend.material.color = Color.red;
                rgdBody.useGravity = true;
                obs.enabled = true;
                Debug.Log(other.ClosestPointOnBounds(transform.position).z);
                other.GetComponent<NavMeshAgent>().enabled = false;
                other.GetComponent<Rigidbody>().useGravity = true;
                ogms.PlaySound(false);
                ogm.SendMessage("Result", false);
                rgdBody.detectCollisions = false;
                yield return new WaitForSeconds(5f);
            }
            else if(other.GetComponent<ModelInfo>().count == serialNumber)
            {
                 other.GetComponent<ModelInfo>().count = serialNumber + 1;
                 Debug.Log("Xmas count is equal to: " + other.GetComponent<ModelInfo>().count);
                rgdBody.gameObject.SetActive(false);
                gorakutile.SetActive(true);
                ogms.PlaySound(true);
				//Instantiate(trees, this.transform);
                //trees.SetActive(true);
				//trees.transform.SetPositionAndRotation (new Vector3 ((this.transform.position.x + 0.196f), 0f, (this.transform.position.z)), trees.transform.rotation);
				//trees.transform.position = new Vector3(trees.transform.position.x - 0.196f, 0f, trees.transform.position.z + 0.879f);
				//Debug.Log("tree position" + serialNumber + trees.transform.position + "tile position" + this.gameObject.transform.position);
                 if(serialNumber == dataController.length)
                {
                    ogm.SendMessage("Result", true);
                  
                }
            }
        }
        //rgdBody.useGravity = false;
    }


}
