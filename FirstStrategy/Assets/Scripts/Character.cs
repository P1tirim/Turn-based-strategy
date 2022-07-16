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

    public int health = 15;
    public int damage = 5;

    public int startPositionX = 2;
    public int startPositionY = 3;

    UnityEngine.AI.NavMeshAgent agent;
    RaycastHit hit;

    Transform obj;
    bool click = false;

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
        character.health = health;
        character.damage = damage;
        Global.listCharactersInGame.Insert(0, character);

        obj = cells.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.currentPerson.obj == this.gameObject)
        {
            if (Input.GetMouseButtonDown(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log(Global.currentPerson.obj.name);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    obj = hit.transform;
                    click = true;
                    walk(animator, linkGameManager, linkCells, agent, obj, click);
                    click = false;
                }
            }
            walk(animator, linkGameManager, linkCells, agent, obj, click);

        }
        
        
    }














}
