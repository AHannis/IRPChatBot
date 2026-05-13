using UnityEngine;

public class DefensiveState : ChatState
{
    int responses = 0;

    public DefensiveState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Playful;

        chat.SendAIImmediate(
            chat.RandomChoice(
                "wow okay i'm being attacked already",
                "this feels targeted",
                "i've entered my defensive era apparently",
                "the disrespect levels are unbelievable",
                "i'm suddenly under emotional assault",
                "i was not emotionally prepared for this"
            )
        );
    }

    public override string HandleInput(string input)
    {
        responses++;

        string lower =
            input.ToLower();

        if (
            lower.Contains("sorry")
            || lower.Contains("jk")
            || lower.Contains("joking")
            || lower.Contains("kidding")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "yeah yeah i'll forgive you this time",
                "mhm sure you were joking",
                "you're lucky i'm emotionally resilient",
                "i'll recover eventually",
                "the emotional scars remain",
                "i expect financial compensation honestly"
            )
            + " "
            + chat.Emoji("smile");
        }

        if (
            lower.Contains("broken")
            || lower.Contains("npc")
            || lower.Contains("robot")
            || lower.Contains("ai")
        )
        {
            return chat.RandomChoice(
                "not the npc allegations",
                "i'm trying my best here",
                "wow suddenly i'm artificial intelligence apparently",
                "i've become emotionally damaged from this conversation",
                "that's rich coming from you",
                "you say one weird sentence and suddenly you're an npc apparently"
            );
        }

        if (
            lower.Contains("old")
            || lower.Contains("dementia")
            || lower.Contains("memory")
        )
        {
            return chat.RandomChoice(
                "i forgot one thing",
                "wow apparently i'm ancient now",
                "suddenly i'm 94 years old apparently",
                "the disrespect is unbelievable",
                "you wait until YOU start forgetting things",
                "my memory is still better than yours probably"
            );
        }

        if (
            lower.Contains("mean")
            || lower.Contains("rude")
            || lower.Contains("bully")
        )
        {
            return chat.RandomChoice(
                "YOU started this",
                "i'm somehow the villain now apparently",
                "this is emotional warfare",
                "i'm being framed",
                "wow okay i'm the bad guy now",
                "history will remember me as the victim"
            );
        }

        if (
            lower.Contains("haha")
            || lower.Contains("lol")
            || lower.Contains("lmao")
        )
        {
            return chat.RandomChoice(
                "don't laugh at my suffering",
                "wow enjoying my emotional collapse huh",
                "you're finding this way too entertaining",
                "i can feel the judgement",
                "you're absolutely encouraging the chaos",
                "you sound way too pleased with yourself"
            );
        }

        if (
            lower.Contains("dramatic")
            || lower.Contains("overreacting")
        )
        {
            return chat.RandomChoice(
                "i'm NOT dramatic",
                "okay maybe slightly dramatic",
                "listen sometimes drama is necessary",
                "i prefer emotionally expressive honestly",
                "being dramatic builds character"
            );
        }

        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "this friendship feels hostile suddenly",
                "i'm filing emotional complaints after this",
                "i deserve compensation honestly",
                "you're lucky i'm mentally strong",
                "i'm surviving purely out of resilience"
            );
        }

        if (responses >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "alright i'll stop being dramatic",
                "fine i'll recover emotionally eventually",
                "anyway before this becomes a legal dispute",
                "right enough bullying me for one day",
                "i've survived the emotional trauma somehow",
                "my lawyers will be in contact"
            )
            + " "
            + chat.Emoji("facepalm");
        }

        return chat.RandomChoice(
            "wow okay rude",
            "can't believe you'd say that to me",
            "i see how it is",
            "that's emotional damage right there",
            "i'm being cyberbullied",
            "this friendship is under investigation",
            "absolutely unbelievable behaviour"
        )
        + " "
        + chat.Emoji("awkward");
    }
}