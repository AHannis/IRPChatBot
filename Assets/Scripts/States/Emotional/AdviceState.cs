using UnityEngine;

public class AdviceState : ChatState
{
    int adviceDepth = 0;

    int reassuranceStyle = 0;

    string[] invalidTopics =
    {
        "life",
        "things",
        "stuff",
        "okay",
        "fine",
        "sense",
        "everything"
    };

    public AdviceState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Concerned;

        //higher relationship levels unlock warmer openings
        if (
            chat.relationshipLevel >= 18
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "come on what's actually going on?",
                    "you've clearly had something sitting on your mind",
                    "talk to me properly for a second",
                    "feels like you've been stuck in your own head lately",
                    "you don't sound fully alright"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "alright talk to me properly then",
                "come on what's bothering you?",
                "you've clearly got something on your mind",
                "something's been getting to you hasn't it?"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        adviceDepth++;

        string lower =
            input.ToLower();

        string topic =
            chat.typingRef.ExtractTopic(
                input
            );

        bool validTopic =
            IsValidTopic(topic);

        //supportive conversations slowly build trust
        chat.relationshipLevel += 1;

        //reflective replies help the uncle feel attentive
        if (
            Random.value < 0.16f
            && validTopic
            && input.Split(' ').Length >= 6
        )
        {
            return chat.RandomChoice(
                "sounds like "
                + topic
                + " has been sitting in your head constantly",

                "feels like "
                + topic
                + " has been draining your energy for a while",

                "your brain keeps circling back to "
                + topic
                + " doesn't it?",

                "when "
                + topic
                + " keeps piling up people start feeling stuck fast"
            );
        }

        //stress related advice
        if (
            chat.ContainsAny(
                lower,
                "stress",
                "stressed",
                "anxious",
                "overwhelmed",
                "panic",
                "pressure"
            )
        )
        {
            reassuranceStyle++;

            if (
                reassuranceStyle > 2
            )
            {
                reassuranceStyle = 0;
            }

            if (
                adviceDepth >= 3
            )
            {
                chat.ChangeState(
                    new CasualState(chat)
                );

                return chat.RandomChoice(
                    "just take things one step at a time alright?",
                    "your brain needs rest too yknow",
                    "don't carry everything alone all the time",
                    "anyway before i accidentally become wise"
                );
            }

            switch (
                reassuranceStyle
            )
            {
                case 0:

                    return chat.RandomChoice(
                        "sounds like you've had too much piling up at once",
                        "your brain sounds overloaded lately",
                        "people can only run on stress for so long"
                    );

                case 1:

                    return chat.RandomChoice(
                        "i dunno... feels like you've been stuck in survival mode lately",
                        "sometimes people don't realise how burnt out they are until they stop for a second",
                        "you've probably been carrying more mentally than you realise"
                    );

                default:

                    return chat.RandomChoice(
                        "your nervous system sounds exhausted",
                        "feels like your brain never fully switches off",
                        "everything starts feeling heavier when stress keeps stacking up"
                    );
            }
        }

        //relationship / friendship advice
        if (
            chat.ContainsAny(
                lower,
                "relationship",
                "friend",
                "partner",
                "boyfriend",
                "girlfriend"
            )
        )
        {
            if (
                validTopic
            )
            {
                return chat.RandomChoice(
                    "people get complicated once "
                    + topic
                    + " gets involved",

                    "half the time people just don't communicate properly about "
                    + topic,

                    "sometimes people make situations worse by avoiding conversations about "
                    + topic
                );
            }

            return chat.RandomChoice(
                "people are complicated",
                "communication fixes more problems than people realise",
                "sometimes people just stop saying what they actually mean",
                "relationships get messy fast when people stop talking properly"
            );
        }

        //burnout / exhaustion
        if (
            chat.ContainsAny(
                lower,
                "tired",
                "drained",
                "burnt out",
                "burned out",
                "exhausted"
            )
        )
        {
            return chat.RandomChoice(
                "you sound like you've been running on fumes lately",
                "that kind of exhaustion builds up quietly",
                "your brain genuinely sounds tired",
                "doesn't sound like you've had chance to properly rest"
            );
        }

        //self doubt
        if (
            chat.ContainsAny(
                lower,
                "worthless",
                "failure",
                "useless",
                "bad at everything"
            )
        )
        {
            return chat.RandomChoice(
                "your brain's being way harsher on you than reality probably is",
                "people judge themselves way more brutally than everyone else does",
                "everyone falls apart sometimes",
                "having a rough time doesn't suddenly make you worthless"
            );
        }

        //thank you exits naturally
        if (
            lower.Contains("thanks")
            || lower.Contains("thank you")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "always",
                "don't mention it",
                "someone has to keep you functioning",
                "you're alright",
                "just don't vanish for six months afterwards"
            )
            + " "
            + chat.Emoji("smile");
        }

        //humour naturally softens emotional conversations
        if (
            lower.Contains("lol")
            || lower.Contains("lmao")
            || lower.Contains("haha")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "there's the chaos again",
                "you really joke through everything huh",
                "somehow you're still making jokes",
                "humour is carrying you mentally at this point"
            );
        }

        //conversation naturally fades out
        if (
            adviceDepth >= 4
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway enough emotional wisdom from me",
                "right before i accidentally become motivational",
                "you'll figure things out alright",
                "just don't be too hard on yourself yeah?"
            );
        }

        //default grounded advice replies
        return chat.RandomChoice(
            "life gets messy sometimes",
            "people aren't built to deal with everything perfectly",
            "you've handled difficult stuff before",
            "sometimes your brain just needs time to settle down a bit",
            "i think you've probably been harder on yourself than you should be",
            "people can only carry so much mentally before it catches up with them"
        );
    }

    bool IsValidTopic(
        string topic
    )
    {
        if (
            string.IsNullOrEmpty(
                topic
            )
        )
        {
            return false;
        }

        if (
            topic.Length <= 4
        )
        {
            return false;
        }

        foreach (
            string invalid
            in invalidTopics
        )
        {
            if (
                topic == invalid
            )
            {
                return false;
            }
        }

        return true;
    }
}