using UnityEngine;

public class ConcernState : ChatState
{
    int concernExchanges = 0;

    public ConcernState(ChatManager manager)
        : base(manager)
    {
    }

    public override void Enter()
    {
        //sets emotional mood state
        chat.currentMood =
            ChatManager.Mood.Concerned;

        //emotional persistence helps the uncle
        //stay emotionally consistent for a few replies
        chat.emotionPersistence = 4;

        chat.emotionLoops = 0;

        //slightly more personal concern
        //unlocks at higher relationship levels
        if (
            chat.relationshipLevel > 14
        )
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "you've seemed really drained recently",
                    "i can tell something's been weighing on you",
                    "you don't sound like yourself lately",
                    "you've been carrying too much on your own haven't you",
                    "your brain sounds exhausted"
                )
            );

            return;
        }

        //opening concern message
        //kept conversational instead of overly therapeutic
        chat.SendAIImmediate(
            chat.RandomChoice(
                "you alright?",
                "something feels off with you recently",
                "talk to me properly for a second",
                "you seem stressed",
                "you've sounded a bit drained",
                "you don't sound fully yourself today"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        concernExchanges++;

        chat.emotionLoops++;

        string lower =
            input.ToLower();

        //relationship slowly increases during
        //supportive conversations
        chat.relationshipLevel += 2;


        if (
            Random.value < 0.18f
            && !chat.IsShortReply(lower)
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

        //humour indicates user is emotionally okay enough
        //to shift back into casual conversation
        if (
            lower.Contains("lol")
            || lower.Contains("lmao")
            || lower.Contains("haha")
            || lower.Contains(":cry:")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ClearContext();

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "alright dramatic",
                "you worried me for a second there",
                "okay so we're not having a breakdown then",
                "you're impossible to read sometimes",
                "you really cope through humour huh",
                "alright good you're still functioning"
            );
        }

        //user says they're okay
        if (
            lower.Contains("fine")
            || lower.Contains("okay")
            || lower.Contains("alright")
            || lower.Contains("i'm good")
            || lower.Contains("im good")
            || lower.Contains("i'll be fine")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ClearContext();

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "if you say so",
                "alright just checking",
                "don't run yourself into the ground",
                "good. just making sure",
                "you've got to look after yourself too",
                "okay. just don't bottle everything up",
                "good. your nervous system sounded exhausted"
            );
        }

        //strong emotional escalation
        //moves into comfort state
        if (
            lower.Contains("not okay")
            || lower.Contains("bad")
            || lower.Contains("awful")
            || lower.Contains("struggling")
            || lower.Contains("terrible")
            || lower.Contains("overwhelmed")
            || lower.Contains("can't cope")
            || lower.Contains("cant cope")
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah... i figured something was weighing on you",
                "sounds like you've been trying to handle a lot quietly",
                "you don't have to carry everything alone yknow",
                "feels like things have been piling up on you",
                "sometimes people keep saying they're fine until they hit their limit",
                "your brain sounds completely exhausted"
            );
        }

        //burnout / exhaustion branch
        if (
            lower.Contains("tired")
            || lower.Contains("stressed")
            || lower.Contains("busy")
            || lower.Contains("exhausted")
            || lower.Contains("drained")
            || lower.Contains("burnt out")
            || lower.Contains("burned out")
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah you sound genuinely exhausted",
                "feels like you've had too much piling up at once",
                "you've been carrying a lot mentally huh?",
                "sounds like your brain never really switches off",
                "that kind of exhaustion builds up over time",
                "your nervous system needs a holiday at this point"
            );
        }

        //school specific concern
        if (
            lower.Contains("school")
            || lower.Contains("teacher")
            || lower.Contains("exam")
        )
        {
            return chat.RandomChoice(
                "school pressure builds up ridiculously fast",
                "people underestimate how draining school gets mentally",
                "exams genuinely destroy people's sleep",
                "school stress follows people home too",
                "your brain never really gets chance to switch off there"
            );
        }

        //uni specific concern
        if (
            lower.Contains("uni")
            || lower.Contains("assignment")
            || lower.Contains("deadline")
        )
        {
            return chat.RandomChoice(
                "uni burnout is brutal",
                "deadlines pile up unbelievably fast",
                "uni sleep schedules are genuinely cursed",
                "students are somehow expected to function on fumes",
                "that workload catches up with people mentally"
            );
        }

        //work specific concern
        if (
            lower.Contains("work")
            || lower.Contains("job")
            || lower.Contains("manager")
            || lower.Contains("shift")
        )
        {
            return chat.RandomChoice(
                "work drains people mentally faster than they realise",
                "being exhausted constantly becomes normal way too quickly",
                "some jobs genuinely consume people's energy",
                "your brain still deserves rest outside work",
                "adult life is basically surviving while tired"
            );
        }

        //thanks branch
        if (
            lower.Contains("thanks")
            || lower.Contains("thank you")
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "always",
                "don't mention it",
                "i've got you",
                "you're alright kid",
                "just don't vanish for another month afterwards",
                "someone has to keep an eye on you"
            )
            + " "
            + chat.Emoji("smile");
        }

        //small occasional concern injections
        //keeps emotional realism active
        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "seriously though take care of yourself",
                "your brain deserves rest too",
                "sometimes surviving the week is enough",
                "you don't have to fix everything immediately",
                "people forget how exhausting life gets mentally",
                "your body eventually notices stress even if you ignore it",
                "people can only run on empty for so long"
            );
        }

        //conversation naturally exits concern state
        if (
            concernExchanges >= 3
            || chat.emotionLoops >= 4
        )
        {
            chat.currentMood =
                ChatManager.Mood.Neutral;

            chat.ClearContext();

            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "just don't disappear into your own head alright?",
                "make sure you're actually taking care of yourself",
                "seriously though don't bottle everything up",
                "look after yourself properly yeah?",
                "you deserve rest too",
                "anyway enough of me sounding wise for five minutes",
                "right before i become accidentally inspirational"
            );
        }

        //default concern responses

        return chat.RandomChoice(
            "you've seemed a bit distant recently",
            "something definitely feels off",
            "i'm just making sure you're alright",
            "you don't always have to pretend you're fine",
            "you can actually talk to me properly yknow",
            "sometimes people hit their limit without noticing",
            "your brain sounds tired"
        );
    }
}