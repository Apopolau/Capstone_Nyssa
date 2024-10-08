using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticBagMonster : Enemy
{
    [Header("Plastic Bag Monster variables")]
    //Interaction with plants
    [SerializeField] private GameObjectRuntimeSet plantSet;
    private GameObject closestPlant;
    [SerializeField] private float smotherRange;
    private bool seesPlant = false;
    private bool inSmotherRange = false;
    private bool smotherInitiated = false;
    private Vector3 plantLocation;

    protected WaitForSeconds chokeTime;
    private WaitForSeconds refreshTime = new WaitForSeconds(0.5f);

    protected override void Awake()
    {
        animator = GetComponent<PlasticBagMonsterAnimator>();
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(CalculatePlantDistance());
    }

    private IEnumerator CalculatePlantDistance()
    {
        while (true)
        {
            //arbitrarily using attack time since it's 1s. May need to change in future
            yield return refreshTime;
            float distance = enemyStats.sightRange;
            seesPlant = false;
            inSmotherRange = false;
            foreach (GameObject plant in plantSet.Items)
            {
                if (Mathf.Abs((plant.GetComponent<Plant>().transform.position - this.transform.position).magnitude) < distance)
                {
                    distance = Mathf.Abs((plant.GetComponent<Plant>().transform.position - this.transform.position).magnitude);
                    closestPlant = plant;
                    //if its already smothered dont gang up
                    if (!plant.GetComponent<Plant>().isSmothered)
                    { seesPlant = true; }
                }
            }
            if (distance < smotherRange)
            {

                inSmotherRange = true;
            }
        }

    }

    

    protected override void OnDeath()
    {
        StartCoroutine(Die());
    }

    protected IEnumerator Die()
    {
        isDying = true;
        isStaggered = true;
        animator.SetAnimationFlag("die", true);
        yield return animator.GetAnimationWaitTime("die"); ;
        Destroy(gameObject);
    }



    /// <summary>
    /// GETTERS AND SETTERS
    /// </summary>

    public bool GetSeesPlant()
    {
        return seesPlant;
    }

    public bool GetInSmotherRange()
    {
        return inSmotherRange;
    }

    public bool GetSmotherInitiated()
    {
        return smotherInitiated;
    }

    public void SetSmotherInitiated(bool initiated)
    {
        smotherInitiated = initiated;
        if (initiated)
        {
            soundLibrary.PlayAttackClips();
            StartCoroutine(SmotherPlant());
        }
        else
        {
            StopCoroutine(SmotherPlant());
            animator.SetAnimationFlag("choke", false);
        }
    }

    //Handles turning the animation on and off for this
    private IEnumerator SmotherPlant()
    {
        animator.SetAnimationFlag("choke", true);
        yield return animator.GetAnimationWaitTime("choke");
        animator.SetAnimationFlag("choke", false);
    }

    public GameObject GetClosestPlant()
    {
        return closestPlant;

    }

    public void SetClosestPlant(GameObject nextPlant)
    {
        closestPlant = nextPlant;
    }

    public Vector3 GetPlantLocation()
    {
        return plantLocation;
    }
}
