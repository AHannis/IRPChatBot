using UnityEngine;

public class CasualState : ChatState
{
    int casualExchanges = 0;

    public CasualState(ChatManager manager)
        : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        casualExchanges++;

        string lower =
            input.ToLower();

        if (
            chat.ContainsAny(
                lower,
                "work",
                "working",
                "job",
                "school",
                "college",
                "uni",
                "gaming",
                "games",
                "family",
                "mum",
                "dad",
                "sleep",
                "tired",
                "food",
                "hungry",
                "sing",
                "song",
                "roleplay",
                "sad",
                "upset",
                "stress",
                "crying"
            )
        )
        {
            chat.ChangeState(
                new RouterState(chat)
            );

            return chat.currentState
                .HandleInput(input);
        }

        if (
            lower == "anyway"
            || lower == "anywaay"
            || lower == "anyways"
        )
        {
            return chat.RandomChoice(
                "right anyway what's been going on with you?",
                "anyway enough about that honestly",
                "right changing subject before this gets weirder",
                "ANYWAY " + chat.Emoji("laugh"),
                "right so what've you been up to lately?",
                "moving on swiftly"
            );
        }

        if (
            lower.Contains("what was the question")
            || lower.Contains("what question")
            || lower.Contains("which question")
        )
        {
            if (chat.lastQuestionTopic == "age")
            {
                return chat.RandomChoice(
                    "your age " + chat.Emoji("laugh"),
                    "i literally asked how old you are",
                    "how old are you then?",
                    "the age question detective"
                );
            }

            if (chat.lastQuestionTopic == "life")
            {
                return chat.RandomChoice(
                    "what you've been up to lately",
                    "how life's been treating you",
                    "what's been going on with you"
                );
            }

            return chat.RandomChoice(
                "honestly i've forgotten too",
                "good question actually",
                "i got distracted halfway through"
            );
        }

        if (
            lower == "yeah"
            || lower == "yep"
            || lower == "true"
            || lower == "fair"
            || lower == "lol"
            || lower == "haha"
            || lower == "lmao"
        )
        {
            if (
                chat.lastBotMessage.Contains("?")
                || chat.lastBotMessage.Contains("what've")
                || chat.lastBotMessage.Contains("how've")
                || chat.lastBotMessage.Contains("never answered")
            )
            {
                return chat.RandomChoice(
                    "right so answer me then",
                    "go on then i'm listening",
                    "well?",
                    "i'm waiting",
                    "you still dodging the question then?"
                );
            }

            return chat.RandomChoice(
              "exactly " + chat.Emoji("laugh"),
              "right? anyway what've you been up to lately?",
              "you get it honestly",
              "see what i mean though",
             "yeah that's what i mean",
                 "ANYWAY what's new with you?",
             "right so what's been going on lately?",
             "you still surviving at least?",
                     "what chaos have you caused lately?"
            );
        }

        if (
            lower == "okay"
            || lower == "ok"
            || lower == "alright"
            || lower == "sure"
        )
        {
            return chat.RandomChoice(
                "right continue then",
                "go on i'm listening",
                "fair enough honestly",
                "right where were we?",
                "okay then"
            );
        }

        if (
            chat.awaitingTopicShift
        )
        {
            chat.awaitingTopicShift =
                false;

            return chat.RandomChoice(
                "anyway what's been going on with you lately?",
                "right changing subject honestly",
                "ANYWAY",
                "so what else is new?",
                "moving on swiftly"
            );
        }

        string topic =
            chat.ExtractTopic(input);
        if (
            !string.IsNullOrEmpty(topic)
            && input.Split(' ').Length >= 6
            && topic.Split(' ').Length >= 2
            && !lower.Contains("?")
            && !chat.IsShortReply(lower)
            && !chat.IsLikelyActivityResponse(input)
            && !topic.StartsWith("not ")
            && !topic.StartsWith("just ")
            && !topic.StartsWith("yeah ")
            && !topic.StartsWith("okay ")
            && UnityEngine.Random.value < 0.20f
        )
        {
            chat.awaitingTopicShift =
                true;

            return chat.RandomChoice(
                topic + " wasn't on my bingo card today",
                "why does " + topic + " sound believable coming from you",
                "i have several questions about " + topic,
                topic + " sounds oddly specific",
                "that's a strange sentence out of context"
            );
        }

        if (
            !chat.knowsUserAge
            && !chat.askedAgeRecently
            && casualExchanges >= 4
            && UnityEngine.Random.value < 0.10f
        )
        {
            chat.askedAgeRecently =
                true;

            chat.lastQuestionTopic =
                "age";

            chat.ChangeState(
                new AgeState(chat)
            );

            return chat.RandomChoice(
                "wait how old are you actually now?",
                "random question how old are you these days?"
            );
        }

        if (
            UnityEngine.Random.value
            < 0.04f
            && !chat.storyActive
        )
        {
            chat.ChangeState(
                new UncleStoryState(chat)
            );

            return
                "you know what happened earlier?";
        }

        if (
    chat.IsReciprocalResponse(
        lower
    )
)
        {
            string reply =
                chat.RandomChoice(
                    "i'm alright honestly",
                    "surviving somehow " + chat.Emoji("laugh"),
                    "bit tired but alive",
                    "doing alright",
                    "mentally somewhere between relaxed and crashing"
                );

            if (
                UnityEngine.Random.value
                < 0.30f
            )
            {
                reply +=
                    ". "
                    + chat.GetDynamicFollowUp();
            }

            return reply;
        }

        string reflective =
            chat.GenerateReflectiveResponse(
                input
            );

        if (
            !string.IsNullOrEmpty(reflective)
            && !chat.IsShortReply(lower)
            && !chat.IsReciprocalResponse(lower)
            && UnityEngine.Random.value < 0.18f
        )
        {
            return reflective;
        }

        if (
    casualExchanges >= 5
)
        {
            casualExchanges = 0;

            return
                chat.GetConversationContinuation();
        }

        if (
            UnityEngine.Random.value < 0.12f
            && !chat.storyActive
        )
        {
            chat.ChangeState(
                new UncleStoryState(chat)
            );

            return chat.RandomChoice(
                "you know what happened earlier?",
                "right i've gotta tell you this",
                "okay funniest thing happened earlier",
                "actually listen to this",
                "speaking of chaos"
            );
        }

        if (
            UnityEngine.Random.value < 0.45f
        )
        {
            return chat.RandomChoice(
                "anyway what've you been up to lately?",
                "so what's been going on with you then?",
                "what's new with you lately?",
                "anything interesting happened lately?",
                "you been doing anything fun recently?",
                "what's the latest chaos in your life?",
                "life been alright lately?"
            );
        }

        string finalReply =
            chat.GetNaturalReply();

        return
            chat.MaybeAddName(
                finalReply
            );
    }
}