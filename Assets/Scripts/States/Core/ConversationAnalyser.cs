using System.Globalization;
using UnityEngine;

public class ConversationAnalyser : MonoBehaviour
{
    //varaibles, public bools, strings 
    #region
    ChatManager chat;

    //tracks most recent detected topic
    public string lastTopic = "";

    //stores last direct question asked
    public string lastQuestionAsked = "";

    //stores last detected conversational intent
    public string lastIntent = "";

    //tracks waiting states for followups
    public bool waitingForVisitAnswer = false;

    public bool userSaidGoodbye = false;


    public bool singingMode = false;

    public bool roleplayMode = false;

    public int exchangesSinceVisitQuestion = 0;
    #endregion
    //slang, accidental fake names
    #region
    //modern slang detection list
    string[] weirdInputs =
    {
        "skibidi",
        "rizz",
        "sus",
        "yeet",
        "delulu",
        "goated",
        "ate",
        "mid",
        "npc"
    };

    //prevents accidental fake names
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
    #endregion
    void Awake()
    {
        chat =
            GetComponent<ChatManager>();
    }
    #region
    //main conversation analysis 
    public void Analyse(
        string input
    )
    {
        string lower =
            input.ToLower();

        exchangesSinceVisitQuestion++;

        DetectTopics(lower);

        HandleEmotionAnalysis(lower);

        //emotional decay system
        //lets emotions naturally fade over time
        if (
            chat.emotionPersistence > 0
        )
        {
            chat.emotionPersistence--;
        }
        else
        {
            chat.lastEmotion = "";
        }

        //syncs analyser topic with chat manager
        //helps eliza style callbacks feel consistent
        chat.lastTopic =
            lastTopic;
    }

    //detects conversational subjects/topics
    void DetectTopics(
        string lower
    )
    {
        chat.previousTopic =
            chat.lastTopic;

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
                "girlfriend",
                "boyfriend",
                "dating",
                "relationship",
                "partner",
                "crush"
                )
        )
                        {
            lastTopic = "relationship";
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
    #endregion
    //emotion detection system
    #region
    void HandleEmotionAnalysis(
        string lower
    )
    {
        //detects masking behaviour
        //inspired by emotional contradiction patterns
        if (
            ContainsAny(
                lower,
                "stressed",
                "overwhelmed",
                "panic"
            )
            &&
            ContainsAny(
                lower,
                "haha",
                "lol",
                "lmao"
            )
        )
        {
            SetEmotion(
                "maskingStress",
                4
            );

            return;
        }

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

    //sets emotional state persistence
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
    #endregion
    //safe keyword detection with word boundaries
    #region
    //prevents false positives from partial words
    public bool ContainsAny(
        string input,
        params string[] words
    )
    {
        string padded =
            " "
            + input.ToLower()
            + " ";

        foreach (
            string w
            in words
        )
        {
            if (
                padded.Contains(
                    " "
                    + w.ToLower()
                    + " "
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    //light fuzzy matching system
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

    //detects low information replies
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

        if (
            lower.Split(' ').Length <= 2
            && lower.Length <= 14
        )
        {
            return true;
        }

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

    //checks if input is likely a username
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

    //cleans + formats detected names
    public string CleanName(
        string raw
    )
    {
        if (
            string.IsNullOrWhiteSpace(
                raw
            )
        )
        {
            return "Kid";
        }

        raw = raw.Trim();

        string name = raw;

        if (
            string.IsNullOrEmpty(
                name
            )
        )
        {
            return "Kid";
        }

        return CultureInfo
            .CurrentCulture
            .TextInfo
            .ToTitleCase(
                name.ToLower()
            );
    }
    #endregion
    //goodbye detection
    #region
    public bool IsGoodbye(
        string lower
    )
    {
        return
            lower.Contains("bye")
            || lower.Contains("goodbye")
            || lower.Contains("byeeeeee")
            || lower.Contains("byeeeeeeeee")
            || lower.Contains("byee")
            || lower.Contains("byeee")
            || lower.Contains("nap")
            || lower.Contains("going to nap")
            || lower.Contains("need a nap")
            || lower.Contains("byeeee")
            || lower.Contains("cya")
            || lower.Contains("see you")
            || lower.Contains("gn")
            || lower.Contains("goodnight")
            || lower.Contains("talk later")
            || lower.Contains("going to bed")
            || lower.Contains("going bed")
            || lower.Contains("im going to sleep")
            || lower.Contains("i'm going to sleep")
            || lower.Contains("going sleep")
            || lower.Contains("sleep now")
            || lower.Contains("off to bed")
            || lower.Contains("night")
            || lower.Contains("sleeping now");
    }
    #endregion
    //roleplay/fantasy detection
    #region
    public bool IsRoleplay(
        string lower
    )
    {
        return
            (
                lower.Contains(
                    "pretend"
                )
                || lower.Contains(
                    "roleplay"
                )
                || lower.Contains(
                    "imagine"
                )
            )
            &&
            (
                lower.Contains(
                    "wizard"
                )
                || lower.Contains(
                    "dragon"
                )
                || lower.Contains(
                    "batman"
                )
                || lower.Contains(
                    "superhero"
                )
                || lower.Contains(
                    "villain"
                )
            );
    }

    //music/singing request detection
    public bool IsSingingRequest(
        string lower
    )
    {
        return
            lower.Contains(
                "sing with me"
            )
            || lower.Contains(
                "lyrics"
            )
            || lower.Contains(
                "sing"
            )
            || lower.Contains(
                "song"
            )
            || lower.Contains(
                "music"
            );
    }

    //detects teasing/mockery
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
    #endregion
    //detects weird/slang inputs
    //used for eliza style "what does that mean?" moments
    #region
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
                && !clean.Contains("http")
                && !clean.Contains("@")
            )
            {
                bool normalWord =
                    clean.Contains("ing")
                    || clean.Contains("tion")
                    || clean.Contains("ment")
                    || clean.Contains("ally")
                    || clean.Contains("able")
                    || clean.Contains("friend")
                    || clean.Contains("girl")
                    || clean.Contains("boy")
                    || clean.Contains("relationship")
                    || clean.Contains("school")
                    || clean.Contains("course")
                    || clean.Contains("student");

                if (
                    !normalWord
                    && !char.IsUpper(clean[0])
                    && clean.Split(' ').Length == 1
                )
                {
                    if (
                        clean.Length >= 12
                    )
                    {
                        chat.pendingSlangWord =
                            clean;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    //returns weird/slang word
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
    #endregion
    //detects reciprocal conversation replies
    #region
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
    public bool IsResolutionResponse(
    string lower
)
    {
        return
            ContainsAny(
                lower,
                "finished",
                "done",
                "completed",
                "sorted",
                "fixed",
                "passed",
                "resolved",
                "submitted",
                "got it done",
                "all done"
            );
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
#endregion