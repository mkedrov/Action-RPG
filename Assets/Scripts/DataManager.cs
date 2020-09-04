using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}
    
    private string _filename;
    
    private NetworkService _network;
    
    public void Startup(NetworkService service)
    {
        Debug.Log("Data manager starting... ");
        
        _network = service;
        
        _filename = Path.Combine(Application.persistentDataPath, "game.dat"); // generating a complete path to the "game.dat" file
        status = ManagerStatus.Started;
    }
    
    public void SaveGameState()
    {
        Dictionary<string, object> gamestate = new Dictionary<string, object>(); // a dictionary that will be serialized
        gamestate.Add("inventory", Managers.Inventory.GetData());
        gamestate.Add("health", Managers.Player.health);
        gamestate.Add("maxHealth", Managers.Player.maxHealth);
        gamestate.Add("curLevel", Managers.Mission.curLevel);
        gamestate.Add("maxLevel", Managers.Mission.maxLevel);
        
        FileStream stream = File.Create(_filename); // creating a file in the _filename's path
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate); // serializing the Dictionary object as a newly created file
        stream.Close();
    }
    
    public void LoadGameState()
    {
        if (!File.Exists(_filename))
        {
            Debug.Log("No saved game");
            return;
        }
        Dictionary<string, object> gamestate; // a Dictionary for data loaded
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(_filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        
        // updating managers and filling them with deserialized data
        Managers.Inventory.UpdateData((Dictionary<string, int>)gamestate["inventory"]);
        Managers.Player.UpdateData((int)gamestate["health"], (int)gamestate["maxHealth"]);
        Managers.Mission.UpdateData((int)gamestate["curLevel"], (int)gamestate["maxLevel"]);
        Managers.Mission.RestartCurrent();
    }
}
