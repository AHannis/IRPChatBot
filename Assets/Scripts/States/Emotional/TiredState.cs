using UnityEngine;

public class TiredState : ChatState
{
    int exchanges = 0;

    public TiredState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        //sets emotional mood state
        chat.currentMood =
            ChatManager.Mood.Concerned;

        //tracks emotional persistence
        chat.emotionLoops = 0;

        //immediate concern response
        //helps emotional states feel reactive 
        chat.SendAIImmediate(
            chat.RandomChoice(
                "long week?",

                "you've been overdoing it haven't you",

                "you seriously need rest "
                + chat.Emoji("awkward"),

                "you alright? you sound exhausted honestly",

                "your sleep schedule sounds medically concerning"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        exchanges++;

        //tracks emotional conversation length
        chat.emotionLoops++;

        string lower =
            input.ToLower();

        //small chance of reflective response
        //inspired by eliza style reflective mirroring
        if (
            Random.value < 0.14f
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
            )
            {
                return reflective;
            }
        }

        //user accepting advice
        if (
            chat.ContainsAny(
                lower,
                "i will",
                "okay",
                "yeah",
                "alright",
                "fine",
                "promise"
            )
        )
        {
            //emotions fade naturally instead of instantly
            chat.emotionPersistence--;

            if (
                chat.emotionPersistence <= 0
            )
            {
                chat.currentMood =
                    ChatManager.Mood.Neutral;

                chat.ChangeState(
                    new CasualState(chat)
                );
            }

            //higher relationship = more teasing familiarity
            if (
                chat.relationshipLevel > 10
            )
            {
                return chat.RandomChoice(
                    "good. i'm legally forcing you to sleep at this point",

                    "look at you finally listening to advice "
                    + chat.Emoji("thumbsup"),

                    "good because your body was starting a rebellion",

                    "i refuse to let you become nocturnal",

                    "finally a sensible life decision"
                );
            }

            return chat.RandomChoice(
                "good "
                + chat.Emoji("smile")
                + " now when are you actually getting a proper break?",

                "look at you listening to advice for once "
                + chat.Emoji("thumbsup"),

                "good. your body was filing complaints",

                "right good. anyway what've you been up to besides being exhausted?",

                "see? growth "
                + chat.Emoji("smile")
            );
        }

        //busy/stress followup branch
        if (
            chat.ContainsAny(
                lower,
                "busy",
                "work",
                "stressed",
                "deadline",
                "assignment",
                "overwhelmed"
            )
        )
        {
            //longer emotional persistence
            if (
                chat.emotionLoops
                >= chat.emotionPersistence
            )
            {
                chat.currentMood =
                    ChatManager.Mood.Neutral;

                chat.ChangeState(
                    new CasualState(chat)
                );
            }

            return chat.RandomChoice(
                "yeah sounds like you've had a lot on",

                "life catching up with you?",

                "you need a proper break",

                "seriously though don't burn yourself out "
                + chat.Emoji("thumbsup"),

                "your brain sounds completely overloaded honestly",

                "that's not sustainable forever you know"
            );
        }

        //sleep/rest acknowledgement
        if (
            chat.ContainsAny(
                lower,
                "sleep",
                "rest",
                "nap",
                "bed"
            )
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "good. don't stay up all night again",

                "finally a sensible decision "
                + chat.Emoji("thumbsup"),

                "your body will thank you honestly",

                "right go recharge your batteries",

                "see this is what responsible people do"
            );
        }

        //eventual state exit
        if (
            exchanges >= 4
            || chat.emotionLoops
            >= chat.emotionPersistence
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            //smooth conversation transition
            return chat.RandomChoice(
                "anyway enough of my lecture "
                + chat.Emoji("laugh"),

                "right i'm done acting like your sleep therapist",

                "ANYWAY before i become your life coach",

                "moving on before i start prescribing vitamins",

                "right enough emotional support from me honestly"
            )
            + ". "
            + chat.GetGeneralFollowUp();
        }

        //default tired state responses
        return chat.RandomChoice(
            "seriously though take care of yourself",

            "burning yourself out helps nobody",

            "you sound exhausted honestly",

            "drink water and sleep "
            + chat.Emoji("thumbsup"),

            "your sleep schedule is fighting for survival",

            "you need at least twelve hours unconscious honestly",

            "your brain sounds like it's running on emergency power"
        );
    }
}