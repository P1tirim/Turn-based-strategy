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
    RaycastHit hit;

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

    public void walk(Animator animator, GameManager linkGameManager, Cells linkCells, UnityEngine.AI.NavMeshAgent agent)
    {
        //Move character
        if (Input.GetMouseButtonDown(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log(Global.currentPerson.obj.name);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Particle")
                {
                    turnAndMove = true;
                    transform.position = agent.nextPosition;
                    Turn(animator, agent);
                    
                    Debug.Log(agent.remainingDistance);
                }
                else if (hit.transform.gameObject.tag == "ParticleEnemy")
                {
                    Attack(animator, linkGameManager, agent);
                }
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
            RotationAfterWalk();
            linkCells.ChangeCurrentPosition(hit);
            linkGameManager.NextPerson();

        }else if(agent.remainingDistance > agent.stoppingDistance && move && animator.GetCurrentAnimatorStateInfo(0).IsName("Blend"))
        {
            agent.nextPosition = transform.position;
            animator.SetFloat("Blend", agent.velocity.magnitude);
            agent.nextPosition = transform.position;
        }
    }
    

    //Move and Turn
    public void Turn(Animator animator, UnityEngine.AI.NavMeshAgent agent)
    {
        DefinitionSide();
        string[] xy = hit.transform.parent.name.Split(new char[] { ' ' });

        //turn depending on the side of the world the character is looking at
        if (Global.currentPerson.currentPositionX - int.Parse(xy[0]) == 1)
        {
            if (side == "nord")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("North1");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
                }
                
            }
            else if (side == "east")
            {
                Debug.Log("East1");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = hit.transform.position;
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
                    StartCoroutine(wait(agent, animator));
                }
            }
            else
            {
                animator.SetTrigger("Turn back");
                Debug.Log("West1");
                if(turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
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
                    StartCoroutine(wait(agent, animator));
                }
            }
            else if (side == "east")
            {
                animator.SetTrigger("Turn back");
                Debug.Log("East2");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
                }
            }
            else if (side == "south")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("South2");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
                }
            }
            else
            {
                Debug.Log("West2");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = hit.transform.position;
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
                    agent.destination = hit.transform.position;
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
                    StartCoroutine(wait(agent, animator));
                }
            }
            else if (side == "south")
            {
                animator.SetTrigger("Turn back");
                Debug.Log("South3");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
                }
            }
            else
            {
                animator.SetTrigger("Turn right");
                Debug.Log("West3");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
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
                    StartCoroutine(wait(agent, animator));
                }
            }
            else if (side == "east")
            {
                animator.SetTrigger("Turn right");
                Debug.Log("East4");
                if (turnAndMove)
                {
                    StartCoroutine(wait(agent, animator));
                }
            }
            else if (side == "south")
            {
                Debug.Log("South4");
                if (turnAndMove)
                {
                    move = true;
                    agent.destination = hit.transform.position;
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
                    StartCoroutine(wait(agent, animator));
                }
            }
            
        }
        
    }

    IEnumerator wait(UnityEngine.AI.NavMeshAgent agent, Animator animator)
    {
        yield return new WaitForSeconds(1.2f);
        move = true;
        animator.SetBool("move", true);
        agent.destination = hit.transform.position;
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
    public void RotationAfterWalk()
    {
        DefinitionSide();

        string numberX = (Global.currentPerson.currentPositionX).ToString();
        string numberY = (Global.currentPerson.currentPositionY).ToString();

        
        Vector3 vector;
        vector.x = hit.transform.position.x;
        vector.y = 0;
        vector.z = hit.transform.position.z;

      //  transform.position = vector;
    }


    public void Attack(Animator animator, GameManager linkGameManager, UnityEngine.AI.NavMeshAgent agent)
    {
        turnAndMove = false;
        Turn(animator, agent);
        animator.SetTrigger("Attack");
        StartCoroutine(waitAttack(linkGameManager));
    }

    IEnumerator waitAttack(GameManager linkGameManager)
    {
        yield return new WaitForSeconds(1.2f);
        linkGameManager.NextPerson();
    }
}
