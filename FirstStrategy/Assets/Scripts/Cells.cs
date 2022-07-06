using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
   public static int currentPositionX = 2;
   public static int currentPositionY = 4;

   public static int currentPositionEnemyX = 2;
   public static int currentPositionEnemyY = 2;
}
public class Cells : MonoBehaviour
{
    public int moveSpeed = 1;
    public GameObject particle;

    GameObject[,] cells = new GameObject[5, 5];
    List<GameObject> listParticles = new List<GameObject>();
    List<GameObject> listPositionEnemy = new List<GameObject>();
    GameObject lastObj;


    // Start is called before the first frame update
    void Start()
    {
        CellsInArray();
        SpawnParticle();
        listPositionEnemy.Add(cells[Global.currentPositionEnemyX, Global.currentPositionEnemyY]);

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
                if (Global.currentPositionX == i && Global.currentPositionY == j) continue;
                if (listPositionEnemy.Contains(cells[i, j])) continue;
                if((Mathf.Abs(Global.currentPositionX-i) <= moveSpeed && Mathf.Abs(Global.currentPositionY-j) == 0) || (Mathf.Abs(Global.currentPositionX - i) == 0 && Mathf.Abs(Global.currentPositionY - j) <= moveSpeed))
                {
                    GameObject cell = cells[i, j];
                    Vector3 vector;
                    vector.x = cell.transform.position.x + 0.16f;
                    vector.y = cell.transform.position.y;
                    vector.z = cell.transform.position.z;

                    GameObject newParticle = Instantiate(particle, vector, Quaternion.Euler(-90, 0, 0), cell.transform);
                    listParticles.Add(newParticle as GameObject);
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

    //Change current position of the character
    public void ChangeCurrentPosition(RaycastHit hit)
    {
       
         string[] xy = hit.transform.parent.name.Split(new char[] { ' ' });
         Global.currentPositionX = int.Parse(xy[0]);
         Global.currentPositionY = int.Parse(xy[1]);

         for (int i = 0; i < listParticles.Count; i++)
         {
         Destroy(listParticles[i]);
         }
         SpawnParticle();

            
        }
    }


