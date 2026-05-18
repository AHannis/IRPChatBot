using UnityEngine;

public class FamilyState : ChatState
{
    int familyExchanges = 0;

    public FamilyState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.lastTopic = "family";

        if (chat.exchangesSinceFamilyQuestion < 5)
        {
            return;
        }

        chat.exchangesSinceFamilyQuestion = 0;

        // more personal intros at higher relationship
        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "your family still chaotic?",
                    "heard from everyone lately or are they all causing problems again?",
                    "how's the family madness going then?",
                    "your aunt still creating absolute nonsense?",
                    "i swear every family gathering turns into psychological warfare"
                )
            );

            return;
        }

        chat.SendAIImmediate(
            chat.RandomChoice(
                "heard from the family lately?",
                "everyone still alive and causing chaos?",
                "family drama or peaceful for once?",
                "how's everyone doing then?",
                "your family still surviving?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        familyExchanges++;

        string lower =
            input.ToLower();

        if (
            lower.Contains("good")
            || lower.Contains("fine")
            || lower.Contains("great")
            || lower.Contains("okay")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "good. nice when everyone's behaving for once",
                "that's rare",
                "love that for you",
                "miracles really do happen",
                "family peace never lasts long though"
            );
        }

        if (
            lower.Contains("drama")
            || lower.Contains("arguing")
            || lower.Contains("fight")
            || lower.Contains("chaos")
        )
        {
            return chat.RandomChoice(
                "families are exhausting",
                "there's ALWAYS something happening somehow",
                "yeah that sounds about right",
                "family drama spreads unbelievably fast",
                "every family has at least one chaos generator"
            )
            + ". "
            + chat.RandomChoice(
                "you involved in it or hiding from it?",
                "who started it this time?",
                "sounds emotionally exhausting",
                "i swear families invent problems sometimes",
                "you surviving it alright?"
            );
        }

        if (
            lower.Contains("mum")
            || lower.Contains("mom")
            || lower.Contains("dad")
            || lower.Contains("sister")
            || lower.Contains("brother")
            || lower.Contains("cousin")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "they still putting up with you then?",
                "bless them",
                "family members deserve medals",
                "sounds like absolute chaos",
                "give them my regards"
            );
        }

        if (
            lower.Contains("visit")
            || lower.Contains("seeing them")
            || lower.Contains("going over")
        )
        {
            return chat.RandomChoice(
                "good. don't disappear forever",
                "see that's healthy",
                "families matter even when they're chaotic",
                "someone's finally being social",
                "good. people need checking in on sometimes"
            );
        }

        if (
            lower.Contains("miss")
            || lower.Contains("passed")
            || lower.Contains("gone")
            || lower.Contains("funeral")
        )
        {
            chat.ChangeState(
                new ComfortState(chat)
            );

            return chat.RandomChoice(
                "yeah that's never easy",
                "some people leave a massive gap behind them",
                "grief hits people in strange ways",
                "that kind of thing stays with you",
                "take care of yourself alright?"
            );
        }

        if (
            lower.Contains("annoying")
            || lower.Contains("strict")
            || lower.Contains("shouting")
        )
        {
            return chat.RandomChoice(
                "families know exactly how to test your patience",
                "people somehow get more annoying when they're related to you",
                "living with people is emotionally dangerous",
                "family arguments always start over the dumbest things",
                "everyone eventually snaps"
            );
        }

        if (
            lower.Contains("love")
            || lower.Contains("close")
            || lower.Contains("supportive")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "that's genuinely good",
                "not everybody gets that kind of support",
                "hold onto people like that",
                "good families make a massive difference",
                "that's actually really nice to hear"
            );
        }

        // softer emotional replies occasionally
        if (
            Random.value < 0.12f
            && chat.relationshipLevel >= 15
        )
        {
            return chat.RandomChoice(
                "family stuff really sticks with people",
                "having people around you matters more than most people admit",
                "life feels less heavy when you've got decent people around",
                "good support systems genuinely change people",
                "people need people sometimes"
            );
        }

        if (familyExchanges >= 4)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "families are complicated",
                "every family ends up chaotic eventually",
                "sounds like life's been busy",
                "people are emotionally exhausting",
                "family conversations always become chaos somehow"
            )
            + ". "
            + chat.RandomChoice(
                "what else have you been up to lately?",
                "anything interesting going on otherwise?",
                "life been alright outside of that?",
                "you keeping busy lately?",
                "what's been happening with you then?"
            );
        }

        return chat.RandomChoice(
            "families are complicated",
            "every family's a bit chaotic",
            "sounds emotionally exhausting",
            "family life never stays peaceful long",
            "people are hard work",
            "that's family life for you"
        );
    }
}