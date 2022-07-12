using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cells : MonoBehaviour
{
    public int moveSpeed = 1;
    public GameObject particleMove;
    public GameObject particleEnemy;

    GameObject lastObj;
    GameObject newParticle;

    public GameObject gameManager;
    GameManager linkGameManager;

    // Start is called before the first frame update
    void Start()
    {
        linkGameManager = gameManager.GetComponent<GameManager>();
        CellsInArray();

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();        
    }

    //Put cells in array for further use
    void CellsInArray()
    {
        int child = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Global.cells[i, j] = this.gameObject.transform.GetChild(child).gameObject;
                child++;
            }
        }

        for(int i = 0; i < Global.listCharactersInGame.Count; i++)
        {
            if (Global.listCharactersInGame[i].obj.tag == "Enemy")
            {
                Global.listCharactersInGame[i].currentCell = Global.cells[Global.listCharactersInGame[i].currentPositionX, Global.listCharactersInGame[i].currentPositionY];
            }
            else
            {
                Global.listCharactersInGame[i].currentCell = Global.cells[Global.listCharactersInGame[i].currentPositionX, Global.listCharactersInGame[i].currentPositionY];
            }
        }
        linkGameManager.spawn();
    }

    //Span particle which show the cells where you can go
    public void SpawnParticle(int currentPositionX,int currentPositionY)
    {
        for (int i = 0; i < Global.listParticles.Count; i++)
        {
            Destroy(Global.listParticles[i]);
        }
        Global.listParticles.Clear();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (currentPositionX == i && currentPositionY == j) continue;
                if((Mathf.Abs(currentPositionX-i) <= moveSpeed && Mathf.Abs(currentPositionY-j) == 0) || (Mathf.Abs(currentPositionX - i) == 0 && Mathf.Abs(currentPositionY - j) <= moveSpeed))
                {
                    bool spawn = false;
                    GameObject cell = Global.cells[i, j];
                    Vector3 vector;
                    vector.x = cell.transform.position.x + 2f;
                    vector.y = cell.transform.position.y;
                    vector.z = cell.transform.position.z;
                    for(int k = 0; k < Global.listCharactersInGame.Count; k++)
                    {
                        if (Global.listCharactersInGame[k].obj.tag == "Enemy" && Global.currentPerson.obj.tag == "Player" && Global.listCharactersInGame[k].currentCell == cell)
                        {
                            newParticle = Instantiate(particleEnemy, vector, Quaternion.Euler(-90, 0, 0), cell.transform);
                            spawn = true;

                        }else if(Global.listCharactersInGame[k].obj.tag == "Enemy" && Global.currentPerson.obj.tag == "Enemy" && Global.listCharactersInGame[k].currentCell == cell)
                        {
                            spawn = true;

                        }
                        else if(Global.listCharactersInGame[k].obj.tag == "Player" && Global.currentPerson.obj.tag == "Enemy" && Global.listCharactersInGame[k].currentCell == cell)
                        {
                            newParticle = Instantiate(particleEnemy, vector, Quaternion.Euler(-90, 0, 0), cell.transform);
                            spawn = true;

                        }
                        else if(Global.listCharactersInGame[k].obj.tag == "Player" && Global.currentPerson.obj.tag == "Player" && Global.listCharactersInGame[k].currentCell == cell)
                        {
                            spawn = true;
                        }
                        
                    }
                    if(spawn == false) newParticle = Instantiate(particleMove, vector, Quaternion.Euler(-90, 0, 0), cell.transform);
                    Global.listParticles.Add(newParticle as GameObject);
                }
            }
        }
    }
 
    //changes in the color of a cell when hovering over it with the cursor
   public void ChangeColor()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != lastObj && lastObj != null)
            {
                var main = lastObj.GetComponent<ParticleSystem>().main;
                if(lastObj.tag == "ParticleMove")
                {
                    main.startColor = Color.blue;
                }else if(lastObj.tag == "ParticleAttack")
                {
                    main.startColor = Color.yellow;
                }
                
            }
            if (hit.transform.gameObject.tag == "ParticleMove")
            {
                var main = hit.transform.gameObject.GetComponent<ParticleSystem>().main;
                main.startColor = Color.green;

                lastObj = hit.transform.gameObject;
            }
            if(hit.transform.gameObject.tag == "ParticleAttack")
            {
                var main = hit.transform.gameObject.GetComponent<ParticleSystem>().main;
                main.startColor = Color.red;

                lastObj = hit.transform.gameObject;
            }
        }
    }

    //Change current position of the character
    public void ChangeCurrentPosition(Transform obj)
    {
         string[] xy = obj.parent.name.Split(new char[] { ' ' });
         Global.currentPerson.currentPositionX = int.Parse(xy[0]);
         Global.currentPerson.currentPositionY = int.Parse(xy[1]);
         Global.currentPerson.currentCell = Global.cells[Global.currentPerson.currentPositionX, Global.currentPerson.currentPositionY];

        for (int i = 0; i < Global.listParticles.Count; i++)
         {
            Destroy(Global.listParticles[i]);
        }
        Global.listParticles.Clear();

    }
    }


