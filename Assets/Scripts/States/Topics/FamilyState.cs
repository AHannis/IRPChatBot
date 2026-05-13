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

        if (chat.relationshipLevel >= 20)
        {
            chat.SendAIImmediate(
                chat.RandomChoice(
                    "your family still chaotic honestly?",
                    "heard from everyone lately or are they all causing problems again?",
                    "how's the family madness going then?",
                    "your aunt still creating absolute nonsense?",
                    "i swear every family gathering turns into psychological warfare honestly"
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
                "your family still surviving honestly?"
            )
        );
    }

    public override string HandleInput(string input)
    {
        familyExchanges++;

        string lower = input.ToLower();

        if (
            lower.Contains("good")
            || lower.Contains("fine")
            || lower.Contains("great")
            || lower.Contains("okay")
        )
        {
            chat.relationshipLevel++;

            return chat.RandomChoice(
                "good. nice when everyone's behaving for once honestly",
                "that's rare honestly",
                "love that for you",
                "miracles really do happen honestly",
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
                "families are exhausting honestly",
                "there's ALWAYS something happening somehow",
                "yeah that sounds about right honestly",
                "family drama spreads unbelievably fast",
                "every family has at least one chaos generator honestly"
            )
            + ". "
            + chat.RandomChoice(
                "you involved in it or hiding from it?",
                "who started it this time?",
                "sounds emotionally exhausting honestly",
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
                "they still putting up with you then honestly?",
                "bless them honestly",
                "family members deserve medals honestly",
                "sounds like absolute chaos honestly",
                "give them my regards honestly"
            );
        }

        if (
            lower.Contains("visit")
            || lower.Contains("seeing them")
            || lower.Contains("going over")
        )
        {
            return chat.RandomChoice(
                "good honestly. don't disappear forever",
                "see that's healthy honestly",
                "families matter even when they're chaotic",
                "someone's finally being social honestly",
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
            chat.ChangeState(new ComfortState(chat));

            return chat.RandomChoice(
                "yeah that's never easy honestly",
                "some people leave a massive gap behind them",
                "grief hits people in strange ways honestly",
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
                "families know exactly how to test your patience honestly",
                "people somehow get more annoying when they're related to you",
                "living with people is emotionally dangerous honestly",
                "family arguments always start over the dumbest things",
                "everyone eventually snaps honestly"
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
                "that's genuinely good honestly",
                "not everybody gets that kind of support",
                "hold onto people like that honestly",
                "good families make a massive difference honestly",
                "that's actually really nice to hear"
            );
        }

        if (familyExchanges >= 4)
        {
            chat.ChangeState(new CasualState(chat));

            return chat.RandomChoice(
                "families are complicated honestly",
                "every family ends up chaotic eventually",
                "sounds like life's been busy honestly",
                "people are emotionally exhausting honestly",
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
            "families are complicated honestly",
            "every family's a bit chaotic",
            "sounds emotionally exhausting honestly",
            "family life never stays peaceful long",
            "people are hard work honestly",
            "that's family life for you honestly"
        );
    }
}