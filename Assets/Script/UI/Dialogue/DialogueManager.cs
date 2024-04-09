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
    private DialogueEvent activeEvent;
    private DialogueMoveEvent currentMove;
    private DialogueCameraPan currentPan;

    private Coroutine panRoutine = null;
    private Coroutine moveRoutine = null;
    private Coroutine typeRoutine = null;

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

    public float typingSpeed = 0.25f;
    bool midTyping = false;
    bool eventEnded = true;

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

    [SerializeField] private bool panningOn = false;
    [SerializeField] private bool returningToOrigin = false;
    private Vector3 moveVelocity;
    private float turnVelocity;
    private bool dialoguePan;

    private bool movingOn = false;

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
            HandleCameraPan(currentPan);
        }
        if (returningToOrigin)
        {
            ReturnCameraToOrigin();
        }
        if (movingOn)
        {
            HandleMoving(currentMove);
        }
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

        if (earthPlayer == null || celestialPlayer == null)
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

        eventEnded = true;
        HandleNextEvents();

    }

    public void HandleDialogueContinue()
    {

        if (!panningOn && !returningToOrigin)
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
        activeEvent = currentEvent;

        if (midTyping)
        {
            Debug.Log("mid typing");
            HaltTyping();
        }
        //If we can move ahead with the next event
        else if (eventEnded || currentEvent.GetIsSkippable())
        {
            //If they in fact skipped, make sure we set all the stuff that was supposed to happen last event to the end
            if (!eventEnded)
            {
                HaltCoroutines();
                eventEnded = true;
                return;
            }
            eventEnded = false;

            //Handle how to start the next event
            if (currentEvent is DialogueLine)
            {
                ToggleDialogueBox(true);
                DisplayNextDialogueLine((DialogueLine)currentEvent);
            }
            else if (currentEvent is DialogueCameraPan)
            {
                ToggleDialogueBox(false);
                DialogueCameraPan pan = currentEvent as DialogueCameraPan;
                dialoguePan = false;
                panningOn = true;
                earthPlayer.ToggleWaiting(true);
                currentPan = pan;
                panRoutine = StartCoroutine(TurnPanOff(currentPan));
            }
            else if (currentEvent is DialoguePanAndText)
            {
                ToggleDialogueBox(true);
                DialoguePanAndText panLineEvent = currentEvent as DialoguePanAndText;

                dialoguePan = true;
                panningOn = true;
                earthPlayer.ToggleWaiting(true);
                currentPan = panLineEvent.dialogueCameraPan;
                DisplayNextDialogueLine(panLineEvent.dialogueLine);
                panRoutine = StartCoroutine(TurnPanOff(currentPan));
            }
            else if (currentEvent is DialogueAnimation)
            {
                eventEnded = true;
                HandleAnimation((DialogueAnimation)currentEvent);
            }
            else if (currentEvent is DialogueMoveEvent)
            {
                Debug.Log("starting move");
                movingOn = true;

                currentMove = currentEvent as DialogueMoveEvent;
                if (!currentMove.MovePlaysOut())
                {
                    eventEnded = true;
                    HandleNextEvents();
                }
                moveRoutine = StartCoroutine(TurnMoveOff(currentMove));
            }
            else if (currentEvent is DialogueMissionEnd)
            {
                HandleSceneTransition((DialogueMissionEnd)currentEvent);
            }

        }
    }

    //Wraps up the dialogue when we run out of events
    public void EndDialogue()
    {
        HaltCoroutines();
        HaltTyping();
        ReturnCameraToOrigin();

        if (dialogueBox.activeSelf) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
        }
        // Toggle other UI elements visibility
        uiController.ToggleUIForDialogue(true); // Pass true to reactivate other UI elements

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
    //When displaying a dialogue line, handles the text
    public void DisplayNextDialogueLine(DialogueLine currentLine)
    {
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
                typeRoutine = StartCoroutine(TypeSentence(dialogueArea, currentLine.line));
                break;
            case UserSettingsManager.GameLanguage.FRENCH:
                // Display French dialogue
                typeRoutine = StartCoroutine(TypeSentence(dialogueArea, currentLine.lineFR));
                break;
        }
        eventEnded = true;
    }


    //This runs if the dialogue event is a camera pan, handles the movement
    public void HandleCameraPan(DialogueCameraPan pan)
    {
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

    public void HandleAnimation(DialogueAnimation animation)
    {
        StartCoroutine(RunAnimation(animation));
        HandleNextEvents();
    }

    public void HandleMoving(DialogueMoveEvent moveEvent)
    {
        Debug.Log("Moving");
        GameObject objectToMove = moveEvent.GetObjectToMove();
        if (moveEvent.HasMove())
        {
            if (moveEvent.IsObjectMoveType())
            {
                objectToMove.transform.position = Vector3.SmoothDamp(objectToMove.transform.position,
                    moveEvent.GetObjectToMoveTo().transform.position, ref moveVelocity, moveEvent.GetMovementSpeed(), 100, 1);
            }
            else if (moveEvent.IsLocationMoveType())
            {
                objectToMove.transform.position = Vector3.SmoothDamp(objectToMove.transform.position, moveEvent.GetPosition(), ref moveVelocity, moveEvent.GetMovementSpeed(), 100, 1);
            }
        }
        if (moveEvent.HasRotation())
        {
            objectToMove.transform.rotation = Quaternion.RotateTowards(objectToMove.transform.rotation, moveEvent.GetRotation(), moveEvent.GetRotationSpeed());
        }

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
        currentPan = null;
        if (!(activeEvent is DialoguePanAndText))
        {
            eventEnded = true;
            HandleNextEvents();
        }
    }

    public IEnumerator TurnMoveOff(DialogueMoveEvent moveEvent)
    {
        Debug.Log("Starting move turn off");
        if (moveEvent.MovePlaysOut())
        {
            earthPlayer.ToggleWaiting(true);
        }

        yield return moveEvent.GetAnimationTimer();

        if (moveEvent.MovePlaysOut())
        {
            eventEnded = true;
            earthPlayer.ToggleWaiting(false);
            HandleNextEvents();
        }
        Debug.Log("Finishing up move");
        movingOn = false;
        currentMove = null;
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
        midTyping = true;
        dialogueArea.text = ""; // Clear the text initially
        foreach (char letter in line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); // Wait for typingSpeed seconds before showing the next letter
        }
        midTyping = false;
        eventEnded = true;
    }

    private void HaltCoroutines()
    {
        if (panRoutine != null && panningOn)
        {
            HaltPanning();
            StopCoroutine(panRoutine);
            panRoutine = null;
        }
        if (typeRoutine != null && midTyping)
        {
            HaltTyping();
            StopCoroutine(typeRoutine);
            typeRoutine = null;
            midTyping = false;
        }
        if (moveRoutine != null && movingOn)
        {
            HaltMove();
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
        eventEnded = true;
    }

    //Instantly finish moving to the target location
    private void HaltPanning()
    {
        zoom = mainCam.orthographicSize;
        //zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        mainCam.orthographicSize = currentPan.GetZoomAmount();

        if (currentPan.GetPanType() == DialogueCameraPan.PanType.OBJECT)
        {
            mainCam.gameObject.transform.position = currentPan.GetPanObject().transform.position;
        }
        else if (currentPan.GetPanType() == DialogueCameraPan.PanType.LOCATION)
        {
            mainCam.gameObject.transform.position = currentPan.GetPanLocation();
        }

        panningOn = false;
        currentPan = null;
    }

    private void HaltTyping()
    {
        if (activeEvent.GetIsSkippable())
        {
            DialogueLine currentLine = null;
            if (currentEvent is DialogueLine)
            {
                currentLine = (DialogueLine)currentEvent;
            }
            else if(currentEvent is DialoguePanAndText)
            {
                DialoguePanAndText text = currentEvent as DialoguePanAndText;
                currentLine = text.dialogueLine;
            }
            
            if(currentLine != null)
            {
                switch (userSettingsManager.chosenLanguage)
                {
                    case UserSettingsManager.GameLanguage.ENGLISH:
                        // Display English dialogue
                        StopCoroutine(typeRoutine);
                        dialogueArea.text = currentLine.line;
                        break;
                    case UserSettingsManager.GameLanguage.FRENCH:
                        // Display French dialogue
                        StopCoroutine(typeRoutine);
                        dialogueArea.text = currentLine.lineFR;
                        break;
                }
            }
            
            midTyping = false;
        }
    }

    private void HaltMove()
    {
        if (currentMove != null)
        {
            if (currentMove.HasMove())
            {
                if (currentMove.IsLocationMoveType())
                {
                    currentMove.GetObjectToMove().transform.position = currentMove.GetPosition();
                }
                else if (currentMove.IsObjectMoveType())
                {
                    currentMove.GetObjectToMove().transform.position = currentMove.GetObjectToMoveTo().transform.position;
                }
            }

            if (currentMove.HasRotation())
            {
                currentMove.GetObjectToMove().transform.rotation = currentMove.GetRotation();
            }
        }
        movingOn = false;
        currentMove = null;
    }

    private void ToggleDialogueBox(bool shouldBeActive)
    {
        if (!dialogueBox.activeSelf && shouldBeActive) // Check if the dialogue box is currently active
        {
            dialogueBox.SetActive(true); // Activate the dialogue box
            isDialogueActive = true; // Set the dialogue state to active
        }
        else if (dialogueBox.activeSelf && !shouldBeActive)
        {
            dialogueBox.SetActive(false); // Deactivate the dialogue box
            isDialogueActive = false; // Set the dialogue state to inactive
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