using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    

    public GameObject nord;
    public GameObject south;
    public GameObject west;
    public GameObject east;

    string side;
    

    Vector3 targetDirNord;
    Vector3 targetDirSouth;
    Vector3 targetDirEast;
    Vector3 targetDirWest;

    bool move = false;

    public GameObject[] sideForRotation = new GameObject[10];

    bool turnAndMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void walk(Animator animator, GameManager linkGameManager, Cells linkCells, UnityEngine.AI.NavMeshAgent agent, Transform obj, bool click)
    {
        //Move character
        if(obj != null)
        {
            if (obj.transform.gameObject.tag == "ParticleMove" && click)
            {
                turnAndMove = true;
                transform.position = agent.nextPosition;
                Turn(animator, agent, obj);
            }
            else if (obj.transform.gameObject.tag == "ParticleAttack" && click)
            {
                Attack(animator, linkGameManager, agent, obj);
            }
        }
        
            
        
        
        if (agent.remainingDistance <= agent.stoppingDistance && move && !agent.pathPending)
        {
            Debug.Log("stop");
            agent.updateRotation = false;
            animator.SetBool("move", false);
            move = false;
            animator.SetFloat("Blend", 0);
            transform.position = agent.nextPosition;
            linkCells.ChangeCurrentPosition(obj);
            linkGameManager.NextPerson();

        }else if(agent.remainingDistance > agent.stoppingDistance && move && animator.GetCurrentAnimatorStateInfo(0).IsName("Blend"))
        {
            agent.nextPosition = transform.position;
            animator.SetFloat("Blend", agent.velocity.magnitude);
            agent.nextPosition = transform.position;
        }
    }
    

    //Move and Turn
    public void Turn(Animator animator, UnityEngine.AI.NavMeshAgent agent, Transform obj)
    {
        DefinitionSide();
        string[] xy = obj.parent.name.Split(new char[] { ' ' });

        //turn depending on the side of the world the character is looking at
        if (Global.currentPerson.currentPositionX - int.Parse(xy[0]) == 1)
        {
            if (side == "nord")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("North1");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
                
            }
            else if (side == "east")
            {
                Debug.Log("East1");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = obj.transform.position;
                    animator.SetBool("move", true);
                    agent.updateRotation = true;
                }
            }
            else if (side == "south")
            {
                animator.SetTrigger("Turn left");
                Debug.Log("South1");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else
            {
                animator.SetTrigger("Turn back");
                Debug.Log("West1");
                if(turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            
        }
        else if (Global.currentPerson.currentPositionX - int.Parse(xy[0]) == -1)
        {
            if (side == "nord")
            {
                animator.SetTrigger("Turn left");
                Debug.Log("North2");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else if (side == "east")
            {
                animator.SetTrigger("Turn back");
                Debug.Log("East2");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else if (side == "south")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("South2");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else
            {
                Debug.Log("West2");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = obj.position;
                    animator.SetBool("move", true);
                    agent.updateRotation = true;
                }   
            }
            
        }
        else if (Global.currentPerson.currentPositionY - int.Parse(xy[1]) == 1)
        {
            if (side == "nord")
            {
                Debug.Log("North3");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = obj.position;
                    animator.SetBool("move", true);
                    agent.updateRotation = true;
                }
            }
            else if (side == "east")
            {
                animator.SetTrigger("Turn left");
                Debug.Log("East3");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else if (side == "south")
            {
                animator.SetTrigger("Turn back");
                Debug.Log("South3");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else
            {
                animator.SetTrigger("Turn right");
                Debug.Log("West3");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            
        }
        else if (Global.currentPerson.currentPositionY - int.Parse(xy[1]) == -1)
        {
            if (side == "nord")
            {
                animator.SetTrigger("Turn back");
                Debug.Log("North4");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else if (side == "east")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("East4");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            else if (side == "south")
            {
                Debug.Log("South4");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = obj.position;
                    animator.SetBool("move", true);
                    agent.updateRotation = true;
                }
            }
            else
            {
                animator.SetTrigger("Turn left");
                Debug.Log("West4");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator, obj));
                }
            }
            
        }
        
    }

    IEnumerator wait(UnityEngine.AI.NavMeshAgent agent, Animator animator, Transform obj)
    {
        yield return new WaitForSeconds(1.2f);
        move = true;
        animator.SetBool("move", true);
        agent.destination = obj.position;
        agent.updateRotation = true;
    }

    //Defination side of the world
    public void DefinitionSide()
    {
        targetDirNord = nord.transform.position - transform.position;
        targetDirSouth = south.transform.position - transform.position;
        targetDirEast = east.transform.position - transform.position;
        targetDirWest = west.transform.position - transform.position;

        float angleNord = Vector3.Angle(targetDirNord, transform.forward);
        float angleSouth = Vector3.Angle(targetDirSouth, transform.forward);
        float angleEast = Vector3.Angle(targetDirEast, transform.forward);
        float angleWest = Vector3.Angle(targetDirWest, transform.forward);
        float min = angleNord;
        int index = 0;
        float[] arr = new float[4] { angleNord, angleSouth, angleEast, angleWest };
        for (int i = 1; i < 4; i++)
        {
            if (arr[i] < min)
            {
                min = arr[i];
                index = i;
            }
        }

        switch (index)
        {
            case 0:
                side = "nord";
                break;
            case 1:
                side = "south";
                break;
            case 2:
                side = "east";
                break;
            case 3:
                side = "west";
                break;
        }

        //adjusting the character in the cell
        

        
    }



    public void Attack(Animator animator, GameManager linkGameManager, UnityEngine.AI.NavMeshAgent agent, Transform obj)
    {
        turnAndMove = false;
        Turn(animator, agent, obj);
        animator.SetTrigger("Attack");
        StartCoroutine(waitAttack(linkGameManager, obj, animator));
    }

    IEnumerator waitAttack(GameManager linkGameManager, Transform obj, Animator animator)
    {
        yield return new WaitForSeconds(0.93f);
        TakeDamage(obj, animator);
        yield return new WaitForSeconds(4f);
        linkGameManager.NextPerson();
    }

    //-health
    void TakeDamage(Transform obj, Animator animator)
    {
        for(int i = 0; i < Global.listCharactersInGame.Count; i++)
        {
            if (Global.listCharactersInGame[i].currentCell.name == obj.parent.name)
            {
                Global.listCharactersInGame[i].health -= Global.currentPerson.damage;
                if (Global.listCharactersInGame[i].health <= 0)
                {
                    int index = i;
                    Global.listCharactersInGame[i].obj.GetComponent<Animator>().SetTrigger("Death");
                    StartCoroutine(waitDeath(index));

                }
                else
                {
                    Global.listCharactersInGame[i].obj.GetComponent<Animator>().SetTrigger("TakeDamage");
                }
                break;
            }
        }
    }

    IEnumerator waitDeath(int index)
    {
        yield return new WaitForSeconds(4f);
        Destroy(Global.listCharactersInGame[index].obj);
        Global.listCharactersInGame.Remove(Global.listCharactersInGame[index]);
    }
}
