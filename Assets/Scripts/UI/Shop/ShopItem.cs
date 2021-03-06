using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/**
 * Class containing the data of an individual item sold in the shop,
 * as well as any behaviour for the animation and display of the item.
 * 
 * This Script is attached to the button prefab
 * 
 * Each button handles its own onClick event and notifies the Controllers of what is happening
 */
public class ShopItem : MonoBehaviour
{
    public int cost;
	public string fullName;
	public string englishName;
	public string description;
	public Sprite itemSprite;
	public Sprite selectedSprite;
	public Sprite unselectedSprite;
	public bool isBuyable;
	public bool isOnSale;
	public bool isSelected;
	private Button itemButton;
	private ShopItem shopItem;
	private Text nameText;

    public void Initialize()
    {
        isSelected = false;
        itemButton = GetComponent<Button>();
        shopItem = GetComponent<ShopItem>();
		nameText = GetComponentInChildren<Text> ();
        itemButton.gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = itemSprite;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => { SelectItem(); });
		nameText.text = "";
		nameText.text = shopItem.fullName;

    }

	public void SelectItem(){
		SendMessageUpwards ("SelectItem_Master", shopItem);
	}

	public void SetSelected(){
		Debug.Log (fullName + " is selected");
		itemButton.GetComponent<Image> ().sprite = selectedSprite;
		isSelected = true;
	}

	public void SetUnselected(){
		itemButton.GetComponent<Image> ().sprite = unselectedSprite;
		nameText.text = "";
		isSelected = false;
	}

	public void SetDisabled(){
		itemButton.onClick.RemoveAllListeners ();
		nameText.text = "";
		Debug.Log ("One item has been disabled");
	}
}
