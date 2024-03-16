using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public UserSettingsManager userSettingsManager;

    public Image characterIconLeft;
    public Image characterIconRight;
    public TextMeshProUGUI dialogueArea;
    //public TextMeshProUGUI dialogueAreaFR;

    private Queue<DialogueEvent> events;
    private DialogueEvent currentEvent;

    public bool isDialogueActive = false;
    public GameObject dialogueBox; // Reference to the entire dialogue box

    public EarthCharacterUIController uiController;// Reference to the UI controller script
    public SplitScreen split;
    public Camera mainCam;

    [SerializeField] private GameObjectRuntimeSet playerSet;
    private EarthPlayer earthPlayer;
    private CelestialPlayer celestialPlayer;

    [SerializeField] private List<SpriteLibrary> spriteLibraries;
    [SerializeField] private List<DialogueCharacter> dialogueCharacters;
    private DialogueCharacter currentCharacter;
    private Sprite currentSprite;

    public float typingSpeed = 0.2f;

    //Camera speed variables
    private float zoom;
    private float zoomMultiplier;
    private Vector3 panVal;
    private float panMultiplier;
    private float zoomVelocity = 0f;
    private Vector3 panVelocity = new Vector3(0, 0, 0);
    private float smoothTime = 0.25f;
    private float maxZoom = 60f;
    private float minZoom = 5f;
    private bool panningOn;

    WaitForSecondsRealtime panTime = new WaitForSecondsRealtime(3f);

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

        events = new Queue<DialogueEvent>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (panningOn)
        {
            HandleCameraPan((DialogueCameraPan)currentEvent);
        }

    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Activate the dialogue box if it's currently inactive

        Time.timeScale = 0f;
        split.EnterCutscene();

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
            if (earthPlayer.earthControls.controls.HealSelect.enabled)
            {
                earthPlayer.OnHealingCancelled();
                earthPlayer.earthControls.controls.HealSelect.Disable();
            }
            if (earthPlayer.earthControls.controls.BarrierSelect.enabled)
            {
                earthPlayer.OnBarrierCancelled();
                earthPlayer.earthControls.controls.BarrierSelect.Disable();
            }
            earthPlayer.earthControls.controls.DialogueControls.Enable();
            earthPlayer.earthControls.controls.EarthPlayerDefault.Disable();
        }
        if (!celestialPlayer.celestialControls.controls.DialogueControls.enabled)
        {
            celestialPlayer.celestialControls.controls.DialogueControls.Enable();
            celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Disable();
        }

        events.Clear();

        foreach (DialogueEvent dialogueEvent in dialogue.dialogueEvents)
        {
            events.Enqueue(dialogueEvent);
        }

        HandleNextEvents();

    }

    private void AssignCharacter(DialogueLine currentLine)
    {
        if (currentLine.speaker == DialogueLine.Character.CELESTE)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Celeste"));
        }
        else if (currentLine.speaker == DialogueLine.Character.SPROUT)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Sprout"));
        }
        else if (currentLine.speaker == DialogueLine.Character.DUCK)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Duck"));
        }
        else if (currentLine.speaker == DialogueLine.Character.HEDGEHOG)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Hedgehog"));
        }
        else if (currentLine.speaker == DialogueLine.Character.FOX)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Fox"));
        }
        else if (currentLine.speaker == DialogueLine.Character.NYSSA)
        {
            currentCharacter = dialogueCharacters.Find(character => character.characterName.Equals("Nyssa"));
        }

        if (currentLine.emotion == DialogueLine.Emotion.DEFAULT)
        {
            currentSprite = currentCharacter.characterSprites.Default;
        }
        else if (currentLine.emotion == DialogueLine.Emotion.SILHOUETTE)
        {
            currentSprite = currentCharacter.characterSprites.Silhouette;
        }
        if (currentLine.emotion == DialogueLine.Emotion.HAPPY)
        {
            currentSprite = currentCharacter.characterSprites.Happy;
        }
        if (currentLine.emotion == DialogueLine.Emotion.UPSET)
        {
            currentSprite = currentCharacter.characterSprites.Upset;
        }
        if (currentLine.emotion == DialogueLine.Emotion.ANGRY)
        {
            currentSprite = currentCharacter.characterSprites.Angry;
        }
        if (currentLine.emotion == DialogueLine.Emotion.DETERMINED)
        {
            currentSprite = currentCharacter.characterSprites.Determined;
        }
    }

    public void HandleNextEvents()
    {
        if (events.Count == 0)
        {
            EndDialogue();

            return;
        }

        currentEvent = events.Dequeue();

        if (currentEvent is DialogueLine)
        {

            DisplayNextDialogueLine((DialogueLine)currentEvent);
        }
        else if (currentEvent is DialogueCameraPan)
        {
            panningOn = true;
            StartCoroutine(TurnPanOff());
            //HandleCameraPan((DialogueCameraPan)currentEvent);
        }
        else if (currentEvent is DialogueMissionEnd)
        {
            HandleSceneTransition((DialogueMissionEnd)currentEvent);
        }

    }

    public void DisplayNextDialogueLine(DialogueLine currentLine)
    {
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            isDialogueActive = true;
        }

        // Clear the dialogue areas
        dialogueArea.text = "";
        //dialogueAreaFR.text = "";

        // Clear both character icons
        characterIconLeft.sprite = null;
        characterIconRight.sprite = null;

        AssignCharacter(currentLine);

        if (currentCharacter.isLeft)
        {
            characterIconLeft.sprite = currentSprite;
            // Fade in characterIconLeft
            characterIconLeft.CrossFadeAlpha(1f, 0f, true);
            // Fade out characterIconRight
            characterIconRight.CrossFadeAlpha(0f, 0f, true);
        }
        else if (!currentCharacter.isLeft)
        {
            characterIconRight.sprite = currentSprite;
            // Fade in characterIconRight
            characterIconRight.CrossFadeAlpha(1f, 0f, true);
            // Fade out characterIconLeft
            characterIconLeft.CrossFadeAlpha(0f, 0f, true);
        }

        // Determine which language to display based on the user's language setting
        switch (userSettingsManager.chosenLanguage)
        {
            case UserSettingsManager.GameLanguage.ENGLISH:
                // Display English dialogue
                dialogueArea.text = currentLine.line;
                break;
            case UserSettingsManager.GameLanguage.FRENCH:
                // Display French dialogue
                dialogueArea.text = currentLine.lineFR;
                Debug.Log("French text should display");
                break;

        }
    }

    public void HandleCameraPan(DialogueCameraPan pan)
    {
        
        //Time.timeScale = 1f;
        if (dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
        }

        //We want to switch to single screen for dialogue I think
        if (split.Manager == 1)
        {

        }

        zoom = mainCam.orthographicSize;
        //zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.orthographicSize, pan.GetZoomAmount(), ref zoomVelocity, pan.GetZoomSpeed(), 100, 1);

        if (pan.GetPanType() == DialogueCameraPan.PanType.OBJECT)
        {
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position, pan.GetPanObject().transform.position + pan.GetPanOffset(), ref panVelocity, pan.GetPanSpeed(), 100, 1);
        }
        else if (pan.GetPanType() == DialogueCameraPan.PanType.LOCATION)
        {
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position, pan.GetPanLocation() + pan.GetPanOffset(), ref panVelocity, pan.GetPanSpeed(), 100, 1);
        }
    }

    public IEnumerator TurnPanOff()
    {
        yield return panTime;

        panningOn = false;
        HandleNextEvents();

    }

    public void HandleSceneTransition(DialogueMissionEnd nextMission)
    {
        SceneManager.LoadScene(nextMission.GetTargetScene());
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

        split.ExitCutscene();
        Time.timeScale = 1f;

    }
}