using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    private DialogueLine currentLineEvent;

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
    bool eventEnded = true;

    //Camera speed variables
    //Zoom variables, zooming in and out
    private float zoomVelocity = 0f;

    //Pan variables, moving around the map
    private Vector3 panVelocity = new Vector3(0, 0, 0);

    //private float smoothTime = 0.25f;

    [SerializeField] private bool panningOn = false;
    [SerializeField] private bool returningToOrigin = false;
    private Vector3 moveVelocity;

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
            ContinueCameraPan(currentPan);
        }
        if (returningToOrigin)
        {
            ReturnCameraToOrigin();
        }
        if (movingOn)
        {
            ContinueMoving(currentMove);
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

        //If this somehow runs while the manager doesn't have a reference to the players
        if (earthPlayer == null || celestialPlayer == null)
        {
            //Set them now
            SetReferences();
        }

        //Make sure any other controls the earth player is using are turned off
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

    //The function called by player input to continue dialogue
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
        //If there aren't actually any events, eject immediately
        if (events.Count == 0)
        {
            EndDialogue();

            return;
        }

        //If the player is trying to proceed to the next event or line
        //Check if it is skippable, and if we're in the middle of anything
        if(currentEvent != null)
        {
            if (currentEvent.GetIsSkippable() && !eventEnded)
            {
                //We want to immediately finish the action that was happening
                HaltCoroutines();
                return;
            }  
        }

        //If we can move ahead with the next event
        if (eventEnded)
        {
            currentEvent = events.Dequeue();
            activeEvent = currentEvent;
            eventEnded = false;

            //Handle how to start the next event
            ActionHandler();
        }
    }

    //Handles how to proceed based on which type the next event is
    private void ActionHandler()
    {
        if (currentEvent is DialogueLine)
        {
            ToggleDialogueBox(true);
            currentLineEvent = currentEvent as DialogueLine;
            DisplayNextDialogueLine((DialogueLine)currentEvent);
        }
        else if (currentEvent is DialogueCameraPan)
        {
            ToggleDialogueBox(false);
            DialogueCameraPan pan = currentEvent as DialogueCameraPan;
            panningOn = true;
            earthPlayer.ToggleWaiting(true);
            currentPan = pan;
            panRoutine = StartCoroutine(TurnPanOff(currentPan));
        }
        else if (currentEvent is DialoguePanAndText)
        {
            ToggleDialogueBox(true);
            DialoguePanAndText panLineEvent = currentEvent as DialoguePanAndText;

            panningOn = true;
            earthPlayer.ToggleWaiting(true);
            currentPan = panLineEvent.dialogueCameraPan;
            currentLineEvent = panLineEvent.dialogueLine;
            DisplayNextDialogueLine(currentLineEvent);
            panRoutine = StartCoroutine(TurnPanOff(currentPan));
        }
        else if (currentEvent is DialogueAnimation)
        {
            eventEnded = true;
            HandleAnimation((DialogueAnimation)currentEvent);
        }
        else if (currentEvent is DialogueMoveEvent)
        {
            movingOn = true;

            currentMove = currentEvent as DialogueMoveEvent;
            //If the move is allowed to play simultaneously over other actions
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

    //Wraps up the dialogue when we run out of events
    public void EndDialogue()
    {
        if(this != null)
        {
            if(!(currentEvent is DialogueMissionEnd))
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

                //Somewhere in this function we need to check if the last item in the event list is a mission end
            }
            
        }
        
    }



    /// <summary>
    /// FUNCTIONS TO START EACH TYPE OF DIALOGUE EVENT
    /// </summary>
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

        if(typeRoutine != null)
        {
            typeRoutine = null;
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

        /*
        if(currentEvent is DialogueLine)
        {
            eventEnded = true;
        }
        */
    }

    public void HandleAnimation(DialogueAnimation animation)
    {
        StartCoroutine(StartAnimation(animation));
        HandleNextEvents();
    }

    //Handles moving on to the next level/cutscene once the final dialogue of the mission is done
    public void HandleSceneTransition(DialogueMissionEnd nextMission)
    {
        SceneManager.LoadScene(nextMission.GetTargetScene());
    }



    /// <summary>
    /// FUNCTIONS TO CONTINUE EACH TYPE OF DIALOGUE EVENT
    /// </summary>
    //This runs if the dialogue event is a camera pan, handles the movement
    public void ContinueCameraPan(DialogueCameraPan pan)
    {
        //We want to switch to single screen for dialogue I think
        if (split.Manager == 1)
        {

        }

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

    public void ContinueMoving(DialogueMoveEvent moveEvent)
    {
        GameObject objectToMove = moveEvent.GetObjectToMove();
        if (moveEvent.HasMove())
        {
            if (moveEvent.IsObjectMoveType())
            {
                objectToMove.transform.position = Vector3.SmoothDamp(objectToMove.transform.position,
                    moveEvent.GetObjectToMoveTo().transform.position, ref moveVelocity, moveEvent.GetAnimationSpeed(), moveEvent.GetMovementSpeed(), 1);
            }
            else if (moveEvent.IsLocationMoveType())
            {
                objectToMove.transform.position = Vector3.SmoothDamp(objectToMove.transform.position, moveEvent.GetPosition(), ref moveVelocity, moveEvent.GetAnimationSpeed(), moveEvent.GetMovementSpeed(), 1);
            }
        }
        if (moveEvent.HasRotation())
        {
            objectToMove.transform.rotation = Quaternion.RotateTowards(objectToMove.transform.rotation, moveEvent.GetRotation(), moveEvent.GetRotationSpeed());
        }

    }

    //Sets the animation in motion based on any specified delay
    public IEnumerator StartAnimation(DialogueAnimation animation)
    {
        yield return animation.GetAnimationDelay();
        animation.GetTargetAnimator().animator.SetBool(animation.GetAnimation(), true);
        animation.GetTargetAnimator().animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(TurnOffAnimation(animation));
    }



    /// <summary>
    /// WRAP-UP FUNCTIONS
    /// </summary>
    /// Finishes up running a character animation
    public IEnumerator TurnOffAnimation(DialogueAnimation animation)
    {
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
        movingOn = false;
        currentMove = null;
    }



    ///
    /// STOPPING FUNCTIONS
    ///
    //Stops all dialogue coroutines, pushes them to completion
    private void HaltCoroutines()
    {
        if (panRoutine != null && panningOn)
        {
            HaltPanning();
            StopCoroutine(panRoutine);
            panRoutine = null;
        }
        if (typeRoutine != null)
        {
            HaltTyping();
            StopCoroutine(typeRoutine);
            typeRoutine = null;
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

    //Instantly completes the line that is being printed
    private void HaltTyping()
    {
        if (activeEvent.GetIsSkippable())
        {
            if (currentLineEvent != null)
            {
                switch (userSettingsManager.chosenLanguage)
                {
                    case UserSettingsManager.GameLanguage.ENGLISH:
                        // Display English dialogue
                        if (typeRoutine != null) StopCoroutine(typeRoutine);
                        dialogueArea.text = currentLineEvent.line;
                        break;
                    case UserSettingsManager.GameLanguage.FRENCH:
                        // Display French dialogue
                        if (typeRoutine != null) StopCoroutine(typeRoutine);
                        dialogueArea.text = currentLineEvent.lineFR;
                        break;
                }
            }
            currentLineEvent = null;
        }
    }

    //Instantly finishes moving the object to the destination
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
        dialogueArea.text = ""; // Clear the text initially
        foreach (char letter in line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); // Wait for typingSpeed seconds before showing the next letter
        }
        eventEnded = true;
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