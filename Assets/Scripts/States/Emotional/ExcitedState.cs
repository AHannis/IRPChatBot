using UnityEngine;

public class ExcitedState : ChatState
{
    //tracks how long excited conversation lasts
    int exchanges = 0;

    public ExcitedState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        //sets playful mood while user is excited
        chat.currentMood =
            ChatManager.Mood.Playful;

        //emotional persistence system helps stop moods instantly vanishing
        chat.emotionPersistence =
            3;

        chat.emotionLoops = 0;
    }

    public override string HandleInput(
        string input
    )
    {
        exchanges++;

        //tracks emotional loop count
        chat.emotionLoops++;

        string lower =
            input.ToLower();

        //eliza style reflective mirroring
        //occasionally reflects part of user input back naturally
        if (
            Random.value < 0.12f
        )
        {
            string reflective =
                chat.GenerateReflectiveResponse(
                    input
                );

            if (
                !string.IsNullOrEmpty(
                    reflective
                )
                && !chat.IsShortReply(
                    lower
                )
            )
            {
                return reflective;
            }
        }

        //positive achievement keywords
        if (
            chat.ContainsAny(
                lower,
                "passed",
                "won",
                "promotion",
                "finally",
                "accepted",
                "completed",
                "finished",
                "got it",
                "succeeded"
            )
            && Random.value < 0.35f
        )
        {
            return chat.RandomChoice(
                "okay that's actually massive honestly",
                "look at you winning at life for once",
                "that's genuinely good news honestly",
                "i'll admit that's impressive",
                "see? occasionally things work out"
            );
        }

        //memory callbacks make conversations feel continuous
        if (
            chat.relationshipLevel > 10
            && Random.value < 0.06f
        )
        {
            return chat.RandomChoice(
                "honestly compared to some of your older chaos this is progress",
                "see this is way better than that disaster you told me about before",
                "i'm genuinely shocked things went smoothly for you",
                "character development honestly"
            );
        }

        //detects agreement responses
        if (
            chat.ContainsAny(
                lower,
                "yeah",
                "yep",
                "yes",
                "i did",
                "finally did",
                "managed to",
                "ended up doing it"
            )
        )
        {
            //prevents immediate hard exits
            if (
                exchanges >= 2
            )
            {
                chat.currentMood =
                    ChatManager.Mood.Neutral;

                chat.ChangeState(
                    new CasualState(chat)
                );

                //higher relationship unlocks more teasing familiarity
                if (
                    chat.relationshipLevel > 12
                )
                {
                    return chat.RandomChoice(
                        "look at you actually succeeding for once "
                        + chat.Emoji("thumbsup")
                        + " anyway what chaos have you caused lately?",

                        "i'll pretend i doubted you less than i actually did "
                        + chat.Emoji("laugh")
                        + " what else is new?",

                        "honestly proud of you idiot "
                        + chat.Emoji("awkward")
                    );
                }

                return chat.RandomChoice(
                    "look at you actually taking my advice for once "
                    + chat.Emoji("thumbsup")
                    + " how's everything else been?",

                    "well someone's thriving honestly "
                    + chat.Emoji("smile")
                    + " what've you been up to otherwise?",

                    "see? i clearly know everything honestly "
                    + chat.Emoji("laugh")
                    + " anyway what's new?",

                    "i'll pretend i always believed in you honestly "
                    + chat.Emoji("thumbsup")
                    + " what else has been happening?"
                );
            }
        }

        //handles gratitude responses
        if (
            chat.ContainsAny(
                lower,
                "thanks",
                "thank you",
                "appreciate it"
            )
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "don't get emotional on me now honestly "
                + chat.Emoji("awkward"),

                "yeah yeah i'm wise beyond my years "
                + chat.Emoji("laugh"),

                "i expect full credit for this success story by the way",

                "see? occasional good advice from me "
                + chat.Emoji("thumbsup"),

                "rare uncle wisdom moment honestly"
            );
        }

        //downshifts emotional energy naturally
        if (
            chat.emotionLoops
            >= chat.emotionPersistence
            || exchanges >= 3
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "okay enough success and positivity honestly",

                "this is getting suspiciously wholesome honestly",

                "right before one of us gets emotional let's move on",

                "ANYWAY before i start acting supportive",

                "right enough motivational speeches from me",

                "so what else has been happening lately?"
            );
        }

        //general excited responses
        return chat.RandomChoice(
            "okay that's actually huge honestly "
            + chat.Emoji("thumbsup"),

            "look at you being successful and everything",

            "well someone's thriving honestly",

            "about time honestly "
            + chat.Emoji("laugh"),

            "i'll admit that's actually decent",

            "rare positive life update detected",

            "honestly that's a massive win",

            "see? the universe isn't bullying you today"
        );
    }
}