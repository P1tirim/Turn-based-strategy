using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Motion
{
    public GameObject cells;
    public GameObject gameManager;
    public GameObject healthBar;

    Cells linkCells;
    GameManager linkGameManager;

    Animator animator;

    public float healthMax = 10;
    public ChooseWeapon weapon;
    public ChooseArmor armor;
    public int rangeAttack = 1;
    public int initiative = 3;

    public int startPositionX = 2;
    public int startPositionY = 2;

    UnityEngine.AI.NavMeshAgent agent;

    Transform obj;
    bool click = false;

    Person nearestPlayer;

    List<GameObject> listParticlesMove = new List<GameObject>();
    List<GameObject> listParticlesAttack = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        linkCells = cells.GetComponent<Cells>();
        linkGameManager = gameManager.GetComponent<GameManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.updatePosition = false;

        StartCoroutine(waitPerson());

        obj = cells.transform;
    }

    IEnumerator waitPerson()
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
        Global.listCharactersInGame.Add(character);
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.currentPerson != null)
        {
            if (Global.currentPerson.obj == this.gameObject)
            {
                if (Global.first)
                {
                    StartCoroutine(wait());
                }
                else
                {

                    walk(animator, linkGameManager, linkCells, agent, obj, click);
                }


            }
        }
        
    }

    IEnumerator wait()
    {
        Global.first = false;
        yield return new WaitForSeconds(1f);
        DeterminingNearestPlayer();
    }

    //Choose cells for move or attack. Attack is a priority
    void chooseCell()
    {
        listParticlesAttack.Clear();
        listParticlesMove.Clear();
        GameObject particle = null;
        for (int i = 0; i < Global.listParticles.Count; i++)
        {
            if (Global.listParticles[i].tag == "ParticleMove")
            {
                listParticlesMove.Add(Global.listParticles[i]);
            }
            else
            {
                listParticlesAttack.Add(Global.listParticles[i]);
            }
        }

        
        if (listParticlesAttack.Count > 0)
        {
            particle = listParticlesAttack[Random.Range(0, listParticlesAttack.Count)];
        }
        else if(listParticlesMove.Count > 0)
        {
            int ch = Mathf.Abs(Global.currentPerson.currentPositionX - nearestPlayer.currentPositionX) + Mathf.Abs(Global.currentPerson.currentPositionY - nearestPlayer.currentPositionY);
            int i = 0;
            while (true)
            {
                string[] xy = listParticlesMove[i].transform.parent.name.Split(new char[] { ' ' });
                int x = int.Parse(xy[0]);
                int y = int.Parse(xy[1]);

                int newch = Mathf.Abs(x - nearestPlayer.currentPositionX) + Mathf.Abs(y - nearestPlayer.currentPositionY);
                if(newch >= ch)
                {
                    listParticlesMove.Remove(listParticlesMove[i]);
                }
                else
                {
                    i++;
                }
                if (i == listParticlesMove.Count) break;
            }
            if(Global.currentPerson.rangeAttack > 1 && Global.currentPerson.haveAttack == false)
            {
                particle = null;
            }else if(listParticlesMove.Count > 0) particle = listParticlesMove[Random.Range(0, listParticlesAttack.Count)];
        }
        if(particle == null)
        {
            obj = null;
            linkGameManager.NextPerson();
        }
        else
        {
            obj = particle.transform;
            GameObject cell = obj.transform.parent.gameObject;
            for (int i = 0; i < Global.listCharactersInGame.Count; i++)
            {
                if (Global.listCharactersInGame[i].currentCell == cell)
                {
                    Global.listCharactersInGame[i].healthBar.SetActive(true);
                    break;
                }
            }
        }
        
        click = true;        
        walk(animator, linkGameManager, linkCells, agent, obj, click);
        click = false;
        
    }

    //determination of the nearest player to be followed by movement
    void DeterminingNearestPlayer()
    {
        
        int min = 1000000;
        for(int i = 0; i < Global.listCharactersInGame.Count; i++)
        {
            if (Global.listCharactersInGame[i].obj.tag == "Player")
            {
                int ch = Mathf.Abs(Global.currentPerson.currentPositionX - Global.listCharactersInGame[i].currentPositionX) + Mathf.Abs(Global.currentPerson.currentPositionY - Global.listCharactersInGame[i].currentPositionY);
                if (ch < min)
                {
                    min = ch;
                    nearestPlayer = Global.listCharactersInGame[i];
                }
            }
        }
        chooseCell();
    }
}
