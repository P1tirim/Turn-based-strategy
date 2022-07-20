using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static List<Person> listCharactersInGame = new List<Person>();
    public static List<GameObject> listPositionEnemy = new List<GameObject>();
    public static List<GameObject> listPositionPlayer = new List<GameObject>();
    public static List<GameObject> listParticles = new List<GameObject>();
    public static Person currentPerson;
    public static GameObject[,] cells = new GameObject[5, 5];
    public static bool first = true;
}
public class GameManager : MonoBehaviour
{
    public GameObject cells;
    Cells linkCells;

    // Start is called before the first frame update
    void Start()
    {
        linkCells = cells.GetComponent<Cells>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Next round
    public void NextPerson()
    {
        Global.first = true;
        Person item = Global.listCharactersInGame[0];
        Global.listCharactersInGame.Remove(item);
        Global.listCharactersInGame.Add(item);
        Global.currentPerson.haveMove = true;
        Global.currentPerson.haveAttack = true;
        Global.currentPerson.healthBar.SetActive(false);
        Global.currentPerson = Global.listCharactersInGame[0];
        Global.currentPerson.healthBar.SetActive(true);
        linkCells.SpawnParticle(Global.currentPerson.currentPositionX, Global.currentPerson.currentPositionY);
    }

    //spawn particles and determining the turn queue with the help of the initiative
    public void spawn()
    {
        for(int i = 0; i < Global.listCharactersInGame.Count; i++)
        {
            for(int j = 0; j < Global.listCharactersInGame.Count - 1; j++)
            {
                if (Global.listCharactersInGame[j].initiativeInFight < Global.listCharactersInGame[j + 1].initiativeInFight)
                {
                    Person buffer = Global.listCharactersInGame[j];
                    Global.listCharactersInGame[j] = Global.listCharactersInGame[j + 1];
                    Global.listCharactersInGame[j + 1] = buffer;
                }
            }
        }
        Global.currentPerson = Global.listCharactersInGame[0];
        Global.currentPerson.healthBar.SetActive(true);
        linkCells.SpawnParticle(Global.currentPerson.currentPositionX, Global.currentPerson.currentPositionY);
    }
}
//class Characters in game
public class Person
{
    public GameObject obj;
    public int currentPositionX;
    public int currentPositionY;
    public GameObject currentCell;
    public float healthMax;
    public float healthCurrent;
    public GameObject healthBar;
    public float damage;
    public int rangeAttack;
    public int initiative;
    public int initiativeInFight;
    public bool haveMove = true;
    public bool haveAttack = true;
}
