using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffTerminal : Interactable
{
    [SerializeField] LevelTwoEvents levelTwoEvents;
    [SerializeField] DialogueTrigger waterCleanedDialogue;
    [SerializeField] GameObject strikeTarget;
    GameObject[] terminalComponents;
    Color scorchColor = new Color(0.3f, 0.3f, 0.3f);
    

    bool hasBeenShutOff;

    private void Awake()
    {
        terminalComponents = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            terminalComponents[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void TerminalShutOff() 
    {
        if (!hasBeenShutOff)
        {
            levelTwoEvents.OnFacilityShutOff();
            waterCleanedDialogue.TriggerDialogue();
            hasBeenShutOff = true;
            foreach(GameObject go in terminalComponents)
            {
                foreach(Material mat in go.GetComponent<MeshRenderer>().materials)
                {
                    mat.color = scorchColor;
                }
                
            }
        }
        
    }

    public GameObject GetStrikeTarget()
    {
        return strikeTarget;
    }
}
