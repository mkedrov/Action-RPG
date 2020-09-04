using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons; // arrays for the images' and text labels' links
    [SerializeField] private Text[] itemLabels;
    
    [SerializeField] private Text curItemLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;
    
    private string _curItem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Refresh()
    {
        List<string> itemList = Managers.Inventory.GetItemList();
        
        int len = itemIcons.Length;
        for (int i = 0; i < len; i++)
        {
            if (i < itemList.Count) // checking the inventory item list while watching the UI elements' images
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);
                
                string item = itemList[i];
                
                Sprite sprite = Resources.Load<Sprite>("Icons/" + item); // loading the sprite from the Resources folder
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize(); // changing the image's size to native
                
                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;
                if (item == Managers.Inventory.equippedItem)
                {
                    message = "Euipped\n" + message; // showing "Equipped" on the label over the equipped item's image
                }
                itemLabels[i].text = message;
                
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick; // turning the images into interactive objects
                entry.callback.AddListener((BaseEventData data) =>
                {
                    OnItem(item); // lambda expression, allowing to activate each element independently
                });
                
                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear(); // resetting the listeners to begin from scratch
                trigger.triggers.Add(entry); // adding a listener function to the EventTrigger class
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false); // deactivating the image/text when elements for display are absent
                itemLabels[i].gameObject.SetActive(false);
            }
        }
        
        if (!itemList.Contains(_curItem))
        {
            _curItem = null;
        }
        
        if (_curItem == null) //
        {
            curItemLabel.gameObject.SetActive(false); // deactivating the buttons when the chosen elements are absent
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            
            if (_curItem == "health") // using the 'use' button only for health element
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
            
            curItemLabel.text = _curItem + ":";
        }
    }
    
    public void OnItem(string item) // a function called by a mouse click event listener
    {
        _curItem = item;
        Refresh(); // refreshing the inventory display after the changes
    }
    
    public void OnEquip()
    {
        Managers.Inventory.EquipItem(_curItem);
        Refresh();
    }
    
    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(_curItem);
        if (_curItem == "health")
        {
            Managers.Player.ChangeHealth(25);
        }
        Refresh();
    }
    
}
