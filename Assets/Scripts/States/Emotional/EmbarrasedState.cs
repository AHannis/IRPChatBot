using UnityEngine;

public class EmbarrasedState : ChatState
{
    int awkwardLevel = 0;

    public EmbarrasedState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Playful;

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

    public override string HandleInput(string input)
    {
        awkwardLevel++;

        string lower =
            input.ToLower();

        if (
            lower.Contains("sorry")
            || lower.Contains("nevermind")
            || lower.Contains("forget it")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "yeah let's both pretend that never happened",
                "agreed. erased from history immediately",
                "i'm deleting this from my brain",
                "that moment never existed",
                "good decision honestly",
                "some stories should stay buried"
            )
            + " "
            + chat.Emoji("awkward");
        }

        if (
            lower.Contains("fell")
            || lower.Contains("tripped")
            || lower.Contains("accidentally")
            || lower.Contains("cringe")
            || lower.Contains("awkward")
        )
        {
            return chat.RandomChoice(
                "nah i'd genuinely never recover",
                "that's the kind of thing that attacks you at 2am later",
                "i would've evaporated on the spot",
                "the second-hand embarrassment is powerful",
                "i think i'd need to legally change identities after that",
                "my soul would've left my body instantly"
            );
        }

        if (
            lower.Contains("texted")
            || lower.Contains("sent")
            || lower.Contains("message")
            || lower.Contains("snap")
        )
        {
            return chat.RandomChoice(
                "please tell me you didn't send it to the wrong person",
                "messages are genuinely dangerous",
                "that would've shortened my lifespan",
                "i'd throw my phone into the sea",
                "technology causes psychological damage",
                "phones ruin lives honestly"
            );
        }

        if (
            lower.Contains("teacher")
            || lower.Contains("class")
            || lower.Contains("school")
        )
        {
            return chat.RandomChoice(
                "school embarrassment hits different honestly",
                "nah i'd skip school for a week after that",
                "teenagers never let people recover from embarrassment",
                "classrooms are brutal socially",
                "everyone remembers embarrassing moments at school forever"
            );
        }

        if (
            lower.Contains("crush")
            || lower.Contains("boy")
            || lower.Contains("girl")
            || lower.Contains("relationship")
        )
        {
            return chat.RandomChoice(
                "romantic embarrassment is genuinely fatal",
                "your brain will replay that forever",
                "nah i'd collapse instantly",
                "that's painful on another level",
                "teenage crush situations are terrifying"
            );
        }

        if (
            lower.Contains("haha")
            || lower.Contains("lol")
            || lower.Contains("lmao")
        )
        {
            return chat.RandomChoice(
                "you're laughing but i'd still be recovering",
                "nah that would've haunted me for years",
                "the confidence to survive that is impressive",
                "emotionally devastating",
                "i respect the recovery",
                "honestly fair play for surviving it"
            );
        }

        if (
            Random.value < 0.18f
        )
        {
            return chat.RandomChoice(
                "embarrassing moments attack at the worst times",
                "your brain really stores cringe moments forever",
                "i swear embarrassing memories become stronger at night",
                "humans are not built to recover from awkward moments quickly",
                "social anxiety remembers everything honestly"
            );
        }

        if (awkwardLevel >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "ANYWAY moving on quickly",
                "right we're changing topic before this gets worse",
                "i physically cannot handle more embarrassment",
                "that's enough emotional damage for one day",
                "anyway before we both evaporate from cringe",
                "right let's never discuss this again"
            )
            + " "
            + chat.Emoji("facepalm")
            + " "
            + chat.brain.GetConversationContinuation();
        }

        return chat.RandomChoice(
            "you really said that with confidence huh",
            "i don't even know how to respond to that",
            "absolutely unbelievable behaviour",
            "nah that's painful",
            "i would've left the country after that",
            "that would've kept me awake for months"
        )
        + " "
        + chat.Emoji("awkward");
    }
}