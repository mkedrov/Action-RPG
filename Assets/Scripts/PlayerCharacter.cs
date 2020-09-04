using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void Hurt(int damage)
    {
        Managers.Player.ChangeHealth(-damage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
