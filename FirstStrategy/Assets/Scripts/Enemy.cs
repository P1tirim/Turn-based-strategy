using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Motion
{
    public GameObject cells;
    public GameObject gameManager;

    Cells linkCells;
    GameManager linkGameManager;

    Animator animator;

    public int startPositionX = 2;
    public int startPositionY = 2;

    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        linkCells = cells.GetComponent<Cells>();
        linkGameManager = gameManager.GetComponent<GameManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.updatePosition = false;

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
            walk(animator, linkGameManager, linkCells, agent);
        }
    }
}
