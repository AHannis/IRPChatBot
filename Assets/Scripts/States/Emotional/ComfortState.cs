using UnityEngine;

public class ComfortState : ChatState
{
    int comfortExchanges = 0;

    //tracks repeated reassurance styles prevents emotional responses feeling too repetitive
    int reassuranceType = -1;

    //filtered topics that sound unnatural when reflected back to the user
    string[] invalidTopics =
    {
        "sense",
        "okay",
        "fine",
        "true",
        "life",
        "yeah",
        "stuff",
        "things",
        "everything"
    };

    public ComfortState(
        ChatManager manager
    ) : base(manager)
    {
    }
    #region Enter Comfort State
    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Concerned;

        //stores active emotional state
        //used for emotional continuity across replies
        chat.lastEmotion =
            "comfort";

        //comfort conversations persist longer emotionally
        chat.emotionPersistence =
            5;

        //higher relationship levels unlock more personal concern
        if (
            chat.relationshipLevel >= 20
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "come on what's happened?",
                    "you sound genuinely drained lately",
                    "talk to me properly for a second",
                    "life been hitting hard lately huh?",
                    "you've seemed off lately",
                    "you alright? you've not sounded yourself",
                    "feels like you've been carrying too much quietly"
                )
            );

            return;
        }

        //lower relationship versions stay softer
        chat.SendAIImmediate(
            chat.RandomChoice(
                "come on what's happened?",
                "you sound genuinely down",
                "talk to me properly what's going on?",
                "you alright?",
                "something's clearly bothering you",
                "what's been weighing on you then?"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        comfortExchanges++;

        chat.lastEmotion =
            "comfort";

        string lower =
            input.ToLower();

        string topic =
            chat.typingRef.ExtractTopic(
                input
            );

        bool validTopic =
            IsValidTopic(topic);

        //supportive conversations slowly increase relationship level
        chat.relationshipLevel += 2;

        if (
            Random.value < 0.18f
            && validTopic
            && input.Split(' ').Length >= 6
            && !chat.IsShortReply(lower)
        )
        {
            return chat.RandomChoice(
                "feels like "
                + topic
                + " has been stuck in your head constantly",

                "sounds like "
                + topic
                + " has been draining you for a while",

                "when "
                + topic
                + " keeps building up it wears people down fast",

                "you've been carrying "
                + topic
                + " around mentally for ages huh?",

                "doesn't sound like your brain's had much break from "
                + topic
            );
        }

        //short exhaustion replies
        if (
            input.Split(' ').Length <= 3
            && (
                lower.Contains(
                    "exhausted"
                )
                || lower.Contains(
                    "drained"
                )
                || lower.Contains(
                    "tired"
                )
                || lower.Contains(
                    "burnt out"
                )
            )
        )
        {
            return chat.RandomChoice(
                "yeah... you sound properly worn down",

                "that kind of exhaustion creeps up quietly",

                "feels like you've been carrying too much lately",

                "sounds like your brain never really switched off",

                "everything starts feeling heavier when you're that drained",

                "doesn't sound like life's given you chance to breathe recently"
            );
        }
        #endregion
        #region Stress & Anxiety Handling
        //stress / anxiety handling
        if (
            lower.Contains("stress")
            || lower.Contains(
                "overwhelmed"
            )
            || lower.Contains(
                "anxious"
            )
            || lower.Contains(
                "panic"
            )
            || lower.Contains(
                "pressure"
            )
        )
        {
            if (
                validTopic
                && input.Split(' ').Length >= 5
            )
            {
                return chat.RandomChoice(
                    "sounds like "
                    + topic
                    + " has been weighing on you a lot recently",

                    "yeah "
                    + topic
                    + " sounds exhausting to deal with constantly",

                    "feels like your brain keeps circling back to "
                    + topic,

                    "when "
                    + topic
                    + " keeps piling up it drains people fast",

                    "sounds like you've never really had chance to switch off from "
                    + topic
                );
            }

            return GetComfortResponse();
        }

        //loneliness handling
        if (
            lower.Contains("alone")
            || lower.Contains(
                "lonely"
            )
            || lower.Contains(
                "nobody"
            )
        )
        {
            return chat.RandomChoice(
                "you're not as alone as your brain makes it feel",

                "people care about you more than you realise",

                "don't disappear into your own head too much",

                "isolation makes everything feel heavier",

                "you don't have to deal with everything silently",

                "your brain lies to you a bit when you're overwhelmed"
            )
            + " "
            + chat.Emoji("smile");
        }

        //hurt / emotional pain
        if (
            lower.Contains("cry")
            || lower.Contains(
                "hurts"
            )
            || lower.Contains(
                "pain"
            )
            || lower.Contains(
                "heartbroken"
            )
        )
        {
            return chat.RandomChoice(
                "yeah some things hit unbelievably hard",

                "you're allowed to feel awful sometimes",

                "just don't bottle everything up alright?",

                "some stuff takes longer to heal from than people admit",

                "people act like emotions are simple when they really aren't",

                "certain things stay with people for a long time"
            );
        }

        //exhaustion handling
        if (
            lower.Contains("tired")
            || lower.Contains(
                "drained"
            )
            || lower.Contains(
                "exhausted"
            )
        )
        {
            if (
                validTopic
                && input.Split(' ').Length >= 5
            )
            {
                return chat.RandomChoice(
                    "feels like "
                    + topic
                    + " has completely drained you",

                    "carrying "
                    + topic
                    + " around constantly would exhaust anyone",

                    "sounds like "
                    + topic
                    + " has been sitting in your head for ages"
                );
            }

            return chat.RandomChoice(
                "yeah you sound genuinely worn out recently",

                "everything feels heavier when you're emotionally exhausted",

                "you've been running on fumes mentally for a while now",

                "sounds like your brain hasn't rested properly",

                "that kind of exhaustion builds up quietly over time"
            );
        }
        #endregion
        #region Daily Stress
        //daily life stress
        if (
            lower.Contains("school")
            || lower.Contains("uni")
            || lower.Contains("work")
        )
        {
            return chat.RandomChoice(
                "daily life piles up ridiculously fast",

                "people underestimate how exhausting normal life gets",

                "sounds like you've had way too much on lately",

                "sometimes everything hits at once",

                "your brain never really gets chance to switch off",

                "people can only juggle so much before it catches up with them"
            );
        }

        //gratitude exits comfort state naturally
        if (
            lower.Contains("thank")
            || lower.Contains(
                "thanks"
            )
        )
        {
            chat.lastEmotion = "";

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "always",

                "you're alright kid",

                "don't mention it",

                "i've got you",

                "just don't vanish afterwards",

                "someone has to keep an eye on you"
            )
            + " "
            + chat.Emoji("smile");
        }

        //humour naturally fades emotional tension
        if (
            lower.Contains("lol")
            || lower.Contains(
                "lmao"
            )
            || lower.Contains(
                "haha"
            )
        )
        {
            chat.lastEmotion = "";

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "there's the chaos again",

                "you're coping through humour i see",

                "even now you're still joking",

                "you really laugh through everything huh",

                "somehow you still manage to joke",

                "humour honestly keeps people functioning sometimes"
            );
        }

        //soft reassurance if user insists they're okay
        if (
            lower.Contains("fine")
            || lower.Contains(
                "i'm okay"
            )
            || lower.Contains(
                "im okay"
            )
            || lower.Contains(
                "i'll be okay"
            )
        )
        {
            return chat.RandomChoice(
                "maybe. but you still sound worn out",

                "alright. just don't bottle everything up",

                "you say that a lot when you're struggling",

                "just make sure you're actually looking after yourself",

                "okay. i'm just checking in on you"
            );
        }

        //small occasional reassurance injections
        if (
            Random.value < 0.10f
        )
        {
            return chat.RandomChoice(
                "seriously though be kinder to yourself",

                "your brain deserves rest too",

                "you don't have to solve everything immediately",

                "sometimes surviving the week is enough",

                "people forget how exhausting life gets mentally"
            );
        }
        #endregion
        #region Prevents Comfort State Lasting Forever

        //prevents comfort state lasting forever
        if (
            comfortExchanges >= 4
        )
        {
            chat.lastEmotion = "";

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "just take care of yourself alright?",

                "seriously though don't carry everything alone",

                "you deserve proper rest",

                "be a little kinder to yourself alright?",

                "look after yourself properly yeah?",

                "promise me you're getting some actual rest",

                "anyway... enough of me sounding wise for five minutes"
            );
        }

        //default comfort responses
        return chat.RandomChoice(
            "sounds like you've had a lot sitting on your mind lately",

            "sometimes people keep everything in until they hit their limit",

            "life gets heavy when things keep stacking up",

            "you sound like you've been running on empty for a while",

            "you don't always have to carry everything quietly yknow",

            "i'm listening if you wanna talk properly about it"
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
    #endregion
    #region Rotates Reaassurance Styles
    //rotates reassurance styles slightlyto reduce repetitive emotional phrasing
    string GetComfortResponse()
    {
        reassuranceType++;

        if (
            reassuranceType > 2
        )
        {
            reassuranceType = 0;
        }

        switch (
            reassuranceType
        )
        {
            case 0:

                return chat.RandomChoice(
                    "you've been carrying too much lately",
                    "sounds like everything's piled up at once",
                    "your brain sounds overloaded recently"
                );

            case 1:

                return chat.RandomChoice(
                    "one thing at a time alright?",
                    "don't try to solve everything at once",
                    "you seriously need a proper break"
                );

            default:

                return chat.RandomChoice(
                    "feels like you've been stuck in survival mode lately",
                    "sounds like your brain never fully switches off",
                    "people can only run on empty for so long"
                );
        }
    }
}
#endregion