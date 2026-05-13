using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ConversationAnalyser analyser;

    public EmojiParser emojiParser;

    public bool completedIntro = false;

    public TypingDots typingDots;

    public TypingRef typingRef;

    public ConversationPersistence persistence;

    public ResponseProcessor responseProcessor;

    public MemorySystem memorySystem;

    public MessageUIManager uiManager;

    public ChatBrain brain;

    public PlayerDataManager playerData;

    public TMP_InputField inputField;

    [HideInInspector]
    public string lastBotMessage = "";

    public enum Mood
    {
        Neutral,
        Happy,
        Concerned,
        Playful,
        Tired
    }

    public ChatState currentState;

    public int exchangesSinceQuestion = 0;

    public int emotionPersistence = 0;

    public int emotionLoops = 0;

    public int exchangesSinceFamilyQuestion = 10;

    public int exchangesSinceGamingQuestion = 10;

    public int exchangesSinceVisitQuestion = 10;

    public int totalMessagesSent = 0;

    public int activeStoryStep = 0;

    public int contextStep = 0;

    public bool askedAgeRecently = false;

    public bool awaitingFunnyStory = false;

    public bool waitingForSlangExplanation = false;

    public bool isProcessingResponse = false;

    public bool storyActive = false;

    public string pendingSlangWord = "";

    public string lastEmotion = "";

    public string lastTopic = "";

    public string previousTopic = "";

    public string lastUserMessage = "";

    public string lastAIMessage = "";

    public string currentConversationContext = "";

    public string activeStory = "";

    public float lastMessageTime = 0f;

    public float callbackChance = 0.2f;

    public float chaosLevel = 0f;

    public List<string> recentMessages =
        new List<string>();

    public List<string> recentAIReplies =
        new List<string>();

    public List<string> rememberedTopics =
        new List<string>();

    [HideInInspector]
    public bool awaitingTopicShift = false;

    [HideInInspector]
    public string lastQuestionTopic = "";

    void Awake()
    {
        brain =
            GetComponent<ChatBrain>();

        analyser =
            GetComponent<ConversationAnalyser>();
    }

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

    void Update()
    {
        if (
            Input.GetKeyDown(
                KeyCode.Return
            )
        )
        {
            OnSendButton();
        }
    }

    void OnDisable()
    {
        playerData.SavePlayerData();

        memorySystem.SaveMemories();
    }

    void OnApplicationQuit()
    {
        playerData.SavePlayerData();

        memorySystem.SaveMemories();
    }

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

    public void OnSendButton()
    {
        if (
            isProcessingResponse
        )
        {
            return;
        }

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
        message =
            emojiParser.ParseEmojiText(
                message
            );

        uiManager.CreateMessage(
            message,
            true,
            this
        );

        persistence.SaveConversation(
            message,
            true
        );

        recentMessages.Add(
            message
        );

        if (
            recentMessages.Count > 10
        )
        {
            recentMessages.RemoveAt(
                0
            );
        }

        lastUserMessage =
            message;

        previousTopic =
            lastTopic;

        StartCoroutine(
            ProcessAIResponse(
                message
            )
        );
    }

    IEnumerator ProcessAIResponse(
        string input
    )
    {
        isProcessingResponse = true;

        if (
            currentState == null
        )
        {
            isProcessingResponse = false;

            yield break;
        }

        yield return StartCoroutine(
     typingDots.PlayTyping(
         input,
         ""
     )
 );

        string reply = "";

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
            string reflectiveReply = "";

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
                && !(currentState is IntroState)
                && !(currentState is NameLoopState)
            )
            {
                reply =
                    reflectiveReply;
            }
            else
            {
                analyser.Analyse(
                    input
                );

                reply =
                    currentState
                    .HandleInput(
                        input
                    );
            }
        }

        reply =
            responseProcessor.ProcessResponse(
                reply,
                this
            );

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

        yield return StartCoroutine(
     typingDots.PlayTyping(
         input,
         reply
     )
 );
        uiManager.CreateMessage(
            reply,
            false,
            this
        );

        persistence.SaveConversation(
            reply,
            false
        );

        isProcessingResponse = false;
    }

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

    public string RandomChoice(
        params string[] options
    )
    {
        return brain.RandomChoice(
            options
        );
    }

    public string GetGeneralFollowUp()
    {
        return brain.GetGeneralFollowUp();
    }

    public string GetNaturalReply()
    {
        return brain.GetNaturalReply();
    }

    public string GetSoftTopicShift()
    {
        return brain.GetSoftTopicShift();
    }

    public string GetTopicShiftLeadIn()
    {
        return brain.GetTopicShiftLeadIn();
    }

    public string ContinueActiveStory()
    {
        return brain.ContinueActiveStory();
    }

    public string GetRandomLifeEvent()
    {
        return brain.GetRandomLifeEvent();
    }

    public string GetRandomMemoryCallback()
    {
        return brain.GetRandomMemoryCallback();
    }

    public string GetFollowUpTopic()
    {
        return brain.GetGeneralFollowUp();
    }

    public bool ContainsGamingTerms(
        string lower
    )
    {
        return brain.ContainsGamingTerms(
            lower
        );
    }

    public bool ContainsUniTerms(
        string lower
    )
    {
        return brain.ContainsUniTerms(
            lower
        );
    }

    public bool ContainsSchoolTerms(
        string lower
    )
    {
        return brain.ContainsSchoolTerms(
            lower
        );
    }

    public bool ContainsHobbyTerms(
        string lower
    )
    {
        return brain.ContainsHobbyTerms(
            lower
        );
    }

    public bool HasActiveContext()
    {
        return
            !string.IsNullOrEmpty(
                currentConversationContext
            );
    }

    public void SetContext(
        string context
    )
    {
        currentConversationContext =
            context;

        contextStep = 0;
    }

    public void ClearContext()
    {
        currentConversationContext =
            "";

        contextStep = 0;
    }

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
            text + " honestly " + playerData.userName,
            text + " though " + playerData.userName
        );
    }

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

        ChangeState(
            new IntroState(this)
        );
    }

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

    public string GetConversationContinuation()
    {
        return brain.GetConversationContinuation();
    }

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
    public string ExtractTopic(
    string input
)
    {
        return typingRef.ExtractTopic(
            input
        );
    }

    public bool IsShortReply(
        string input
    )
    {
        return analyser.IsShortReply(
            input
        );
    }

    public string GetDynamicFollowUp()
    {
        return brain.GetGeneralFollowUp();
    }

    public string GenerateReflectiveResponse(
        string input
    )
    {
        return typingRef.GenerateReflectiveResponse(
            input,
            this
        );
    }

    public bool IsReciprocalResponse(
        string input
    )
    {
        return analyser.IsReciprocalResponse(
            input
        );
    }

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
    public bool ShouldMisread()
    {
        return
            playerData.relationshipLevel > 5
            && Random.value < 0.015f;
    }
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
}