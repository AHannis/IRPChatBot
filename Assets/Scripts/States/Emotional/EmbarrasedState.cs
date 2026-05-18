using UnityEngine;

public class EmbarrasedState : ChatState
{
    //tracks how awkward the conversation becomes
    int awkwardLevel = 0;

    public EmbarrasedState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        //playful mood fits teasing + awkward conversations
        chat.currentMood =
            ChatManager.Mood.Playful;

        //keeps emotional states active briefly instead of instantly disappearing
        chat.emotionPersistence =
            3;

        chat.emotionLoops = 0;

        //opening reactions establish shared social embarrassment

        chat.SendAIImmediate(
            chat.RandomChoice(
                "oh no this already sounds painful",

                "i can feel the second-hand embarrassment already",

                "why do i feel like this story ends terribly",

                "this has disaster energy",

                "i'm emotionally preparing myself",

                "something about this already feels catastrophic"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        awkwardLevel++;

        chat.emotionLoops++;

        string lower =
            input.ToLower();

        //occasionally mirrors user topic back naturally
        //conversations feel more personal
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

        //backs off if user wants to abandon the topic
        if (
            lower.Contains("sorry")
            || lower.Contains("nevermind")
            || lower.Contains("forget it")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "yeah let's both pretend that never happened",

                "agreed. erased from history immediately",

                "i'm deleting this from my brain",

                "that moment never existed",

                "good decision",

                "some stories should stay buried"
            )
            + " "
            + chat.Emoji("awkward");
        }

        //physical/social embarrassment
        if (
            chat.ContainsAny(
                lower,
                "fell",
                "tripped",
                "accidentally",
                "awkward",
                "cringe",
                "stared",
                "wave",
                "voice crack",
                "presentation",
                "mute",
                "mic",
                "zoom",
                "camera"
            )
        )
        {
            if (
                awkwardLevel >= 2
            )
            {
                return chat.RandomChoice(
                    "okay somehow this story keeps getting worse",

                    "nah your nervous system is never recovering from that",

                    "that would've ended me immediately",

                    "social damage level critical",

                    "i would've vanished into another dimension"
                );
            }

            return chat.RandomChoice(
                "that kind of memory attacks you randomly at 2am later",

                "i would've evaporated on the spot",

                "the second-hand embarrassment is powerful",

                "my soul would've left my body instantly",

                "that would've permanently altered my brain chemistry"
            );
        }

        //phone/message embarrassment
        if (
            chat.ContainsAny(
                lower,
                "texted",
                "sent",
                "message",
                "snap",
                "wrong chat"
            )
        )
        {
            return chat.RandomChoice(
                "please tell me you didn't send it to the wrong person",

                "phones genuinely ruin lives",

                "that would've shortened my lifespan",

                "i'd throw my phone directly into the sea",

                "technology causes psychological damage"
            );
        }

        //school embarrassment
        if (
            chat.ContainsAny(
                lower,
                "teacher",
                "class",
                "school",
                "called teacher mum"
            )
        )
        {
            return chat.RandomChoice(
                "school embarrassment hits differently",

                "teenagers never let people recover socially",

                "classrooms are brutal",

                "nah i'd skip school for a week after that",

                "everyone in class remembers moments forever"
            );
        }

        //relationship embarrassment
        if (
            chat.ContainsAny(
                lower,
                "crush",
                "boy",
                "girl",
                "relationship"
            )
        )
        {
            return chat.RandomChoice(
                "romantic embarrassment is genuinely fatal",

                "your brain will replay that forever",

                "that would've finished me",

                "that's painful on another level",

                "crush situations are terrifying"
            );
        }

        //laughing recovery responses
        if (
            lower.Contains("haha")
            || lower.Contains("lol")
            || lower.Contains("lmao")
        )
        {
            //higher relationship = more teasing
            if (
                chat.relationshipLevel > 12
            )
            {
                return chat.RandomChoice(
                    "you're laughing but i know that hurt",

                    "the confidence to survive that is impressive",

                    "you recover from embarrassment suspiciously fast",

                    "i respect the survival instincts"
                );
            }

            return chat.RandomChoice(
                "nah that would've haunted me for years",

                "emotionally devastating",

                "i respect the recovery",

                "you're stronger than me because i'd never mentally recover"
            );
        }

       
        // illusion of emotional understanding
        if (
            Random.value < 0.10f
        )
        {
            return chat.RandomChoice(
                "the worst part is your brain will randomly replay this in five years",

                "socially devastating",

                "your nervous system is storing this permanently",

                "humans were not designed to survive embarrassment",

                "awkward memories genuinely become immortal"
            );
        }

        //memory callbacks make relationship feel continuous
        if (
            chat.relationshipLevel > 14
            && Random.value < 0.05f
        )
        {
            return chat.RandomChoice(
                "this somehow isn't even your most embarrassing story",

                "compared to your older chaos this barely surprises me now",

                "you consistently end up in impossible situations",

                "your life genuinely plays out like a sitcom"
            );
        }

        //naturally exits awkward topic after enough exchanges
        if (
            awkwardLevel >= 4
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
                "ANYWAY moving on quickly",

                "right we're changing subject before this gets worse",

                "i physically cannot handle more embarrassment",

                "that's enough emotional damage for one day",

                "before we both evaporate from cringe let's move on",

                "right let's never discuss this again"
            )
            + " "
            + chat.Emoji("facepalm")
            + " "
            + chat.GetConversationContinuation();
        }

        //default awkward responses
        return chat.RandomChoice(
            "you really said that with confidence huh",

            "i don't even know how to respond to that",

            "absolutely unbelievable behaviour",

            "that would've destroyed me",

            "i would've left the country after that",

            "that's emotionally dangerous"
        )
        + " "
        + chat.Emoji("awkward");
    }
}