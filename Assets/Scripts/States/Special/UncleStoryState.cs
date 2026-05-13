using UnityEngine;

public class UncleStoryState : ChatState
{
    int storyStep = 0;

    string currentStory = "";

    public UncleStoryState(ChatManager manager) : base(manager)
    {
    }

    public override void Enter()
    {
        chat.currentMood =
            ChatManager.Mood.Playful;

        int story =
            Random.Range(0, 5);

        switch (story)
        {
            case 0:

                currentStory = "tesco";

                chat.storyActive = true;

                chat.activeStory = "tesco";

                chat.activeStoryStep = 0;

                chat.currentConversationContext =
                    "tesco_story";

                chat.contextStep = 0;

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "you know what happened to me at tesco earlier?",
                        "honestly remind me to never go tesco again",
                        "i had the weirdest interaction at tesco earlier honestly",
                        "i swear supermarkets are where sanity goes to die honestly"
                    )
                );

                break;

            case 1:

                currentStory = "microwave";

                chat.currentConversationContext =
                    "microwave_story";

                chat.contextStep = 0;

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "your aunt nearly destroyed the microwave earlier honestly",
                        "i nearly witnessed a kitchen disaster earlier",
                        "honestly i think your aunt is secretly at war with appliances",
                        "i heard a noise from the kitchen earlier that changed me psychologically"
                    )
                );

                break;

            case 2:

                currentStory = "neighbour";

                chat.currentConversationContext =
                    "neighbour_story";

                chat.contextStep = 0;

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "my neighbour cornered me for half an hour earlier",
                        "i made eye contact with the neighbour earlier and instantly regretted it",
                        "you ever get trapped in conversations you physically can't escape from honestly?",
                        "i accidentally activated neighbour dialogue earlier"
                    )
                );

                break;

            case 3:

                currentStory = "coffee";

                chat.currentConversationContext =
                    "coffee_story";

                chat.contextStep = 0;

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "i made the strongest coffee known to mankind earlier",
                        "i think i accidentally overdosed on caffeine earlier honestly",
                        "my coffee this morning nearly restarted my nervous system",
                        "i genuinely think i transcended reality briefly earlier"
                    )
                );

                break;

            default:

                currentStory = "shopping";

                chat.currentConversationContext =
                    "shopping_story";

                chat.contextStep = 0;

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "remind me to never go shopping on a saturday again honestly",
                        "weekend shopping is basically psychological warfare",
                        "honestly supermarkets on weekends should come with safety training",
                        "i survived saturday shopping barely honestly"
                    )
                );

                break;
        }
    }

    public override string HandleInput(string input)
    {
        string lower =
            input.ToLower();

        if (
            lower.Contains("what happened")
            || lower.Contains("then")
            || lower.Contains("why")
            || lower.Contains("what did she say")
            || lower.Contains("what did you do")
            || lower.Contains("go on")
            || lower.Contains("seriously")
        )
        {
            chat.contextStep++;

            if (currentStory == "tesco")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "this random bloke trapped me talking about lawnmowers for forty minutes honestly",
                        "i made ONE mistake and nodded at this guy near gardening supplies",
                        "he started explaining grass maintenance like it was ancient forbidden knowledge"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "your aunt fully abandoned me aisle by aisle honestly",
                        "i could literally SEE her escaping while i was trapped there",
                        "she looked at me struggling and just disappeared honestly"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "and the worst part is i still don't even own a lawnmower",
                        "i don't even remember how the conversation started honestly",
                        "somehow we ended up discussing fence regulations"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    chat.ChangeState(
                        new CasualState(chat)
                    );

                    return chat.RandomChoice(
                        "whole thing changed me psychologically honestly",
                        "i still don't trust garden centres after that",
                        "i've never escaped a conversation slower in my life honestly"
                    )
                    + ". "
                    + chat.GetFollowUpTopic();
                }
            }

            if (currentStory == "microwave")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "she tried heating food wrapped in foil somehow",
                        "i suddenly heard this horrible crackling sound from the kitchen",
                        "there were ACTUAL sparks honestly"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "and somehow I'M the dramatic one for unplugging it apparently",
                        "she just stared at me while i panicked honestly",
                        "apparently preventing electrical fires is overreacting now"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "honestly i saw my life flash before my eyes briefly",
                        "that microwave's never emotionally recovering honestly",
                        "the kitchen smelt haunted afterwards"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    chat.ChangeState(
                        new CasualState(chat)
                    );

                    return chat.RandomChoice(
                        "technology genuinely fears your aunt honestly",
                        "i don't trust appliances anymore honestly",
                        "whole thing nearly sent me into cardiac arrest"
                    )
                    + ". "
                    + chat.GetFollowUpTopic();
                }
            }

            if (currentStory == "neighbour")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "he cornered me talking about bins for ages honestly",
                        "the conversation somehow became a lecture about fence paint",
                        "i made eye contact once and it was over for me"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "i nodded for so long my neck nearly locked up",
                        "i stopped understanding the conversation after like ten minutes honestly",
                        "he genuinely had powerpoint presentation energy"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "i think he just wanted someone to witness his thoughts honestly",
                        "the man speaks like side quests honestly",
                        "i forgot where i was halfway through"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    chat.ChangeState(
                        new CasualState(chat)
                    );

                    return chat.RandomChoice(
                        "some people are conversational snipers honestly",
                        "i should've pretended i had a phone call",
                        "whole experience drained my life force"
                    )
                    + ". "
                    + chat.GetFollowUpTopic();
                }
            }

            if (currentStory == "coffee")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "i accidentally used about four times too much coffee",
                        "one sip nearly restarted my nervous system",
                        "my heartbeat achieved a new speed honestly"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "my hands were vibrating for two hours",
                        "i cleaned the entire kitchen at lightning speed afterwards",
                        "i genuinely think i saw through time briefly"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "i sat down afterwards and could still hear colours honestly",
                        "i was one bad decision away from running a marathon honestly",
                        "i transcended normal human caffeine limits"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    chat.ChangeState(
                        new CasualState(chat)
                    );

                    return chat.RandomChoice(
                        "coffee should probably be regulated honestly",
                        "never trusting myself with caffeine again",
                        "i nearly ascended spiritually"
                    )
                    + ". "
                    + chat.GetFollowUpTopic();
                }
            }

            if (currentStory == "shopping")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "this kid was screaming over dinosaur nuggets honestly",
                        "someone dropped a bottle and the entire shop went silent",
                        "it felt like survival television honestly"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "your aunt vanished while i was emotionally suffering",
                        "i nearly abandoned my basket and escaped honestly",
                        "supermarkets shouldn't be this stressful psychologically"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "and somehow there were no normal trolleys left either",
                        "every aisle felt like combat honestly",
                        "i don't understand why everyone shops at the exact same time"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    chat.ChangeState(
                        new CasualState(chat)
                    );

                    return chat.RandomChoice(
                        "whole thing felt like psychological warfare honestly",
                        "i need at least three business days to recover",
                        "shopping on weekends is not natural"
                    )
                    + ". "
                    + chat.GetFollowUpTopic();
                }
            }
        }

        storyStep++;

        if (storyStep == 1)
        {
            return chat.RandomChoice(
                "you listening or have i lost you already?",
                "honestly i still can't believe it happened",
                "i swear weird things only happen when i'm outside",
                "i'm still emotionally processing it honestly"
            );
        }

        if (storyStep == 2)
        {
            return chat.RandomChoice(
                "actually wait no that's not even the worst part honestly",
                "hang on i forgot the stupidest part",
                "nah wait it somehow got worse honestly"
            );
        }

        if (storyStep >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway enough about my disasters honestly",
                "right i've traumatised you enough with that story honestly",
                "anyway moving on before i relive the experience emotionally"
            )
            + ". "
            + chat.GetFollowUpTopic();
        }

        return chat.GetFollowUpTopic();
    }
}