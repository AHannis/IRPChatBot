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

    string[] blockedNameWords =
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

        DetectTopics(lower);

        HandleEmotionAnalysis(lower);
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
            ContainsAny(
                lower,
                "family",
                "mum",
                "dad",
                "cousin",
                "nan",
                "grandad",
                "brother",
                "sister"
            )
        )
        {
            lastTopic = "family";
        }

        if (
            ContainsAny(
                lower,
                "gaming",
                "game",
                "xbox",
                "playstation",
                "steam",
                "pc"
            )
        )
        {
            lastTopic = "gaming";
        }

        if (
            ContainsAny(
                lower,
                "school",
                "teacher",
                "homework",
                "gcse",
                "lesson"
            )
        )
        {
            lastTopic = "school";
        }

        if (
            ContainsAny(
                lower,
                "uni",
                "assignment",
                "coursework",
                "dissertation",
                "lecture"
            )
        )
        {
            lastTopic = "uni";
        }

        if (
            ContainsAny(
                lower,
                "work",
                "shift",
                "manager",
                "coworker",
                "boss"
            )
        )
        {
            lastTopic = "work";
        }

        if (
            ContainsAny(
                lower,
                "music",
                "song",
                "playlist"
            )
        )
        {
            lastTopic = "music";
        }

        if (
            ContainsAny(
                lower,
                "reading",
                "book",
                "novel"
            )
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
            ContainsAny(
                lower,
                "mean",
                "bully",
                "drama",
                "ignored",
                "rude",
                "toxic",
                "fake",
                "talking behind"
            )
        )
        {
            SetEmotion(
                "socialStress",
                4
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "stressed",
                "overwhelmed",
                "too much",
                "pressure",
                "panic",
                "anxious",
                "burnt out",
                "burned out"
            )
        )
        {
            SetEmotion(
                "stress",
                4
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "tired",
                "exhausted",
                "drained",
                "sleepy",
                "no sleep",
                "barely slept"
            )
        )
        {
            SetEmotion(
                "tired",
                4
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "sad",
                "crying",
                "upset",
                "hurt",
                "lonely",
                "heartbroken",
                "depressed"
            )
        )
        {
            SetEmotion(
                "sad",
                5
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "excited",
                "happy",
                "won",
                "passed",
                "good news",
                "promotion",
                "proud"
            )
        )
        {
            SetEmotion(
                "excited",
                3
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "awkward",
                "embarrassed",
                "cringe",
                "humiliating",
                "mortifying"
            )
        )
        {
            SetEmotion(
                "embarrassed",
                3
            );

            return;
        }

        if (
            ContainsAny(
                lower,
                "angry",
                "annoying",
                "frustrating",
                "mad",
                "irritating"
            )
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
        foreach (
            string w
            in words
        )
        {
            if (
                input.Contains(w)
            )
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
        foreach (
            string word
            in words
        )
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

        lower =
            lower
            .Replace("?", "")
            .Replace("!", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace(":", "")
            .Replace(";", "");

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
            || lower == "lol"
            || lower == "haha"
            || lower == "yeah haha"
            || lower == "true haha";
    }

    public bool IsLikelyName(
        string input,
        string currentUserName
    )
    {
        if (
            !string.IsNullOrEmpty(
                currentUserName
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

        foreach (
            string word
            in blockedNameWords
        )
        {
            if (
                trimmed == word
            )
            {
                return false;
            }
        }

        return char.IsLetter(
            trimmed[0]
        );
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

        if (
            string.IsNullOrEmpty(name)
        )
        {
            return "Kid";
        }

        if (
            name.Length == 1
        )
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

    public bool ContainsWeirdInput(
        string input,
        ChatManager chat
    )
    {
        foreach (
            string weird
            in weirdInputs
        )
        {
            if (
                input.Contains(
                    weird
                )
            )
            {
                return true;
            }
        }

        string[] words =
            input.Split(' ');

        foreach (
            string word
            in words
        )
        {
            string clean =
                word
                .ToLower()
                .Trim();

            if (
                clean.Length >= 9
            )
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
        foreach (
            string weird
            in weirdInputs
        )
        {
            if (
                input.Contains(
                    weird
                )
            )
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
        lower =
            lower
            .ToLower()
            .Trim();

        lower =
            lower
            .Replace("?", "")
            .Replace("!", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace(":", "")
            .Replace(";", "")
            .Replace(")", "")
            .Replace("(", "");

        return
            lower == "and you"
            || lower == "you"
            || lower == "hbu"
            || lower == "how about you"
            || lower == "what about you"
            || lower == "how are you"
            || lower == "howve you been"
            || lower == "how have you been"
            || lower == "and you haha"
            || lower == "and you lol";
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

        foreach (
            string trigger
            in triggers
        )
        {
            if (
                lower.Contains(
                    trigger
                )
            )
            {
                int i =
                    lower.IndexOf(
                        trigger
                    );

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