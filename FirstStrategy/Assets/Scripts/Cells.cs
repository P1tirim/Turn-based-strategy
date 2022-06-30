using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour
{
    public int moveSpeed = 1;
    public GameObject particle;

    int currentPositionX = 0;
    int currentPositionY = 2;
    GameObject[,] cells = new GameObject[5, 5];
    List<GameObject> listParticles = new List<GameObject>();
    GameObject lastObj;


    // Start is called before the first frame update
    void Start()
    {
        CellsInArray();
        SpawnParticle();
    }

    // Update is called once per frame
    void Update()
    {
        //changes in the color of a cell when hovering over it with the cursor

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray ,out hit))
        {
            if(hit.transform.gameObject != lastObj && lastObj != null)
            {
                var main = lastObj.GetComponent<ParticleSystem>().main;
                main.startColor = Color.blue;
            }
            if (hit.transform.gameObject.tag == "Particle")
            {
                var main = hit.transform.gameObject.GetComponent<ParticleSystem>().main;
                main.startColor = Color.green;

                lastObj = hit.transform.gameObject;
            }

        }
        
    }

    //Put cells in array for further use
    void CellsInArray()
    {
        int child = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                cells[i, j] = this.gameObject.transform.GetChild(child).gameObject;
                child++;
            }
        }
    }

    //Span particle which show the cells where you can go
    void SpawnParticle()
    {
        
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (currentPositionX == i && currentPositionY == j) continue;
                if(Mathf.Abs(currentPositionX-i) <= moveSpeed && Mathf.Abs(currentPositionY-j) <= moveSpeed)
                {
                    GameObject cell = cells[i, j];
                    Vector3 vector;
                    vector.x = cell.transform.position.x + 0.16f;
                    vector.y = cell.transform.position.y;
                    vector.z = cell.transform.position.z;

                    GameObject newParticle = Instantiate(particle, vector, Quaternion.Euler(-90, 0, 0));
                    listParticles.Add(newParticle as GameObject);
                }
            }
        }
    }

    
}
