using UnityEngine;

public class ComfortState : ChatState
{
    int comfortExchanges = 0;

    public ComfortState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Concerned;

        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "come on what's happened?",
                    "you sound genuinely drained lately",
                    "talk to me properly for a second",
                    "life been hitting hard lately huh?",
                    "you've seemed off lately",
                    "you alright? you've not sounded yourself"
                )
            );

            return;
        }

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

    public override string HandleInput(string input)
    {
        comfortExchanges++;

        string lower =
            input.ToLower();

        chat.relationshipLevel += 2;

        if (
            lower.Contains("stress")
            || lower.Contains("overwhelmed")
            || lower.Contains("anxious")
            || lower.Contains("panic")
            || lower.Contains("pressure")
        )
        {
            return chat.RandomChoice(
                "you've been carrying too much lately",
                "one thing at a time alright?",
                "don't try dealing with everything at once",
                "your brain sounds overloaded",
                "you seriously need an actual break",
                "you've been stuck in survival mode lately huh?"
            )
            + " "
            + chat.Emoji("thumbsup");
        }

        if (
            lower.Contains("alone")
            || lower.Contains("lonely")
            || lower.Contains("nobody")
        )
        {
            return chat.RandomChoice(
                "you're not as alone as you think",
                "people care about you more than you realise",
                "don't disappear into your own head too much",
                "isolation makes everything feel worse",
                "you don't have to deal with everything silently",
                "your brain lies to you when you're overwhelmed"
            )
            + " "
            + chat.Emoji("smile");
        }

        if (
            lower.Contains("cry")
            || lower.Contains("hurts")
            || lower.Contains("pain")
            || lower.Contains("heartbroken")
        )
        {
            return chat.RandomChoice(
                "yeah some things hit unbelievably hard",
                "you're allowed to feel awful sometimes",
                "just don't bottle everything up alright?",
                "healing takes time",
                "people act like emotions are easy when they aren't",
                "some stuff stays with you longer than people expect"
            )
            + " "
            + chat.Emoji("thumbsup");
        }

        if (
            lower.Contains("tired")
            || lower.Contains("drained")
            || lower.Contains("exhausted")
        )
        {
            return chat.RandomChoice(
                "you sound emotionally exhausted",
                "your brain needs rest properly",
                "you've been running on fumes lately huh?",
                "everything feels worse when you're exhausted",
                "you seriously need time to breathe",
                "you can't keep carrying everything nonstop"
            );
        }

        if (
            lower.Contains("school")
            || lower.Contains("uni")
            || lower.Contains("work")
        )
        {
            return chat.RandomChoice(
                "life piles up ridiculously fast",
                "people underestimate how exhausting daily life gets",
                "you've had way too much on lately",
                "sometimes everything hits at once",
                "you've been mentally carrying a lot huh?",
                "your brain never really gets chance to switch off"
            );
        }

        if (
            lower.Contains("thank")
            || lower.Contains("thanks")
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "always",
                "you're alright kid",
                "don't mention it",
                "i've got you",
                "just don't vanish afterwards",
                "you don't always have to deal with stuff alone"
            )
            + " "
            + chat.Emoji("smile");
        }

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
                "you're coping through humour i see",
                "even now you're still joking",
                "you really do laugh through everything huh",
                "somehow you still find ways to joke",
                "honestly humour keeps people sane sometimes"
            );
        }

        if (
            lower.Contains("fine")
            || lower.Contains("i'm okay")
            || lower.Contains("im okay")
            || lower.Contains("i'll be okay")
        )
        {
            return chat.RandomChoice(
                "maybe. but you still sound worn out",
                "alright. just don't bottle everything up",
                "i know you say that when you're struggling sometimes",
                "just make sure you're actually looking after yourself",
                "okay. i'm just checking in on you"
            );
        }

        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "seriously though be kind to yourself",
                "your brain deserves rest too",
                "you don't have to solve everything immediately",
                "sometimes surviving the week is enough honestly",
                "people forget how exhausting life gets mentally"
            );
        }

        if (comfortExchanges >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "just take care of yourself alright?",
                "seriously though don't carry everything alone",
                "you deserve proper rest",
                "be a little kinder to yourself alright?",
                "look after yourself properly yeah?",
                "promise me you're getting some actual rest"
            );
        }

        return chat.RandomChoice(
            "i'm listening",
            "sounds rough",
            "you don't have to carry everything quietly",
            "life can be brutal sometimes",
            "you've had a lot weighing on you lately huh?",
            "sometimes people hit their limit"
        );
    }
}