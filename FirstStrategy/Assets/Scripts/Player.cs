using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Motion
{
    public GameObject cells;
    public GameObject gameManager;
    public GameObject healthBar;

    Cells linkCells;
    GameManager linkGameManager;

    Animator animator;

    public float healthMax = 15;
    public ChooseWeapon weapon;
    public ChooseArmor armor;
    public int rangeAttack = 1;
    public int initiative = 4;

    

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

        StartCoroutine(wait());

        obj = cells.transform;
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        //Create new character
        Person character = new Person();
        character.obj = this.gameObject;
        character.currentPositionX = startPositionX;
        character.currentPositionY = startPositionY;
        character.healthMax = healthMax;
        character.healthCurrent = healthMax;
        character.healthBar = healthBar;
        character.weapon = Global.listWeapon[(int)weapon];
        character.armor = Global.listArmor[(int)armor];
        character.rangeAttack = rangeAttack;
        character.initiative = initiative;
        character.initiativeInFight = Random.Range(1, 20) + initiative;
        Global.listCharactersInGame.Insert(0, character);
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.currentPerson != null)
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

                if (Input.GetKeyDown("space"))
                {
                    linkGameManager.NextPerson();
                }
            }
        }
        
        
        
    }














}
