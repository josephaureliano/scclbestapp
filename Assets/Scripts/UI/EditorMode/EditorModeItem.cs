﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorModeItem : MonoBehaviour {

	public int cost;
	public string fullName;
	public string englishName;
	public Sprite itemSprite;
	public Sprite selectedSprite;
	public Sprite unselectedSprite;
	public bool isBuyable;
	public bool isOnSale;
	public bool isSelected;
    public GameObject furniture;
	private Button itemButton;
	private EditorModeItem editorModeItem;
    public Vector3 position;
    public Vector3 rotation;

    public void Initialize(){
		itemButton = GetComponent<Button> ();
		editorModeItem = GetComponent<EditorModeItem> ();
		itemButton.gameObject.transform.GetChild(0).GetComponentInChildren<Image> ().sprite = itemSprite;
		itemButton.onClick.RemoveAllListeners ();
		itemButton.onClick.AddListener (()=>{SelectItem();});
		if (isSelected) {
			isSelected = false;
			SelectItem ();
			//Debug.Log ("Setting this item as equipped: " + fullName);
		} else {
			//Debug.Log ("did not set this item as equipped: " + fullName);
		}
	}

	public void SelectItem(){
		SendMessageUpwards ("SelectItem_Master", editorModeItem);
	}

	public void SetSelected(){
		Debug.Log (fullName + " is selected");
		itemButton.GetComponent<Image> ().sprite = selectedSprite;
		isSelected = true;
		furniture.SetActive (true);
	}

	public void SetUnselected(){
		itemButton.GetComponent<Image> ().sprite = unselectedSprite;
		if (furniture)
			furniture.gameObject.SetActive (false);
		isSelected = false;
	}

	public void SetDisabled(){
		itemButton.onClick.RemoveAllListeners ();
		Debug.Log ("One item has been disabled: " + fullName);
	}

	public void Restart(){
	}
	public void Shutdown(){
	}
}
	
