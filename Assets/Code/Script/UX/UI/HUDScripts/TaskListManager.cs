using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskListManager : MonoBehaviour
{
    private bool isTaskCompleted = false;
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            
        }
        
    }

    public void CrossOutTask()
    {
        if (!isTaskCompleted && textComponent != null)
        {
            isTaskCompleted = true;
            textComponent.text = "<s>" + textComponent.text + "</s>"; // TextMeshPro supports rich text
        }
    }

    public bool GetTaskCompletion()
    {
        return isTaskCompleted;
    }
}
