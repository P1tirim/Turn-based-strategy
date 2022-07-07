using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startPositionX = 2;
    public int startPositionY = 2;

    // Start is called before the first frame update
    void Start()
    {
        //Create new character
        Person character = new Person();
        character.obj = this.gameObject;
        character.currentPositionX = startPositionX;
        character.currentPositionY = startPositionY;
        Global.listCharactersInGame.Add(character);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.currentPerson.obj == this.gameObject)
        {

        }
    }
}
