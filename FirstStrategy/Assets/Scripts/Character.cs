using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Motion
{
    public GameObject cells;
    public GameObject gameManager;

    Cells linkCells;
    GameManager linkGameManager;

    Animator animator;

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
        character.currentPositionX = 2;
        character.currentPositionY = 4;
        Global.listCharactersInGame.Insert(0, character);

    }

    // Update is called once per frame
    void Update()
    {
        if (Global.currentPerson.obj == this.gameObject)
        {
            walk(animator, linkGameManager, linkCells, agent);
        }
        
        
    }
















}
