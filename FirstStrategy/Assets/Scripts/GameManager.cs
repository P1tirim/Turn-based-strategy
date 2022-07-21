using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class Global
{
    public static List<Person> listCharactersInGame = new List<Person>();
    public static List<GameObject> listPositionEnemy = new List<GameObject>();
    public static List<GameObject> listPositionPlayer = new List<GameObject>();
    public static List<GameObject> listParticles = new List<GameObject>();
    public static List<Weapon> listWeapon = new List<Weapon>();
    public static List<Armor> listArmor = new List<Armor>();
    public static Person currentPerson;
    public static GameObject[,] cells = new GameObject[5, 5];
    public static bool first = true;
    public static TMP_Text textOnMouse;
}
public class GameManager : MonoBehaviour
{
    public GameObject cells;
    Cells linkCells;
    public TMP_Text textOnMouse;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        linkCells = cells.GetComponent<Cells>();
        Global.textOnMouse = textOnMouse;
        textOnMouse.gameObject.SetActive(false);
        CreateWeapons();
        CreateArmor();
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
        Global.textOnMouse.gameObject.SetActive(false);
        CalculateProbability();
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
        CalculateProbability();
        linkCells.SpawnParticle(Global.currentPerson.currentPositionX, Global.currentPerson.currentPositionY);
    }

    //calcuate probability the attack
    void CalculateProbability()
    {
        for (int ii = 0; ii < Global.listCharactersInGame.Count; ii++)
        {
            if (Global.listCharactersInGame[ii].obj.tag == "Enemy")
            {
                float prob = (20 - Global.listCharactersInGame[ii].armor.armorClass + Global.currentPerson.weapon.hitProbability) / 20f * 100f;
                Global.listCharactersInGame[ii].probabilityOnHit = prob;
            }
        }
    }

    void CreateWeapons()
    {
        Weapon weapon = new Weapon();

        
        weapon.name = "sword";
        weapon.damage[0] = 1;
        weapon.damage[1] = 8;
        weapon.hitProbability = 4;
        Global.listWeapon.Add(weapon);

        weapon = new Weapon();
        weapon.name = "bow";
        weapon.damage[0] = 1;
        weapon.damage[1] = 8;
        weapon.hitProbability = 4;
        Global.listWeapon.Add(weapon);

        weapon = new Weapon();
        weapon.name = "special";
        weapon.damage[0] = 1;
        weapon.damage[1] = 12;
        weapon.hitProbability = 4;
        Global.listWeapon.Add(weapon);
    }

    void CreateArmor()
    {
        Armor armor = new Armor();

        armor.name = "hauberk";
        armor.armorClass = 18;
        armor.haveShield = true;
        Global.listArmor.Add(armor);

        armor = new Armor();
        armor.name = "leather";
        armor.armorClass = 15;
        armor.haveShield = false;
        Global.listArmor.Add(armor);

        armor = new Armor();
        armor.name = "none";
        armor.armorClass = 14;
        armor.haveShield = false;
        Global.listArmor.Add(armor);
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
    public Weapon weapon;
    public Armor armor;
    public int rangeAttack;
    public int initiative;
    public int initiativeInFight;
    public float probabilityOnHit;
    public bool haveMove = true;
    public bool haveAttack = true;
}

public class Weapon
{
    public string name;
    public int[] damage = new int[2];
    public int hitProbability;
}

public class Armor
{
    public string name;
    public int armorClass;
    public bool haveShield;
}

public enum ChooseWeapon
{
    Sword = 0,
    Bow = 1,
    Special = 2
}

public enum ChooseArmor
{
    Hauberk = 0,
    Leather = 1,
    None = 2
}
