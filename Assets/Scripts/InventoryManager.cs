using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}
    
    private Dictionary<string, int> _items; // Dictionary of string keys and integer values instead of a List
    public string equippedItem {get; private set;}
    
    private NetworkService _network;
    
    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory manager starting...");
        
        _network = service;
        
        _items = new Dictionary<string, int>(); // initializing an empty item Dictionary
        
        status = ManagerStatus.Started;
    }
    
    public void UpdateData(Dictionary<string, int> items)
    {
        _items = items;
    }
    
    public Dictionary<string, int> GetData() // a data getting function is needed for saving game's data
    {
        return _items;
    }
    
    private void DisplayItems() // displaying the current inventory item list
    {
        string itemDisplay = "Items: ";
        foreach (KeyValuePair<string, int> item in _items)
        {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }
    
    public void AddItem(string name) // method can be called from outer scripts
    {
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }
        DisplayItems();
    }
    
    public List<string> GetItemList() // return list containing all the dictionary's keys
    {
        List<string> list = new List<string>(_items.Keys);
        return list;
    }
    
    public int GetItemCount(string name) // return a part. item's count
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }
    
    public bool EquipItem(string name)
    {
        if (_items.ContainsKey(name) && equippedItem != name) // checking if there's an item in inv. but it's not equipped yet
        {
            equippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }
        
        equippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }
    
    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            if (_items[name]==0)
            {
                _items.Remove(name);
            }
        }
        else
        {
            Debug.Log("cannot consume " + name);
            return false;
        }
        DisplayItems();
        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
