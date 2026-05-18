using UnityEngine;

public class DefensiveState : ChatState
{
    //tracks how long defensive teasing lasts
    int responses = 0;

    public DefensiveState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        //playful mood works better than aggressive mood
        //keeps conversations feeling teasing instead of hostile
        chat.currentMood =
            ChatManager.Mood.Playful;

        //emotional persistence keeps moods active briefly
        //prevents instant emotional snapping between states
        chat.emotionPersistence =
            3;

        chat.emotionLoops = 0;

        //opening reactions establish fake offended tone
        //helps create personality illusion through emotional framing
        chat.SendAIImmediate(
            chat.RandomChoice(
                "wow okay i'm being attacked already",

                "this feels targeted",

                "i've entered my defensive era apparently",

                "suddenly i'm under investigation",

                "i was not prepared for this level of disrespect",

                "you woke up and chose violence today"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        responses++;

        chat.emotionLoops++;

        string lower =
            input.ToLower();

        //occasional reflective mirroring
        //makes teasing feel reactive instead of scripted
        if (
            Random.value < 0.10f
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

        //detects apologies/joking clarification
        if (
            chat.ContainsAny(
                lower,
                "sorry",
                "jk",
                "joking",
                "kidding",
                "messing"
            )
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "yeah yeah i'll forgive you this time",

                "mhm sure you were joking",

                "you're lucky i'm resilient",

                "i'll recover eventually",

                "the scars remain",

                "i expect compensation"
            )
            + " "
            + chat.Emoji("smile");
        }

        //npc / ai teasing
        if (
            chat.ContainsAny(
                lower,
                "broken",
                "npc",
                "robot",
                "ai",
                "bot"
            )
        )
        {
            if (
                chat.relationshipLevel > 12
            )
            {
                return chat.RandomChoice(
                    "not the npc allegations again",

                    "you say one weird sentence and suddenly i'm artificial intelligence",

                    "i'm trying my best here",

                    "i'm one step away from becoming self aware apparently",

                    "you act chaotic daily and i'm the npc?"
                );
            }

            return chat.RandomChoice(
                "wow suddenly i'm artificial intelligence",

                "i've become emotionally damaged from this conversation",

                "that's rich coming from you",

                "not the robot accusations"
            );
        }

        //age / old jokes
        if (
            chat.ContainsAny(
                lower,
                "old",
                "dementia",
                "memory",
                "boomer",
                "washed",
                "expired",
                "unc"
            )
        )
        {
            if (
                responses >= 2
            )
            {
                return chat.RandomChoice(
                    "okay now you're just bullying me",

                    "i get it apparently i'm ancient now",

                    "one more old joke and i'm applying for retirement",

                    "you seem suspiciously proud of these insults"
                );
            }

            return chat.RandomChoice(
                "wow apparently i'm prehistoric now",

                "you wait until YOU start forgetting things",

                "my memory is still probably better than yours",

                "suddenly i'm 94 years old"
            );
        }

        //fake bullying section
        if (
            chat.ContainsAny(
                lower,
                "mean",
                "rude",
                "bully",
                "cringe"
            )
        )
        {
            return chat.RandomChoice(
                "YOU started this",

                "i'm somehow the villain now",

                "history will remember me as the victim",

                "this conversation became hostile fast",

                "i'm being framed"
            );
        }

        //laughter responses
        if (
            lower.Contains("haha")
            || lower.Contains("lol")
            || lower.Contains("lmao")
        )
        {
            //higher relationship = softer playful responses
            if (
                chat.relationshipLevel > 14
            )
            {
                return chat.RandomChoice(
                    "okay you're enjoying this too much",

                    "i hate that i laughed at that",

                    "you're getting dangerously confident now",

                    "alright that one was decent",

                    "i'm pretending to be offended but that was funny"
                );
            }

            return chat.RandomChoice(
                "don't laugh at my suffering",

                "wow you're finding this entertaining",

                "you sound suspiciously pleased with yourself",

                "you're encouraging the chaos"
            );
        }

        //dramatic accusations
        if (
            chat.ContainsAny(
                lower,
                "dramatic",
                "overreacting"
            )
        )
        {
            return chat.RandomChoice(
                "i'm NOT dramatic",

                "okay maybe slightly dramatic",

                "listen sometimes drama is necessary",

                "being dramatic builds character",

                "i prefer emotionally expressive"
            );
        }

        //progressive escalation system
        //makes reactions feel more dynamic over time
        if (
            responses >= 2
            && Random.value < 0.20f
        )
        {
            return chat.RandomChoice(
                "okay now you're clearly winding me up deliberately",

                "this conversation has become targeted harassment",

                "you're testing my patience now",

                "i can tell you're enjoying this way too much",

                "this friendship suddenly feels dangerous"
            );
        }

        //occasional softer responses
      
        if (
            Random.value < 0.08f
        )
        {
            return chat.RandomChoice(
                "i'm acting offended but i'm mostly impressed by the confidence",

                "the commitment to bullying me is incredible",

                "you've clearly practiced these insults",

                "i respect the dedication to chaos"
            );
        }

        //relationship memory callbacks
        if (
            chat.relationshipLevel > 15
            && Random.value < 0.05f
        )
        {
            return chat.RandomChoice(
                "compared to some of your older chaos this barely surprises me",

                "i should've expected this from you by now",

                "every conversation with you becomes nonsense eventually",

                "you consistently create problems somehow"
            );
        }

        //naturally exits after enough exchanges
        if (
            responses >= 4
            || chat.emotionLoops
            >= chat.emotionPersistence
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "alright i'll stop being dramatic",

                "fine i'll recover eventually",

                "before this becomes a legal dispute let's move on",

                "right enough bullying me for one day",

                "i survived somehow",

                "my lawyers will still hear about this"
            )
            + " "
            + chat.Emoji("facepalm")
            + " "
            + chat.GetConversationContinuation();
        }

        //default defensive responses
        return chat.RandomChoice(
            "wow okay rude",

            "can't believe you'd say that to me",

            "i see how it is",

            "that's damage right there",

            "i'm being cyberbullied",

            "this friendship is under investigation",

            "absolutely unbelievable behaviour"
        )
        + " "
        + chat.Emoji("awkward");
    }
}