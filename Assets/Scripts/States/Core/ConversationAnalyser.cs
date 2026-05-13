using UnityEngine;

public class ConversationAnalyser : MonoBehaviour
{
    ChatManager chat;

    public string lastTopic = "";

    public string lastQuestionAsked = "";

    public string lastIntent = "";

    public bool waitingForVisitAnswer = false;

    public bool userSaidGoodbye = false;

    public bool singingMode = false;

    public bool roleplayMode = false;

    public int exchangesSinceVisitQuestion = 0;

    void Awake()
    {
        chat =
            GetComponent<ChatManager>();
    }

    public void Analyse(
        string input
    )
    {
        string lower =
            input.ToLower();

        exchangesSinceVisitQuestion++;

        HandleEmotionAnalysis(lower);

        DetectTopics(lower);
    }

    void DetectTopics(
        string lower
    )
    {
        if (
            lower.Contains("visit")
        )
        {
            lastTopic = "visit";
        }

        if (
            lower.Contains("family")
            || lower.Contains("mum")
            || lower.Contains("dad")
            || lower.Contains("cousin")
            || lower.Contains("nan")
            || lower.Contains("grandad")
            || lower.Contains("brother")
            || lower.Contains("sister")
        )
        {
            lastTopic = "family";
        }

        if (
            lower.Contains("gaming")
            || lower.Contains("game")
            || lower.Contains("xbox")
            || lower.Contains("playstation")
            || lower.Contains("steam")
            || lower.Contains("pc")
        )
        {
            lastTopic = "gaming";
        }

        if (
            lower.Contains("school")
            || lower.Contains("teacher")
            || lower.Contains("homework")
            || lower.Contains("gcse")
            || lower.Contains("lesson")
        )
        {
            lastTopic = "school";
        }

        if (
            lower.Contains("uni")
            || lower.Contains("assignment")
            || lower.Contains("coursework")
            || lower.Contains("dissertation")
            || lower.Contains("lecture")
        )
        {
            lastTopic = "uni";
        }

        if (
            lower.Contains("work")
            || lower.Contains("shift")
            || lower.Contains("manager")
            || lower.Contains("coworker")
            || lower.Contains("boss")
        )
        {
            lastTopic = "work";
        }

        if (
            lower.Contains("music")
            || lower.Contains("song")
            || lower.Contains("playlist")
        )
        {
            lastTopic = "music";
        }

        if (
            lower.Contains("reading")
            || lower.Contains("book")
            || lower.Contains("novel")
        )
        {
            lastTopic = "reading";
        }
    }

    void HandleEmotionAnalysis(
        string lower
    )
    {
        if (
            lower.Contains("mean")
            || lower.Contains("bully")
            || lower.Contains("drama")
            || lower.Contains("ignored")
            || lower.Contains("rude")
            || lower.Contains("toxic")
            || lower.Contains("fake")
            || lower.Contains("talking behind")
        )
        {
            SetEmotion(
                "socialStress",
                4
            );

            return;
        }

        if (
            lower.Contains("stressed")
            || lower.Contains("overwhelmed")
            || lower.Contains("too much")
            || lower.Contains("pressure")
            || lower.Contains("panic")
            || lower.Contains("anxious")
            || lower.Contains("burnt out")
            || lower.Contains("burned out")
        )
        {
            SetEmotion(
                "stress",
                4
            );

            return;
        }

        if (
            lower.Contains("tired")
            || lower.Contains("exhausted")
            || lower.Contains("drained")
            || lower.Contains("sleepy")
            || lower.Contains("no sleep")
            || lower.Contains("barely slept")
        )
        {
            SetEmotion(
                "tired",
                4
            );

            return;
        }

        if (
            lower.Contains("sad")
            || lower.Contains("crying")
            || lower.Contains("upset")
            || lower.Contains("hurt")
            || lower.Contains("lonely")
            || lower.Contains("heartbroken")
            || lower.Contains("depressed")
        )
        {
            SetEmotion(
                "sad",
                5
            );

            return;
        }

        if (
            lower.Contains("excited")
            || lower.Contains("happy")
            || lower.Contains("won")
            || lower.Contains("passed")
            || lower.Contains("good news")
            || lower.Contains("promotion")
            || lower.Contains("proud")
        )
        {
            SetEmotion(
                "excited",
                3
            );

            return;
        }

        if (
            lower.Contains("awkward")
            || lower.Contains("embarrassed")
            || lower.Contains("cringe")
            || lower.Contains("humiliating")
            || lower.Contains("mortifying")
        )
        {
            SetEmotion(
                "embarrassed",
                3
            );

            return;
        }

        if (
            lower.Contains("angry")
            || lower.Contains("annoying")
            || lower.Contains("frustrating")
            || lower.Contains("mad")
            || lower.Contains("irritating")
        )
        {
            SetEmotion(
                "frustrated",
                4
            );
        }
    }

    void SetEmotion(
        string emotion,
        int persistence
    )
    {
        chat.lastEmotion =
            emotion;

        chat.emotionPersistence =
            persistence;

        chat.emotionLoops = 0;
    }

    public bool ContainsAny(
        string input,
        params string[] words
    )
    {
        foreach (string w in words)
        {
            if (input.Contains(w))
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsFuzzy(
        string input,
        params string[] words
    )
    {
        foreach (string word in words)
        {
            if (
                input.Contains(word)
            )
            {
                return true;
            }

            if (
                Mathf.Abs(
                    input.Length
                    - word.Length
                ) <= 2
            )
            {
                int matches = 0;

                for (
                    int i = 0;
                    i < Mathf.Min(
                        input.Length,
                        word.Length
                    );
                    i++
                )
                {
                    if (
                        input[i]
                        == word[i]
                    )
                    {
                        matches++;
                    }
                }

                if (
                    matches >=
                    word.Length - 2
                )
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsShortReply(
        string input
    )
    {
        string lower =
            input.ToLower().Trim();

        return
            lower == "yeah"
            || lower == "yea"
            || lower == "yh"
            || lower == "yep"
            || lower == "nah"
            || lower == "no"
            || lower == "ok"
            || lower == "okay"
            || lower == "sure"
            || lower == "cool"
            || lower == "fair"
            || lower == "nice"
            || lower == "damn"
            || lower == "true"
            || lower == "real"
            || lower == "lmao"
            || lower == "lol";
    }

    public bool IsLikelyName(
        string input
    )
    {
        string t =
            input.Trim().ToLower();

        if (t.Length < 3)
        {
            return false;
        }

        if (t.Length > 18)
        {
            return false;
        }

        if (
            t.Contains("!")
            || t.Contains("?")
            || t.Contains(".")
        )
        {
            return false;
        }

        return true;
    }

    public string CleanName(
        string raw
    )
    {
        if (
            string.IsNullOrWhiteSpace(raw)
        )
        {
            return "Kid";
        }

        raw = raw.Trim();

        string name = raw;

        if (string.IsNullOrEmpty(name))
        {
            return "Kid";
        }

        if (name.Length == 1)
        {
            return name.ToUpper();
        }

        return
            char.ToUpper(name[0])
            + name.Substring(1)
            .ToLower();
    }

    public bool IsGoodbye(
        string lower
    )
    {
        return
            lower.Contains("bye")
            || lower.Contains("goodbye")
            || lower.Contains("cya")
            || lower.Contains("see you")
            || lower.Contains("gn")
            || lower.Contains("goodnight")
            || lower.Contains("talk later");
    }

    public bool IsRoleplay(
        string lower
    )
    {
        return
            lower.Contains("power ranger")
            || lower.Contains("super power")
            || lower.Contains("saving the world")
            || lower.Contains("wizard")
            || lower.Contains("dragon")
            || lower.Contains("batman")
            || lower.Contains("superhero")
            || lower.Contains("villain");
    }

    public bool IsSingingRequest(
        string lower
    )
    {
        return
            lower.Contains("sing with me")
            || lower.Contains("lyrics")
            || lower.Contains("sing")
            || lower.Contains("song")
            || lower.Contains("music");
    }
    public bool IsTeasing(
    string lower
)
    {
        return
            lower.Contains("broken")
            || lower.Contains("dementia")
            || lower.Contains("cruel")
            || lower.Contains("rude")
            || lower.Contains("npc")
            || lower.Contains("robot")
            || lower.Contains("old")
            || lower.Contains("bully");
    }
    string[] weirdInputs =
{
    "skibidi",
    "rizz",
    "sus",
    "yeet",
    "gyatt",
    "sigma",
    "fanum",
    "delulu",
    "goated",
    "ate",
    "mid",
    "npc"
};

    public bool ContainsWeirdInput(
        string input,
        ChatManager chat
    )
    {
        foreach (string weird in weirdInputs)
        {
            if (input.Contains(weird))
            {
                return true;
            }
        }

        string[] words =
            input.Split(' ');

        foreach (string word in words)
        {
            string clean =
                word
                .ToLower()
                .Trim();

            if (clean.Length >= 9)
            {
                bool normalWord =
                    clean.Contains("ing")
                    || clean.Contains("tion")
                    || clean.Contains("ment")
                    || clean.Contains("ally")
                    || clean.Contains("able");

                if (
                    !normalWord
                    && Random.value < 0.08f
                )
                {
                    chat.pendingSlangWord =
                        clean;

                    return true;
                }
            }
        }

        return false;
    }

    public string GetDetectedWeirdWord(
        string input
    )
    {
        foreach (string weird in weirdInputs)
        {
            if (input.Contains(weird))
            {
                return weird;
            }
        }

        return "that";
    }
    public bool IsReciprocalResponse(
    string lower
)
    {
        lower = lower.Trim();

        return
            lower == "and you"
            || lower == "and you?"
            || lower == "you?"
            || lower == "hbu"
            || lower == "how about you"
            || lower == "what about you"
            || lower == "how are you"
            || lower == "how've you been"
            || lower == "how have you been";
    }
    public string ExtractNameFlexible(
    string input
)
    {
        string lower =
            input.ToLower();

        string[] triggers =
        {
        "call me",
        "save me as",
        "save it as",
        "change it to",
        "change my name to",
        "my name is",
        "im",
        "i'm"
    };

        foreach (string trigger in triggers)
        {
            if (lower.Contains(trigger))
            {
                int i =
                    lower.IndexOf(trigger);

                string result =
                    input.Substring(
                        i + trigger.Length
                    ).Trim();

                return result;
            }
        }

        return input.Trim();
    }
}