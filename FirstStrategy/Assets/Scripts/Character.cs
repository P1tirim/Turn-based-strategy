using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    float elapsedTime = 0.0f;
    public GameObject cells;
    Cells linkCells;
    Animator animator;

    public GameObject nord;
    public GameObject south;
    public GameObject west;
    public GameObject east;

    public GameObject[] sideForRotation = new GameObject[10];

    Vector3 targetDirNord;
    Vector3 targetDirSouth;
    Vector3 targetDirEast;
    Vector3 targetDirWest;

    string side;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        linkCells = cells.GetComponent<Cells>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move character
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Particle")
                {
                    Move();
                }else if(hit.transform.gameObject.tag == "ParticleEnemy")
                {
                    Attack();
                }
            }
        }

        //Stop walking
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && elapsedTime >= 0.7f)
        {
            animator.SetTrigger("Stop Walk");
            elapsedTime = 0.0f;
            linkCells.ChangeCurrentPosition(hit);
            RotationAfterWalk();

        }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && elapsedTime < 0.7f)
        {
            elapsedTime += Time.deltaTime;
        }
        
    }

    //Move and Turn
    void Move()
    {
        
                DefinitionSide();
                string[] xy = hit.transform.parent.name.Split(new char[] { ' ' });

                //turn depending on the side of the world the character is looking at
                if (Global.currentPositionX - int.Parse(xy[0]) == 1)
                {
                    if (side == "nord")
                    {
                        animator.SetTrigger("Turn right");
                        Debug.Log("North1");
                    }
                    else if (side == "east")
                    {
                        Debug.Log("East1");
                    }
                    else if (side == "south")
                    {
                        animator.SetTrigger("Turn left");
                        Debug.Log("South1");
                    }
                    else
                    {
                        animator.SetTrigger("Turn back");
                        Debug.Log("West1");
                    }
                }
                else if (Global.currentPositionX - int.Parse(xy[0]) == -1)
                {
                    if (side == "nord")
                    {
                        animator.SetTrigger("Turn left");
                        Debug.Log("North2");
                    }
                    else if (side == "east")
                    {
                        animator.SetTrigger("Turn back");
                        Debug.Log("East2");
                    }
                    else if (side == "south")
                    {
                        animator.SetTrigger("Turn right");
                        Debug.Log("South2");
                    }
                    else
                    {
                        Debug.Log("West2");
                    }
                }
                else if (Global.currentPositionY - int.Parse(xy[1]) == 1)
                {
                    if (side == "nord")
                    {
                        Debug.Log("North3");
                    }
                    else if (side == "east")
                    {
                        animator.SetTrigger("Turn left");
                        Debug.Log("East3");
                    }
                    else if (side == "south")
                    {
                        animator.SetTrigger("Turn back");
                        Debug.Log("South3");
                    }
                    else
                    {
                        animator.SetTrigger("Turn right");
                        Debug.Log("West3");
                    }

                }
                else if (Global.currentPositionY - int.Parse(xy[1]) == -1)
                {
                    if (side == "nord")
                    {
                        animator.SetTrigger("Turn back");
                        Debug.Log("North4");
                    }
                    else if (side == "east")
                    {
                        animator.SetTrigger("Turn right");
                        Debug.Log("East4");
                    }
                    else if (side == "south")
                    {
                        Debug.Log("South4");
                    }
                    else
                    {
                        animator.SetTrigger("Turn left");
                        Debug.Log("West4");
                    }
                }
                animator.SetTrigger("Walk");
                elapsedTime = 0.0f;
            }
        
    

    //Defination side of the world
    void DefinitionSide()
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
        float[] arr = new float[4] {angleNord, angleSouth, angleEast, angleWest};
        for (int i = 1; i<4; i++)
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

    }

    //adjusting the character in the cell
    void RotationAfterWalk()
    {
        DefinitionSide();

        string numberX = (Global.currentPositionX).ToString();
        string numberY = (Global.currentPositionY).ToString();

        if(side == "nord" || side == "south")
        {
            for(int i = 0; i < 10; i++)
            {
                string name = "n" + numberX;
                if (sideForRotation[i].name == name)
                {
                    if(side == "nord")
                    {
                        transform.LookAt(sideForRotation[i].transform);
                    }
                    else
                    {
                        transform.LookAt(sideForRotation[i].transform);
                        transform.Rotate(0, 180, 0);
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                string name = "w" + numberY;
                if (sideForRotation[i].name == name)
                {
                    if (side == "west")
                    {
                        transform.LookAt(sideForRotation[i].transform);
                    }
                    else
                    {
                        transform.LookAt(sideForRotation[i].transform, Vector3.up);
                        transform.Rotate(0, 180, 0);
                    }
                    break;
                }
            }
        }
        Vector3 vector;
        vector.x = hit.transform.position.x + 0.1f;
        vector.y = 0;
        vector.z = hit.transform.position.z;

        transform.position = vector;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
