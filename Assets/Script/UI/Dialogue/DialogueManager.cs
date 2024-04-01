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
    [SerializeField] private DialogueCameraPan panToOrigin;

    public float typingSpeed = 0.2f;

    //Camera speed variables
    //Zoom variables, zooming in and out
    private float zoom;
    private float zoomMultiplier;
    private float zoomVelocity = 0f;
    private float maxZoom = 60f;
    private float minZoom = 5f;

    //Pan variables, moving around the map
    private Vector3 panVal;
    private float panMultiplier;
    private Vector3 panVelocity = new Vector3(0, 0, 0);

    //private float smoothTime = 0.25f;

    [SerializeField] private bool panningOn;
    [SerializeField] private bool returningToOrigin;
    private bool dialoguePan;

    //WaitForSecondsRealtime panTime = new WaitForSecondsRealtime(3f);

    private void OnEnable()
    {
        SetReferences();
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
            //Handle the two types of panning
            if (currentEvent is DialogueCameraPan)
            {
                HandleCameraPan((DialogueCameraPan)currentEvent);
            }
            else if (currentEvent is DialoguePanAndText)
            {
                HandleCameraPanDialogue((DialoguePanAndText)currentEvent);
            }
        }
        if (returningToOrigin)
        {
            ReturnCameraToOrigin();
        }
    }

    private void FixedUpdate()
    {
        
    }
    
    private void SetReferences()
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

    /// <summary>
    /// MAIN DIALOGUE PROGRESS FUNCTIONS
    /// </summary>
    /// <param name="dialogue"></param>
    //Initiate the dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        // Activate the dialogue box if it's currently inactive

        Time.timeScale = 0f;
        split.EnterCutscene();

        // Toggle other UI elements visibility
        uiController.ToggleOtherUIElements(false); // Pass false to deactivate other UI elements

        if(earthPlayer == null || celestialPlayer == null)
        {
            SetReferences();
        }

        earthPlayer.ToggleDialogueState(true);

        if (!celestialPlayer.celestialControls.controls.DialogueControls.enabled)
        {
            celestialPlayer.celestialControls.controls.DialogueControls.Enable();
            celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Disable();
        }

        events.Clear();

        if (dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
        }

        foreach (DialogueEvent dialogueEvent in dialogue.dialogueEvents)
        {
            events.Enqueue(dialogueEvent);
        }

        HandleNextEvents();

    }

    public void HandleDialogueContinue()
    {

        if(!panningOn && !returningToOrigin)
        {
            HandleNextEvents();
        }

    }

    //Figures out what to do with each dialogue event in the list
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
            dialoguePan = false;
            panningOn = true;
            earthPlayer.ToggleWaiting(true);
            StartCoroutine(TurnPanOff((DialogueCameraPan)currentEvent));
            //HandleCameraPan((DialogueCameraPan)currentEvent);
        }
        else if(currentEvent is DialoguePanAndText)
        {
            DialoguePanAndText panLineEvent = currentEvent as DialoguePanAndText;

            dialoguePan = true;
            panningOn = true;
            earthPlayer.ToggleWaiting(true);
            DisplayNextDialogueLine(panLineEvent.dialogueLine);
            StartCoroutine(TurnPanOff(panLineEvent));
        }
        else if(currentEvent is DialogueAnimation)
        {
            HandleAnimation((DialogueAnimation)currentEvent);
        }
        else if (currentEvent is DialogueMissionEnd)
        {
            HandleSceneTransition((DialogueMissionEnd)currentEvent);
        }

    }

    //Wraps up the dialogue when we run out of events
    public void EndDialogue()
    {
        ReturnCameraToOrigin();
        if (dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
        }
        // Toggle other UI elements visibility
        uiController.ToggleOtherUIElements(true); // Pass true to reactivate other UI elements

        panningOn = false;

        //Restore both characters' default controls
        celestialPlayer.celestialControls.controls.DialogueControls.Disable();
        celestialPlayer.celestialControls.controls.CelestialPlayerDefault.Enable();
        earthPlayer.ToggleDialogueState(false);

        split.ExitCutscene();
        Time.timeScale = 1f;
    }



    /// <summary>
    /// FUNCTIONS TO HANDLE EACH TYPE OF DIALOGUE EVENT
    /// </summary>
    //When displaying a dialogue line, handles the text
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
                break;

        }
    }

    //This runs if the dialogue event is a camera pan, handles the movement
    public void HandleCameraPan(DialogueCameraPan pan)
    {
        //Sets states related to panning and dialogue

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
            //Set location
            Vector3 targetLocation = new Vector3(pan.GetPanObject().transform.position.x + pan.GetPanOffset().x,
                pan.GetPanObject().transform.position.y + pan.GetPanOffset().y, pan.GetPanObject().transform.position.z + pan.GetPanOffset().z);
            //Shift towards it
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position,
                targetLocation, ref panVelocity, pan.GetPanSpeed(), 100, 1);
        }
        else if (pan.GetPanType() == DialogueCameraPan.PanType.LOCATION)
        {
            //Set location
            Vector3 targetLocation = new Vector3(pan.GetPanLocation().x + pan.GetPanOffset().x,
                pan.GetPanLocation().y + pan.GetPanOffset().y, pan.GetPanLocation().z + pan.GetPanOffset().z);
            //Shift towards it
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position,
                targetLocation, ref panVelocity, pan.GetPanSpeed(), 100, 1);
        }
    }

    public void HandleCameraPanDialogue(DialoguePanAndText pan)
    {
        DialogueCameraPan cameraPan = pan.dialogueCameraPan;
        DialogueLine currentLine = pan.dialogueLine;

        if (!dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(true); // Deactivate the dialogue box
            isDialogueActive = true; // Set the dialogue state to inactive
        }

        //We want to switch to single screen for dialogue I think
        if (split.Manager == 1)
        {

        }

        // Clear the dialogue areas
        dialogueArea.text = "";

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
                break;

        }

        zoom = mainCam.orthographicSize;
        //zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.orthographicSize, pan.dialogueCameraPan.GetZoomAmount(), ref zoomVelocity, pan.dialogueCameraPan.GetZoomSpeed(), 100, 1);

        if (cameraPan.GetPanType() == DialogueCameraPan.PanType.OBJECT)
        {
            //Set location
            Vector3 targetLocation = new Vector3(cameraPan.GetPanObject().transform.position.x + cameraPan.GetPanOffset().x,
                cameraPan.GetPanObject().transform.position.y + cameraPan.GetPanOffset().y, cameraPan.GetPanObject().transform.position.z + cameraPan.GetPanOffset().z);
            //Shift towards it
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position,
                targetLocation, ref panVelocity, cameraPan.GetPanSpeed(), 100, 1);
        }
        else if (cameraPan.GetPanType() == DialogueCameraPan.PanType.LOCATION)
        {
            //Set location
            Vector3 targetLocation = new Vector3(cameraPan.GetPanLocation().x + cameraPan.GetPanOffset().x,
                cameraPan.GetPanLocation().y + cameraPan.GetPanOffset().y, cameraPan.GetPanLocation().z + cameraPan.GetPanOffset().z);
            //Shift towards it
            mainCam.gameObject.transform.position = Vector3.SmoothDamp(mainCam.gameObject.transform.position,
                targetLocation, ref panVelocity, cameraPan.GetPanSpeed(), 100, 1);
        }
    }

    public void HandleAnimation(DialogueAnimation animation)
    {
        StartCoroutine(RunAnimation(animation));

        HandleNextEvents();
    }

    //Handles moving on to the next level/cutscene once the final dialogue of the mission is done
    public void HandleSceneTransition(DialogueMissionEnd nextMission)
    {
        SceneManager.LoadScene(nextMission.GetTargetScene());
    }



    /// <summary>
    /// WRAP-UP FUNCTIONS
    /// </summary>
    /// Finishes up running a character animation
    public IEnumerator RunAnimation(DialogueAnimation animation)
    {
        yield return animation.GetAnimationDelay();
        animation.GetTargetAnimator().animator.SetBool(animation.GetAnimation(), true);
        animation.GetTargetAnimator().animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return animation.GetAnimationTime();
        animation.GetTargetAnimator().animator.SetBool(animation.GetAnimation(), false);
        animation.GetTargetAnimator().animator.updateMode = AnimatorUpdateMode.Normal;
    }

    //Handles moving to the next dialogue event once the animation time is up
    public IEnumerator TurnPanOff(DialogueCameraPan pan)
    {
        yield return pan.GetAnimationTime();

        earthPlayer.ToggleWaiting(false);
        panningOn = false;
        if (!dialoguePan)
        {
            HandleNextEvents();
        }
    }

    public IEnumerator TurnPanOff(DialoguePanAndText pan)
    {
        yield return pan.dialogueCameraPan.GetAnimationTime();

        earthPlayer.ToggleWaiting(false);
        panningOn = false;
    }

    /// <summary>
    /// HELPER FUNCTIONS
    /// </summary>
    /// <param name="currentLine"></param>

    //When displaying a dialogue line, handles the sprites
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

    //display letter by letter
    IEnumerator TypeSentence(TextMeshProUGUI dialogueArea, string line)
    {
        foreach (char letter in line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return null;
        }
    }

    //Handles setting the camera back to its previous state when the dialogue is over
    private void ReturnCameraToOrigin()
    {
        //The camera depth/zoom amount
        if (mainCam.orthographicSize < 59)
        {
            returningToOrigin = true;
            mainCam.orthographicSize = Mathf.SmoothDamp(mainCam.orthographicSize, 60, ref zoomVelocity, 60, 100, 1);
        }
        //The position of the camera/point it is focused over
        if (mainCam.gameObject.transform.localPosition != Vector3.zero)
        {
            returningToOrigin = true;
            mainCam.gameObject.transform.localPosition = Vector3.SmoothDamp(mainCam.gameObject.transform.localPosition, Vector3.zero, ref panVelocity, 60, mainCam.gameObject.transform.position.magnitude / 30, 1);
        }
        //If we're basically on top of the spot we want to go, stop moving
        else if ((mainCam.orthographicSize) > 59 && (mainCam.gameObject.transform.localPosition.magnitude < 0.5))
        {
            mainCam.orthographicSize = 60;
            mainCam.gameObject.transform.localPosition = Vector3.zero;
            returningToOrigin = false;
        }
    }
}