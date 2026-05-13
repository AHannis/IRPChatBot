using UnityEngine;

public class JokeState : ChatState
{
    int jokeExchanges = 0;

    public JokeState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Playful;

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

    public override string HandleInput(string input)
    {
        jokeExchanges++;

        string lower =
            input.ToLower();

        chat.relationshipLevel++;

        if (
            lower.Contains("dementia")
            || lower.Contains("memory")
            || lower.Contains("forgot")
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
            lower.Contains("broken")
            || lower.Contains("robot")
            || lower.Contains("npc")
            || lower.Contains("ai")
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
            lower.Contains("old")
            || lower.Contains("boomer")
            || lower.Contains("ancient")
        )
        {
            return chat.RandomChoice(
                "watch it",
                "i'm not THAT old",
                "the disrespect from younger generations is unbelievable",
                "wow okay i'm being aged in real time",
                "one more comment like that and i'm cancelling christmas"
            );
        }

        if (
            lower.Contains("wow")
            || lower.Contains("rude")
            || lower.Contains("mean")
            || lower.Contains("bully")
        )
        {
            return chat.RandomChoice(
                "i'm somehow the victim here",
                "wow now I'M rude apparently",
                "this is emotional warfare",
                "you're the one bullying me right now",
                "i see how it is",
                "i'll remember this betrayal"
            );
        }

        if (
            lower.Contains("haha")
            || lower.Contains("lol")
            || lower.Contains("lmao")
            || lower.Contains("funny")
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

        if (
            lower.Contains("meme")
            || lower.Contains("tiktok")
            || lower.Contains("brainrot")
            || lower.Contains("skibidi")
        )
        {
            return chat.RandomChoice(
                "your generation worries me",
                "i understood maybe three words there",
                "internet humour gets stranger every year",
                "i genuinely don't know what's happening anymore",
                "modern humour feels like psychological warfare",
                "half of your jokes sound AI generated"
            );
        }

        if (
            lower.Contains("cry")
            || lower.Contains("crying")
            || lower.Contains("dead")
        )
        {
            return chat.RandomChoice(
                "please survive the conversation",
                "don't die laughing on me now",
                "you'll recover eventually",
                "you're being dramatic",
                "i'm choosing to believe i'm hilarious now"
            );
        }

        if (
            Random.value < 0.18f
        )
        {
            return chat.RandomChoice(
                "you would've absolutely bullied me in school",
                "you genuinely wake up and choose chaos",
                "your messages always feel slightly threatening",
                "you definitely laugh at your own jokes instantly",
                "i can never tell if you're joking or plotting something"
            );
        }

        if (jokeExchanges >= 4)
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