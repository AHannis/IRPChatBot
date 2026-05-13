using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public ConversationAnalyser analyser;

    public EmojiParser emojiParser;

    public TypingDots typingDots;

    public TypingRef typingRef;

    public ConversationPersistence persistence;

    public ResponseProcessor responseProcessor;

    public MemorySystem memorySystem;

    public MessageUIManager uiManager;

    public ChatBrain brain;

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

    public Mood currentMood =
        Mood.Neutral;

    public ChatState currentState;

    public int exchangesSinceQuestion = 0;

    public int relationshipLevel = 0;

    public int userAge = -1;

    public int emotionPersistence = 0;

    public int emotionLoops = 0;

    public int exchangesSinceFamilyQuestion = 10;

    public int exchangesSinceGamingQuestion = 10;

    public int exchangesSinceVisitQuestion = 10;

    public int totalMessagesSent = 0;

    public int activeStoryStep = 0;

    public int contextStep = 0;

    public bool knowsUserAge = false;

    public bool askedAgeRecently = false;

    public bool awaitingFunnyStory = false;

    public bool waitingForSlangExplanation = false;

    public bool isProcessingResponse = false;

    public bool storyActive = false;

    public string pendingSlangWord = "";

    public string lastEmotion = "";

    public string lifeStage = "";

    public string lastTopic = "";

    public string previousTopic = "";

    public string lastUserMessage = "";

    public string lastAIMessage = "";

    public string userName = "";

    public string currentConversationContext = "";

    public string activeStory = "";

    public float lastMessageTime = 0f;

    public float followUpCooldown = 0f;

    public float callbackChance = 0.2f;

    public float chaosLevel = 0f;

    public List<string> recentMessages =
        new List<string>();

    public List<string> recentAIReplies =
        new List<string>();

    public List<string> rememberedTopics =
        new List<string>();

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

        LoadPlayerData();

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
        SaveStateData();
    }

    void OnApplicationQuit()
    {
        SaveStateData();
    }

    void LoadPlayerData()
    {
        userName =
            PlayerPrefs.GetString(
                "UserName",
                ""
            );

        relationshipLevel =
            PlayerPrefs.GetInt(
                "RelationshipLevel",
                0
            );

        lastTopic =
            PlayerPrefs.GetString(
                "LastTopic",
                ""
            );

        currentMood =
            (Mood)PlayerPrefs.GetInt(
                "CurrentMood",
                0
            );

        userAge =
            PlayerPrefs.GetInt(
                "UserAge",
                -1
            );

        knowsUserAge =
            PlayerPrefs.GetInt(
                "KnowsUserAge",
                0
            ) == 1;

        lifeStage =
            PlayerPrefs.GetString(
                "LifeStage",
                ""
            );
    }

    void SaveStateData()
    {
        PlayerPrefs.SetInt(
            "RelationshipLevel",
            relationshipLevel
        );

        PlayerPrefs.SetString(
            "LastTopic",
            lastTopic
        );

        PlayerPrefs.SetInt(
            "CurrentMood",
            (int)currentMood
        );

        PlayerPrefs.SetInt(
            "UserAge",
            userAge
        );

        PlayerPrefs.SetInt(
            "KnowsUserAge",
            knowsUserAge ? 1 : 0
        );

        PlayerPrefs.SetString(
            "LifeStage",
            lifeStage
        );

        memorySystem.SaveMemories();

        PlayerPrefs.Save();
    }

    public void ChangeState(
        ChatState newState
    )
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState =
            newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void OnSendButton()
    {
        if (isProcessingResponse)
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

        SendUserMessage(message);

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
            recentMessages.RemoveAt(0);
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

        if (currentState == null)
        {
            isProcessingResponse = false;

            yield break;
        }

        float thinkTime =
            typingDots.CalculateThinkTime(
                input
            );

        yield return new WaitForSeconds(
            thinkTime
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

            if (Random.value < 0.1f)
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
                analyser.Analyse(
                    input
                );

                if (
                    IsLikelyName(input)

                    )
                    
                
                {
                    userName =
                        input.Trim();

                    PlayerPrefs.SetString(
                        "UserName",
                        userName
                    );

                    reply = RandomChoice(
                        "alright "
                        + userName
                        + ". i'll remember that",
                        "nice to properly meet you "
                        + userName,
                        "right got you "
                        + userName,
                        userName
                        + " honestly suits you"
                    );
                }
                else if (
                    analyser.IsShortReply(
                        input
                    )
                    && Random.value < 0.35f
                )
                {
                    reply = RandomChoice(
                        "fair",
                        "real",
                        "yeah i get you",
                        "that's rough",
                        "valid",
                        "sounds about right",
                        "can't blame you",
                        "damn",
                        "true"
                    );
                }
                else
                {
                    if (
                        !(currentState
                        is RouterState)
                    )
                    {
                        ChangeState(
                            new RouterState(
                                this
                            )
                        );
                    }

                    reply =
                        currentState
                        .HandleInput(
                            input
                        );
                }
            }
        }

        if (
            recentAIReplies.Contains(
                reply
            )
            || responseProcessor.StartsSimilar(
                reply,
                recentAIReplies
            )
        )
        {
            reply =
                GetNaturalReply();
        }

        reply =
            responseProcessor.ProcessResponse(
                reply,
                this
            );

        reply =
            typingDots.AddOccasionalTypo(
                reply
            );

        recentAIReplies.Add(
            reply
        );

        if (
            recentAIReplies.Count > 10
        )
        {
            recentAIReplies.RemoveAt(0);
        }

        lastAIMessage =
            reply;

        lastBotMessage =
            reply;

        int loops =
            typingDots.CalculateTypingLoops(
                reply
            );

        yield return StartCoroutine(
            typingDots.AnimateDots(
                loops
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
                userName
            )
            || Random.value > 0.18f
        )
        {
            return text;
        }

        return RandomChoice(
            userName + ", " + text,
            text + " honestly " + userName,
            text + " though " + userName
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

    public void ResetChat()
    {
        StopAllCoroutines();

        PlayerPrefs.DeleteAll();

        PlayerPrefs.Save();

        uiManager.ClearMessages();

        recentMessages.Clear();

        recentAIReplies.Clear();

        rememberedTopics.Clear();

        memorySystem.ClearMemories();

        userName = "";

        relationshipLevel = 0;

        lastTopic = "";

        previousTopic = "";

        currentMood =
            Mood.Neutral;

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

    public string GetDynamicFollowUp()
    {
        return brain.GetGeneralFollowUp();
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
    [HideInInspector]
    public bool awaitingTopicShift = false;

    [HideInInspector]
    public string lastQuestionTopic = "";

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

    public bool IsLikelyName(
    string input
)
    {
        if (
            !string.IsNullOrEmpty(
                userName
            )
        )
        {
            return false;
        }

        string trimmed =
            input
            .Trim()
            .ToLower();

        if (
            trimmed.Contains(" ")
        )
        {
            return false;
        }

        if (
            trimmed.Length < 2
            || trimmed.Length > 14
        )
        {
            return false;
        }

        string[] blockedWords =
        {
        "yes",
        "yeah",
        "yep",
        "nah",
        "no",
        "okay",
        "ok",
        "sure",
        "thanks",
        "thankyou",
        "hello",
        "hi",
        "hey",
        "fair",
        "real",
        "true",
        "valid",
        "cool",
        "nice",
        "lol",
        "lmao",
        "mood"
    };

        foreach (string word in blockedWords)
        {
            if (trimmed == word)
            {
                return false;
            }
        }

        return char.IsLetter(
            trimmed[0]
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

    public string GenerateReflectiveResponse(
        string input
    )
    {
        return typingRef.GenerateReflectiveResponse(
            input,
            this
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
            relationshipLevel > 5
            && Random.value < 0.015f;
    }
}