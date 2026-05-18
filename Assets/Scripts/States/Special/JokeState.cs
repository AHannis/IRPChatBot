using UnityEngine;

public class JokeState : ChatState
{
    int jokeExchanges = 0;

    public JokeState(
        ChatManager manager
    ) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Playful;

        // playful state creates fake conversational chemistry
        chat.SendAIImmediate(
            chat.RandomChoice(
                "go on then menace",
                "i can already tell this is going to be nonsense",
                "i'm emotionally preparing myself already",
                "why do i suddenly not trust this conversation",
                "this feels dangerous already",
                "alright what chaos are you about to say now?"
            )
        );
    }

    public override string HandleInput(
        string input
    )
    {
        jokeExchanges++;

        string lower =
            input.ToLower();

        chat.relationshipLevel++;

        if (
            chat.ContainsAny(
                lower,
                "dementia",
                "memory",
                "forgot"
            )
        )
        {
            return chat.RandomChoice(
                "i forgot ONE thing",
                "wow suddenly i'm elderly apparently",
                "the disrespect is unbelievable",
                "i'm being attacked in my own messages",
                "okay that one was slightly funny",
                "my memory's fine leave me alone"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "broken",
                "robot",
                "npc",
                "ai"
            )
        )
        {
            return chat.RandomChoice(
                "wow rude",
                "i'm trying my best here",
                "not the npc allegations",
                "i've become emotionally damaged from this conversation",
                "that's rich coming from you",
                "i knew you'd eventually bully me"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "old",
                "boomer",
                "ancient"
            )
        )
        {
            return chat.RandomChoice(
                "watch it",
                "i'm not THAT old",
                "the disrespect from younger generations is unbelievable",
                "wow okay i'm aging in real time",
                "one more comment like that and i'm cancelling christmas"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "haha",
                "lol",
                "lmao",
                "funny"
            )
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "alright that one was decent",
                "okay i'll give you that",
                "you're still strange though",
                "i hate that i laughed",
                "you've got issues",
                "alright maybe you're a little funny"
            );
        }

        // occasional random replies stop patterns feeling too robotic
        if (
            Random.value < 0.15f
        )
        {
            return chat.RandomChoice(
                "you genuinely wake up and choose chaos",
                "your messages always feel slightly threatening",
                "you definitely laugh at your own jokes instantly",
                "i can never tell if you're joking or plotting something",
                "you would've absolutely bullied me in school"
            );
        }

        if (
            jokeExchanges >= 4
        )
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway before this becomes a court case",
                "right enough bullying me for one day",
                "you've had enough chaos for now",
                "this conversation's becoming legally dangerous",
                "anyway what've you actually been up to?",
                "right let's return to normal society for a minute"
            );
        }

        return chat.RandomChoice(
            "you're ridiculous",
            "i'm judging you slightly",
            "you've too much free time",
            "i walked right into that one",
            "you're absolutely a problem",
            "you enjoy causing chaos way too much",
            "i can't believe this is my life now"
        );
    }
}