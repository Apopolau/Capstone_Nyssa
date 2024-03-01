using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIconLeft;
    public Image characterIconRight;
    public TextMeshProUGUI dialogueArea;
    public TextMeshProUGUI dialogueAreaFR;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public GameObject dialogueBox; // Reference to the entire dialogue box
                                   // Reference to the UI controller script
    public EarthCharacterUIController uiController;
    public SplitScreen split;


    [SerializeField] private GameObjectRuntimeSet playerSet;
    private EarthPlayer earthPlayer;
    private CelestialPlayer celestialPlayer;



    public float typingSpeed = 0.2f;



    private void OnEnable()
    {
        for (int i = 0; i < playerSet.Items.Count; i++)
        {
            if (playerSet.Items[i].GetComponent<EarthPlayer>())
            {
                earthPlayer = playerSet.Items[i].GetComponent<EarthPlayer>();
            }
            else if (playerSet.Items[i].GetComponent<CelestialPlayer>())
            {
                celestialPlayer = playerSet.Items[i].GetComponent<CelestialPlayer>();
            }

        }


    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }
    private void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {


        // Activate the dialogue box if it's currently inactive
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);

        }
        else
        {
            isDialogueActive = true;
        }


        // Toggle other UI elements visibility
        uiController.ToggleOtherUIElements(false); // Pass false to deactivate other UI elements

        //Turn off other player controls, turn on dialogue controls
        if (!earthPlayer.earthControls.controls.DialogueControls.enabled)
        {
            //Don't change this order - the cancellation functions turn the default map back on, so it needs to be turned back off after
            if (earthPlayer.earthControls.controls.PlantIsSelected.enabled)
            {
                earthPlayer.OnPlantingCancelled();
                earthPlayer.earthControls.controls.PlantIsSelected.Disable();
            }
            if (earthPlayer.earthControls.controls.RemovingPlant.enabled)
            {
                earthPlayer.OnRemovingCancelled();
                earthPlayer.earthControls.controls.RemovingPlant.Disable();
            }
            earthPlayer.earthControls.controls.DialogueControls.Enable();
            earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        }
        if (!celestialPlayer.celestialControls.controls.DialogueControls.enabled)
        {
            celestialPlayer.celestialControls.controls.DialogueControls.Enable();
            celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Disable();
        }
        Time.timeScale = 0f;



        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();




    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();

            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        //characterIcon.sprite = currentLine.character.icon;

        //StopAllCoroutines();


        // Clear the dialogue areas
        dialogueArea.text = "";
        dialogueAreaFR.text = "";

        // Clear both character icons
        characterIconLeft.sprite = null;
        characterIconRight.sprite = null;

        if (currentLine.character.isLeft)
        {
            characterIconLeft.sprite = currentLine.character.iconLeft;
            // Fade in characterIconLeft
            characterIconLeft.CrossFadeAlpha(1f, 0f, true);
            // Fade out characterIconRight
            characterIconRight.CrossFadeAlpha(0f, 0f, true);
        }
        else if (!currentLine.character.isLeft)
        {
            characterIconRight.sprite = currentLine.character.iconRight;
            // Fade in characterIconRight
            characterIconRight.CrossFadeAlpha(1f, 0f, true);
            // Fade out characterIconLeft
            characterIconLeft.CrossFadeAlpha(0f, 0f, true);
        }


        //display all text at once
        dialogueArea.text = currentLine.line;
        dialogueAreaFR.text = currentLine.lineFR;

        // Display French line
        // StartCoroutine(TypeSentence(dialogueAreaFR, currentLine.lineFR));
        //StartCoroutine(TypeSentence(dialogueArea,currentLine.line));
    }

    //display letter by letter
    IEnumerator TypeSentence(TextMeshProUGUI dialogueArea, string line)
    {
        foreach (char letter in line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        if (dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
        }
        // Toggle other UI elements visibility
        uiController.ToggleOtherUIElements(true); // Pass true to reactivate other UI elements


        //Turn off the dialogue controls and turn on the default controls of both players
        earthPlayer.earthControls.controls.DialogueControls.Disable();
        earthPlayer.earthControls.controls.EarthPlayerDefault.Enable();
        celestialPlayer.celestialControls.controls.DialogueControls.Disable();
        celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Enable();

        Time.timeScale = 1f;

    }
}