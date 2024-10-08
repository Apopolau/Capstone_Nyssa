using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Data Type/Task")]
public class TaskData : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] private string englishText;
    [TextArea(3, 10)]
    [SerializeField] private string frenchText;
    [Tooltip("Whether or not this task should be active when the game starts. This value shouldn't change")]
    [SerializeField] private bool taskActiveOnStart;
    [Tooltip("Whether or not this task is active right now")]
    [SerializeField] private bool taskActive;

    //Returns the text for the English version of the task
    public string GetEnglishText()
    {
        return englishText;
    }

    //Returns the text for the French version of the task
    public string GetFrenchText()
    {
        return frenchText;
    }

    //Get whether the task is supposed to be active when the game starts or not
    public bool GetTaskActiveOnStart()
    {
        return taskActiveOnStart;
    }

    //Get the task's current activation state
    public bool GetTaskActive()
    {
        return taskActive;
    }

    //Set the task's current activation state
    public void SetTaskActive(bool active)
    {
        taskActive = active;
    }
}
