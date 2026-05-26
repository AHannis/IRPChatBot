using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour


#region Variables, References, Editable
{
    public AudioSource audioSource;

    public AudioClip sendSound;

    public AudioClip receiveSound;

    //handles conversation analysis in seperate script 
    public ConversationAnalyser analyser;
    public GameObject offlineIndicator;
    //converts emojis
    public EmojiParser emojiParser;
    [Header("Debug Showcase")]

    public bool forceComfortState;

    public bool forceConcernState;

    public bool forceGamingState;

    public bool forceUniState;

    public bool forceSchoolState;

    public bool forceWorkState;

    public bool forceFamilyState;

    public bool forceFoodState;

    public bool forceJokeState;

    public bool forceRoleplayState;

    public bool forceSingingState;

    public bool forceGoodbyeState;

    public bool forceFunnyStory = false;
    //tracks whether user has finished initial intro flow
    public bool completedIntro = false;

    //stores timestamp of last user message for pacing
    public float lastUserMessageTime = 0f;

    //controls typing animation + fake human timing
    public TypingDots typingDots;

    //handles reflective responses + topic extraction
    public TypingRef typingRef;

    //handles saving/loading old conversations
    public ConversationPersistence persistence;

    //cleans responses + prevents repetition
    public ResponseProcessor responseProcessor;

    //stores long term memory + slang learning
    public MemorySystem memorySystem;

    //creates chat bubbles in ui
    public MessageUIManager uiManager;

    //main response generation system
    public ChatBrain brain;

    //stores relationship level, mood, username etc
    public PlayerDataManager playerData;

    public TMP_InputField inputField;

    float idleTimer = 0f;

    bool waitingForReply = false;

    bool followUpAlreadySent = false;

    public float followUpDelay = 5f;

    [HideInInspector]
    public string lastBotMessage = "";

    //different emotional moods bot can enter
    public enum Mood
    {
        Neutral,
        Happy,
        Concerned,
        Playful,
        Tired
    }

    //stores active state in state machine
    public ChatState currentState;

    //tracks responses since last bot question
    public int exchangesSinceQuestion = 0;

    //controls emotional persistence
    public int emotionPersistence = 0;

    //prevents emotional loops repeating forever
    public int emotionLoops = 0;

    //used for spaced out callbacks
    public int exchangesSinceFamilyQuestion = 10;

    public int exchangesSinceGamingQuestion = 10;

    public int exchangesSinceVisitQuestion = 10;

    //total amount of sent user messages
    public int totalMessagesSent = 0;

    //tracks story progress
    public int activeStoryStep = 0;

    //tracks conversation context progression
    public int contextStep = 0;

    //prevents repeated age asking
    public bool askedAgeRecently = false;

    //tracks if user agreed to hear a story
    public bool awaitingFunnyStory = false;

    //waiting for user to explain slang
    public bool waitingForSlangExplanation = false;

    //prevents overlapping responses
    public bool isProcessingResponse = false;

    //tracks if bot is inside a story sequence
    public bool storyActive = false;

    //stores slang word awaiting explanation
    public string pendingSlangWord = "";

    //stores emotional state keywords
    public string lastEmotion = "";

    //tracks current + previous topics
    public string lastTopic = "";

    public string previousTopic = "";

    //stores last sent messages
    public string lastUserMessage = "";

    public string lastAIMessage = "";

    //stores current active conversational context
    public string currentConversationContext = "";

    //stores active uncle story
    public string activeStory = "";

    //used for anti spam send timing
    public float lastMessageTime = 0f;

    //chance of memory callback happening
    public float callbackChance = 0.2f;

    //tracks how chaotic conversations become
    public float chaosLevel = 0f;

    //short term recent message memory
    public List<string> recentMessages =
        new List<string>();

    //tracks recent bot replies to avoid repetition
    public List<string> recentAIReplies =
        new List<string>();

    //stores important remembered topics
    public List<string> rememberedTopics =
        new List<string>();

    [HideInInspector]
    public bool awaitingTopicShift = false;

    [HideInInspector]
    public string lastQuestionTopic = "";
    #endregion 
    void Awake()
    {
        brain =
            GetComponent<ChatBrain>();

        analyser =
            GetComponent<ConversationAnalyser>();
    }

    
 #region Loads Memories, Intro or Reconnect
    void Start()
    {
        memorySystem.LoadMemories();

        persistence.LoadConversation(
            this
        );

        uiManager.SetupScrollListener();

        playerData.LoadPlayerData();

        if (
            !PlayerPrefs.HasKey(
                "HasChattedBefore"
            )
        )
        {
            PlayerPrefs.SetInt(
                "HasChattedBefore",
                1
            );

            ChangeState(
                new IntroState(this)
            );
        }
        else
        {
            ChangeState(
                new ReconnectState(this)
            );
        }
    }

    //enter sends message unless shift is held
    void Update()
    {
        if (
            Input.GetKeyDown(
                KeyCode.Return
            )
            && !Input.GetKey(
                KeyCode.LeftShift
            )
        )
        {
            OnSendButton();
        }

        //idle followup timer
        if (
            waitingForReply
            && !isProcessingResponse
            && !followUpAlreadySent
            && string.IsNullOrWhiteSpace(
                inputField.text
            )
        )
        {
            idleTimer += Time.deltaTime;

            if (
                idleTimer >= followUpDelay
            )
            {
                waitingForReply = false;
                followUpAlreadySent = false;

                idleTimer = 0f;

                StartCoroutine(
                    SendIdleFollowUp()
                );
            }
        }
    }


    //saves player data when object disabled
    void OnDisable()
    {
        playerData.SavePlayerData();

        memorySystem.SaveMemories();
    }

    //backup save when app closes
    void OnApplicationQuit()
    {
        playerData.SavePlayerData();

        memorySystem.SaveMemories();
    }

    //core state machine transition function
    public void ChangeState(
        ChatState newState
    )
    {
        if (
            currentState != null
        )
        {
            currentState.Exit();
        }

        currentState =
            newState;

        if (
            currentState != null
        )
        {
            currentState.Enter();
        }
    }

    //handles send button validation
    public void OnSendButton()
    {
        if (
            isProcessingResponse
        )
        {
            return;
        }

        //anti spam protection
        if (
            Time.time - lastMessageTime
            < 0.25f
        )
        {
            return;
        }

        lastMessageTime =
            Time.time;

        string message =
            inputField.text;

        if (
            string.IsNullOrWhiteSpace(
                message
            )
        )
        {
            return;
        }
        offlineIndicator.SetActive(
    false
);

        SendUserMessage(
            message
        );


        inputField.text = "";

        inputField.ActivateInputField();
    }
   

    public void SendUserMessage(
        string message
    )
    {
        offlineIndicator.SetActive(
            false
        );


        {
            //converts text emoji shortcuts
            message =
            emojiParser.ParseEmojiText(
                message
            );
            uiManager.CreateMessage(
                message,
                true,
                this
            );

            audioSource.PlayOneShot(
                sendSound
            );

            //saves user message
            persistence.SaveConversation(
                message,
                true
            );

            //stores recent messages
            recentMessages.Add(
                message
            );

            //cleans old messages from memory list
            if (
                recentMessages.Count > 10
            )
            {
                recentMessages.RemoveAt(
                    0
                );
            }

            lastUserMessageTime =
                Time.time;
            idleTimer = 0f;

            waitingForReply = false;

            lastUserMessage =
                message;

            previousTopic =
                lastTopic;

            totalMessagesSent++;

            //longer messages slightly increase chaos score
            if (
                message.Length > 60
            )
            {
                chaosLevel += 0.15f;
            }

            //starts bot response coroutine
            StartCoroutine(
                ProcessAIResponse(
                    message
                )
            );
        }
        #endregion 
  
 #region Bot Response Pipeline
        IEnumerator ProcessAIResponse(
            string input
        )
        {
            //prevents overlapping bot responses
            if (
                isProcessingResponse
            )
            {
                yield break;
            }

            isProcessingResponse = true;

            if (
                currentState == null
            )
            {


                yield break;
            }

            //calculates fake human thinking delay
            float thinkTime =
                typingDots.CalculateThinkTime(
                    input
                );

            //emotional conversations take longer
            if (
                currentState is ComfortState
                || currentState is ConcernState
            )
            {
                thinkTime +=
                    Random.Range(
                        0.4f,
                        1.2f
                    );
            }

            //storytelling gets slight extra pause
            if (
                currentState is UncleStoryState
            )
            {
                thinkTime +=
                    Random.Range(
                        0.2f,
                        0.6f
                    );
            }

            yield return new WaitForSeconds(
                thinkTime
            );
            #region Debug Forced States

            if (forceFunnyStory)
            {
                forceFunnyStory = false;

                ChangeState(
                    new UncleStoryState(this)
                );
            }

            else if (forceComfortState)
            {
                forceComfortState = false;

                ChangeState(
                    new ComfortState(this)
                );
            }

            else if (forceConcernState)
            {
                forceConcernState = false;

                ChangeState(
                    new ConcernState(this)
                );
            }

            else if (forceGamingState)
            {
                forceGamingState = false;

                ChangeState(
                    new GamingState(this)
                );
            }

            else if (forceUniState)
            {
                forceUniState = false;

                ChangeState(
                    new UniState(this)
                );
            }

            else if (forceSchoolState)
            {
                forceSchoolState = false;

                ChangeState(
                    new SchoolState(this)
                );
            }

            else if (forceWorkState)
            {
                forceWorkState = false;

                ChangeState(
                    new WorkState(this)
                );
            }

            else if (forceFamilyState)
            {
                forceFamilyState = false;

                ChangeState(
                    new FamilyState(this)
                );
            }

            else if (forceFoodState)
            {
                forceFoodState = false;

                ChangeState(
                    new FoodState(this)
                );
            }

            else if (forceJokeState)
            {
                forceJokeState = false;

                ChangeState(
                    new JokeState(this)
                );
            }

            else if (forceRoleplayState)
            {
                forceRoleplayState = false;

                ChangeState(
                    new RoleplayState(this)
                );
            }

            else if (forceSingingState)
            {
                forceSingingState = false;

                ChangeState(
                    new SingingState(this)
                );
            }

            else if (forceGoodbyeState)
            {
                forceGoodbyeState = false;

                ChangeState(
                    new GoodbyeState(this)
                );
            }

            #endregion
            string reply = "";

            //detects weird/slang user input
            if (
                analyser.ContainsWeirdInput(
                    input.ToLower(),
                    this
                )
            )
            {
                string detected =
                    analyser.GetDetectedWeirdWord(
                        input.ToLower()
                    );

                //checks if slang already remembered
                if (
                    memorySystem.HasMemory(
                        "slang_" + detected
                    )
                )
                {
                    reply = RandomChoice(
                        "wait i've heard that one before",

                        detected
                        + " means "
                        + memorySystem.Recall(
                            "slang_" + detected
                        )
                        + " right?",

                        "see i'm learning your weird language now",

                        "oh i actually remember that one now"
                    );
                }
                else
                {
                    //stores slang waiting for explanation
                    waitingForSlangExplanation =
                        true;

                    pendingSlangWord =
                        detected;

                    reply = RandomChoice(
                        "is that some kind of slang now?",

                        "right you've completely lost me there",

                        "what does "
                        + pendingSlangWord
                        + " even mean?",

                        "i'm too old for this conversation"
                    );
                }
            }
            else
            {
                if (
                    analyser.IsGoodbye(
                        input.ToLower()
                    )
                )
                {
                    reply =
                        brain.GetGoodbyeReply();
                    yield return new WaitForSeconds(
        2f
    );

                    offlineIndicator.SetActive(
                        true
                    );
                }

                else if (
                    analyser.IsResolutionResponse(
                        input.ToLower()
                    )
                )
                {
                    reply =
                        RandomChoice(
                            "OH thank god honestly",
                            "right so you survived then",
                            "that's actually impressive honestly",
                            "finally free from coursework prison",
                            "bet that felt good honestly",
                            "look at you actually finishing things",
                            "honestly that's probably a relief"
                        );
                }

                else
                {
                    string reflectiveReply = "";

                    //small chance of reflective response
                    if (
                        Random.value < 0.1f
                    )
                    {
                        reflectiveReply =
                            typingRef.GenerateReflectiveResponse(
                                input,
                                this
                            );
                    }

                    if (
                        !string.IsNullOrEmpty(
                            reflectiveReply
                        )
                    )
                    {
                        reply =
                            reflectiveReply;
                    }
                    else
                    {
                        reply =
                            currentState
                            .HandleInput(
                                input
                            );
                    }
                }
            }
            #endregion

 #region Fallback Response
            if (
                string.IsNullOrEmpty(
                    reply
                )
            )
            {
                reply =
                    brain.GetNaturalReply();
            }

            reply =
                responseProcessor.ProcessResponse(
                    reply,
                    this
                );

            //adds fake human typos
            reply =
                typingDots.AddOccasionalTypo(
                    reply
                );

            //prevents exact repeated bot replies
            if (
                recentAIReplies.Contains(
                    reply
                )
                && Random.value < 0.7f
            )
            {
                reply =
                    brain.GetNaturalReply();
            }

            //stores bot reply history
            recentAIReplies.Add(
                reply
            );

            if (
                recentAIReplies.Count > 10
            )
            {
                recentAIReplies.RemoveAt(
                    0
                );
            }

            lastAIMessage =
                    reply;

            lastBotMessage =
                reply;

            //controls typing duration
            int loops =
                typingDots.CalculateTypingLoops(
                    reply
                );

            yield return StartCoroutine(
                typingDots.PlayTyping(
                    loops
                )
            );

            //creates bot message bubble
            uiManager.CreateMessage(
                reply,
                false,
                this
            );
            audioSource.PlayOneShot(
                receiveSound
                );

            //saves bot response
            persistence.SaveConversation(
                reply,
                false
            );

            //occasionally sends typo correction followup
            if (
                typingDots.HasCorrection()
                && Random.value < 0.8f
            )
            {
                yield return new WaitForSeconds(
                    Random.Range(
                        0.4f,
                        1.2f
                    )
                );

                string correction =
                    typingDots.GetCorrection();

                uiManager.CreateMessage(
                    correction,
                    false,
                    this
                );

                persistence.SaveConversation(
                    correction,
                    false
                );
            }

            //starts waiting for user reply
            idleTimer = 0f;

            waitingForReply =
                !analyser.ContainsAny(
                    reply.ToLower(),
                    "bye",
                    "later",
                    "see you",
                    "goodnight",
                    "go get some sleep"
                );

            isProcessingResponse = false;
        }
    }
    #endregion 
  
 #region Calls Follow Up, Analyser & Chat Brain
    //instantly sends bot message without typing delay
    public void SendAIImmediate(
    string message
)
{
    uiManager.CreateMessage(
        message,
        false,
        this
    );

    persistence.SaveConversation(
        message,
        false
    );
}

//randomly selects one response from list
public string RandomChoice(
    params string[] options
)
{
    return brain.RandomChoice(
        options
    );
}

//general followup generator
public string GetGeneralFollowUp()
{
    return brain.GetGeneralFollowUp();
}

//natural fallback conversation response
public string GetNaturalReply()
{
    return brain.GetNaturalReply();
}

//softly changes conversation topic
public string GetSoftTopicShift()
{
    return brain.GetSoftTopicShift();
}

//lead in phrase before topic changes
public string GetTopicShiftLeadIn()
{
    return brain.GetTopicShiftLeadIn();
}

//continues currently active uncle story
public string ContinueActiveStory()
{
    return brain.ContinueActiveStory();
}

//random life event storytelling system
public string GetRandomLifeEvent()
{
    return brain.GetRandomLifeEvent();
}

//memory callback system for older topics
public string GetRandomMemoryCallback()
{
    return brain.GetRandomMemoryCallback();
}

//gets random followup topic
public string GetFollowUpTopic()
{
    return brain.GetGeneralFollowUp();
}

//checks for gaming related keywords
public bool ContainsGamingTerms(
    string lower
)
{
    return brain.ContainsGamingTerms(
        lower
    );
}

//checks university keywords
public bool ContainsUniTerms(
    string lower
)
{
    return brain.ContainsUniTerms(
        lower
    );
}

//checks school keywords
public bool ContainsSchoolTerms(
    string lower
)
{
    return brain.ContainsSchoolTerms(
        lower
    );
}

//checks for hobby  keywords
public bool ContainsHobbyTerms(
    string lower
)
{
    return brain.ContainsHobbyTerms(
        lower
    );
}

//checking if active conversation context exists
public bool HasActiveContext()
{
    return
        !string.IsNullOrEmpty(
            currentConversationContext
        );
}

//sets active conversational context
public void SetContext(
    string context
)
{
    currentConversationContext =
        context;

    contextStep = 0;
}

//clears active conversation context
public void ClearContext()
{
    currentConversationContext =
        "";

    contextStep = 0;
}

//occasionally injects username naturally into replies
public string MaybeAddName(
    string text
)
{
    if (
        string.IsNullOrEmpty(
            playerData.userName
        )
        || Random.value > 0.18f
    )
    {
        return text;
    }

    return RandomChoice(
        playerData.userName + ", " + text,
        text + " " + playerData.userName,
        text + " though " + playerData.userName
    );
}

//helper function for keyword matching
public bool ContainsAny(
    string input,
    params string[] words
)
{
    return analyser.ContainsAny(
        input,
        words
    );
}
    #endregion 

 #region Resets State & Memory
    public void ResetChat()
{
    StopAllCoroutines();

    PlayerPrefs.DeleteAll();

    PlayerPrefs.Save();

    uiManager.ClearMessages();

    recentMessages.Clear();

    recentAIReplies.Clear();

    rememberedTopics.Clear();

    typingRef.ClearRecentTopics();

    memorySystem.ClearMemories();

    playerData.ResetPlayerData();

    lastTopic = "";

    previousTopic = "";

    currentConversationContext =
        "";

    totalMessagesSent = 0;

    chaosLevel = 0f;

    ChangeState(
        new IntroState(this)
    );
}
    #endregion

 #region Checks For User Keywords
    //checks if user recently mentioned keyword
    public bool RecentlyMentioned(
    string key
)
{
    return
        lastUserMessage
        .ToLower()
        .Contains(
            key.ToLower()
        );
}
    #endregion
    //gets continuation for previous conversations
    public string GetConversationContinuation()
{
    return brain.GetConversationContinuation();
}
    //detects responses
    #region
    //emoji shortcut conversion system
    public string Emoji(
    string emojiName
)
{
    switch (emojiName)
    {
        case "smile":
            return "\U0001F642";

        case "thumbsup":
            return "\U0001F44D";

        case "laugh":
            return "\U0001F602";

        case "cry":
            return "\U0001F62D";

        case "awkward":
            return "\U0001F605";

        case "facepalm":
            return "\U0001F926";

        case "thinking":
            return "\U0001F914";
    }

    return "";
}

//extracts important topic phrases from input
public string ExtractTopic(
    string input
)
{
    return typingRef.ExtractTopic(
        input
    );
}

//detects short user replies
public bool IsShortReply(
    string input
)
{
    return analyser.IsShortReply(
        input
    );
}

//gets dynamic followup responses
public string GetDynamicFollowUp()
{
    return brain.GetGeneralFollowUp();
}

//generates reflective ai responses
public string GenerateReflectiveResponse(
    string input
)
{
    return typingRef.GenerateReflectiveResponse(
        input,
        this
    );
}

//checks for reciprocal conversation replies
public bool IsReciprocalResponse(
    string input
)
{
    return analyser.IsReciprocalResponse(
        input
    );
}

//detecting absurd/fantasy style inputs
public bool IsAbsurdInput(
    string input
)
{
    return analyser.ContainsAny(
        input.ToLower(),
        "vampire",
        "alien",
        "wizard",
        "immortal",
        "ghost",
        "demon",
        "time traveller"
    );
}

//detects activity based replies
public bool IsLikelyActivityResponse(
    string input
)
{
    string lower =
        input.ToLower();

    return
        ContainsAny(
            lower,
            "working",
            "work",
            "job",
            "busy",
            "gaming",
            "watching",
            "playing",
            "studying",
            "sleeping",
            "eating"
        )
        || lower.Split(' ').Length <= 5;
}

//small chance for intentional ai misread behaviour
public bool ShouldMisread()
{
    return
        playerData.relationshipLevel > 5
        && Random.value < 0.015f;
}
    #endregion
    //properties for player data system e.g., mood, relatuonship level, user age etc
    #region
    public Mood currentMood
{
    get
    {
        return playerData.currentMood;
    }
    set
    {
        playerData.currentMood = value;
    }
}

public int relationshipLevel
{
    get
    {
        return playerData.relationshipLevel;
    }
    set
    {
        playerData.relationshipLevel = value;
    }
}

public int userAge
{
    get
    {
        return playerData.userAge;
    }
    set
    {
        playerData.userAge = value;
    }
}

public bool knowsUserAge
{
    get
    {
        return playerData.knowsUserAge;
    }
    set
    {
        playerData.knowsUserAge = value;
    }
}

public string userName
{
    get
    {
        return playerData.userName;
    }
    set
    {
        playerData.userName = value;
    }
}

public string lifeStage
{
    get
    {
        return playerData.lifeStage;
    }
    set
    {
        playerData.lifeStage = value;
    }
}

//memory lookup helpers
public bool HasMemory(
    string key
)
{
    return memorySystem.HasMemory(
        key
    );
}

public string Recall(
    string key
)
{
    return memorySystem.Recall(
        key
    );
}

//stores memory values into memory system
public void Remember(
    string key,
    string value
)
{
    memorySystem.Remember(
        key,
        value
    );
}

IEnumerator SendIdleFollowUp()
{
    yield return new WaitForSeconds(
        Random.Range(
            0.5f,
            1.5f
        )
    );

    string followUp = "";

    switch (lastTopic)
    {
        case "relationship":

            followUp =
                RandomChoice(
                    "so how did that even happen then",
                    "you seem happy talking about that honestly",
                    "how long has that been going on then",
                    "wasn't expecting you to become romantic"
                );

            break;

        case "uni":

            followUp =
                RandomChoice(
                    "coursework still destroying you?",
                    "what's been the worst assignment lately",
                    "student life sounds relentless honestly"
                );

            break;

        case "gaming":

            followUp =
                RandomChoice(
                    "what've you been playing lately then",
                    "still addicted to games i assume",
                    "what game has consumed your life recently"
                );

            break;

        default:

            followUp =
                RandomChoice(
                    "what else has been going on lately?",
                    "you keeping busy lately?",
                    "life been alright recently?",
                    "anything interesting been happening?",
                    "what've you been focused on lately?"
                );

            break;

    }

    int loops =
        typingDots.CalculateTypingLoops(
            followUp
        );

    yield return StartCoroutine(
        typingDots.PlayTyping(
            loops
        )
    );

    uiManager.CreateMessage(
        followUp,
        false,
        this
    );

    persistence.SaveConversation(
        followUp,
        false
    );
    lastAIMessage =
        followUp;

    lastBotMessage =
        followUp;

    idleTimer = 0f;
    followUpAlreadySent = true;
}
}
#endregion