using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    float elapsedTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move character
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "Particle")
                {
                    this.GetComponent<Animator>().SetTrigger("Walk");
                    elapsedTime = 0.0f;
                }
            }
        }

        //Stop walking
        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk") && elapsedTime >= 0.935)
        {
            this.GetComponent<Animator>().SetTrigger("Stop Walk");
            Debug.Log(elapsedTime);
            elapsedTime = 0.0f;
        }

        elapsedTime += Time.deltaTime;
    }
}
