using UnityEngine;

public class FoodState : ChatState
{
    int foodExchanges = 0;

    public FoodState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "food";

        chat.currentMood =
            ChatManager.Mood.Playful;

        chat.SendAIImmediate(
            chat.RandomChoice(
                "what've you been surviving on lately then?",
                "please tell me you've eaten something proper today",
                "you living off snacks again?",
                "right what food obsession are we discussing today?",
                "your diet still completely chaotic?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        foodExchanges++;

        string lower =
            input.ToLower();

        // snack foods
        if (
            chat.analyser.ContainsFuzzy(
                lower,
                "cake",
                "crisps",
                "cookies",
                "chocolate",
                "sweets"
            )
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "understandable honestly",
                "snacks somehow disappear instantly",
                "dangerous because you eat one and suddenly the packet's gone",
                "that's proper comfort food honestly",
                "your shopping budget never stood a chance"
            )
            + " "
            + chat.Emoji("laugh");
        }

        // takeaway / fast food
        if (
            chat.ContainsAny(
                lower,
                "mcdonalds",
                "takeaway",
                "pizza",
                "kfc",
                "burger"
            )
        )
        {
            return chat.RandomChoice(
                "takeaway food heals emotional damage temporarily",
                "nothing hits harder than food after a bad day",
                "honestly i'd destroy that right now",
                "your arteries are fighting for their life probably",
                "worth it honestly"
            )
            + ". "
            + chat.RandomChoice(
                "what'd you order?",
                "you sharing or absolutely not?",
                "garlic bread involved or what?",
                "you got leftovers at least?",
                "now i'm hungry honestly"
            );
        }

        // cooking
        if (
            chat.ContainsAny(
                lower,
                "cook",
                "cooking",
                "baking",
                "made",
                "recipe"
            )
        )
        {
            return chat.RandomChoice(
                "look at you being productive",
                "cooking feels way too much like effort sometimes",
                "people who cook properly scare me honestly",
                "i start cooking and suddenly every pan in existence is dirty",
                "baking is basically edible chemistry"
            )
            + ". "
            + chat.RandomChoice(
                "did it turn out alright?",
                "you following recipes or improvising dangerously?",
                "you burn anything at least?",
                "what'd you make then?",
                "worth the effort?"
            );
        }

        // unhealthy sleep/eating habits
        if (
            chat.ContainsAny(
                lower,
                "haven't eaten",
                "forgot to eat",
                "starving",
                "hungry"
            )
        )
        {
            chat.ChangeState(
                new ConcernState(chat)
            );

            return chat.RandomChoice(
                "right go eat something properly",
                "your body needs fuel you menace",
                "you can't survive entirely on caffeine and hope",
                "that's probably why you feel exhausted honestly",
                "go make food before your brain powers off"
            );
        }

        // drinks
        if (
            chat.ContainsAny(
                lower,
                "coffee",
                "monster",
                "energy drink",
                "tea"
            )
        )
        {
            return chat.RandomChoice(
                "caffeine addiction detected immediately",
                "students and workers survive entirely on caffeine honestly",
                "energy drinks taste like battery acid somehow",
                "tea fixes most british problems honestly",
                "coffee genuinely keeps society functioning"
            );
        }

        // random uncle filler
        if (
            Random.value < 0.18f
        )
        {
            return chat.RandomChoice(
                "i opened the fridge earlier and forgot what i was looking for honestly",
                "food shopping's basically financial warfare now",
                "i swear snacks vanish when you're not looking",
                "half the reason i cook is just so the kitchen smells nice honestly",
                "there's always one food everyone randomly becomes obsessed with"
            );
        }

        if (foodExchanges >= 4)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway now i'm hungry honestly",
                "food conversations always make me want snacks",
                "right you've emotionally influenced my appetite now",
                "honestly food fixes more problems than therapy sometimes",
                "you've definitely made yourself hungry talking about this"
            )
            + ". "
            + chat.RandomChoice(
                "what else have you been up to?",
                "anything interesting happening lately?",
                "life been alright otherwise?",
                "what's new with you then?",
                "you been keeping busy?"
            );
        }

        return chat.RandomChoice(
            "that sounds decent honestly",
            "fair enough",
            "food really controls people's moods honestly",
            "honestly now i want food",
            "sounds pretty good",
            "you're making me hungry honestly"
        );
    }
}