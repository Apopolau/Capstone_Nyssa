using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialPlayerBasicAttackTrigger : MonoBehaviour
{
    public bool enemyHit = false;




    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

           

          enemyHit=true;
        
        }
    }
    private void OnTriggerExit(Collider other)
    {
        

        if (other.transform.gameObject.tag == "Enemy")
        {

          enemyHit = false;
        
            //enemyTarget = null;



        }
    }



}
