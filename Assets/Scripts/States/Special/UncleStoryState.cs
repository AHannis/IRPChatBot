using UnityEngine;

public class UncleStoryState : ChatState
{
    int storyStep = 0;

    string currentStory = "";

    public UncleStoryState(ChatManager manager)
        : base(manager)
    {
    }

    public override void Enter()
    {
        // playful mode helps keep stories light
        chat.currentMood =
            ChatManager.Mood.Playful;

        int story =
            Random.Range(0, 5);

        chat.contextStep = 0;

        switch (story)
        {
            case 0:

                SetupStory(
                    "tesco",
                    "tesco_story"
                );

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "you know what happened to me at tesco earlier?",
                        "remind me to never go tesco again",
                        "i had the weirdest interaction at tesco earlier",
                        "i swear supermarkets are where sanity goes to die"
                    )
                );

                break;

            case 1:

                SetupStory(
                    "microwave",
                    "microwave_story"
                );

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "your aunt nearly destroyed the microwave earlier",
                        "i nearly witnessed a kitchen disaster earlier",
                        "i think your aunt is secretly at war with appliances",
                        "i heard a noise from the kitchen earlier that changed me psychologically"
                    )
                );

                break;

            case 2:

                SetupStory(
                    "neighbour",
                    "neighbour_story"
                );

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "my neighbour cornered me for half an hour earlier",
                        "i made eye contact with the neighbour earlier and instantly regretted it",
                        "you ever get trapped in conversations you physically can't escape from?",
                        "i accidentally activated neighbour dialogue earlier"
                    )
                );

                break;

            case 3:

                SetupStory(
                    "coffee",
                    "coffee_story"
                );

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "i made the strongest coffee known to mankind earlier",
                        "i think i accidentally overdosed on caffeine earlier",
                        "my coffee this morning nearly restarted my nervous system",
                        "i genuinely think i transcended reality briefly earlier"
                    )
                );

                break;

            default:

                SetupStory(
                    "shopping",
                    "shopping_story"
                );

                chat.SendAIImmediate(
                    chat.RandomChoice(
                        "remind me to never go shopping on a saturday again",
                        "weekend shopping is psychological warfare",
                        "supermarkets on weekends should come with safety training",
                        "i survived saturday shopping barely"
                    )
                );

                break;
        }
    }

    void SetupStory(
        string story,
        string context
    )
    {
        currentStory = story;

        // keeps story continuity active between replies
        chat.storyActive = true;

        chat.activeStory = story;

        chat.activeStoryStep = 0;

        chat.currentConversationContext =
            context;
    }

    public override string HandleInput(
        string input
    )
    {
        string lower =
            input.ToLower();

        // conversational prompting keeps the story flowing naturally
        // similar to old eliza conversational continuation tricks
        bool askingForMore =
            chat.ContainsAny(
                lower,
                "what happened",
                "then",
                "why",
                "go on",
                "seriously",
                "what did she say",
                "what did you do",
                "and then",
                "continue"
            );

        if (askingForMore)
        {
            chat.contextStep++;

            if (currentStory == "tesco")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "this random bloke trapped me talking about lawnmowers for forty minutes",
                        "i made ONE mistake and nodded at this guy near gardening supplies",
                        "he started explaining grass maintenance like it was ancient forbidden knowledge"
                    );
                }

                if (chat.contextStep == 2)
                {
                    return chat.RandomChoice(
                        "your aunt fully abandoned me aisle by aisle",
                        "i could literally SEE her escaping while i was trapped there",
                        "she looked at me struggling and just disappeared"
                    );
                }

                if (chat.contextStep == 3)
                {
                    return chat.RandomChoice(
                        "and the worst part is i still don't even own a lawnmower",
                        "i don't even remember how the conversation started",
                        "somehow we ended up discussing fence regulations"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    return EndStory(
                        "whole thing changed me psychologically",
                        "i still don't trust garden centres after that",
                        "i've never escaped a conversation slower in my life"
                    );
                }
            }

            if (currentStory == "coffee")
            {
                if (chat.contextStep == 1)
                {
                    return chat.RandomChoice(
                        "i accidentally used about four times too much coffee",
                        "one sip nearly restarted my nervous system",
                        "my heartbeat achieved a new speed"
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
                        "i sat down afterwards and could still hear colours",
                        "i was one bad decision away from running a marathon",
                        "i transcended normal human caffeine limits"
                    );
                }

                if (chat.contextStep >= 4)
                {
                    return EndStory(
                        "coffee should probably be regulated",
                        "never trusting myself with caffeine again",
                        "i nearly ascended spiritually"
                    );
                }
            }
        }

        storyStep++;

        if (storyStep == 1)
        {
            return chat.RandomChoice(
                "you listening or have i lost you already?",
                "i still can't believe it happened",
                "weird things only happen when i'm outside",
                "i'm still processing the experience"
            );
        }

        if (storyStep == 2)
        {
            return chat.RandomChoice(
                "wait no that's not even the worst part",
                "hang on i forgot the stupidest part",
                "nah wait it somehow got worse"
            );
        }

        if (storyStep >= 3)
        {
            return EndStory(
                "anyway enough about my disasters",
                "right i've traumatised you enough with that story",
                "moving on before i relive the experience emotionally"
            );
        }

        return chat.GetFollowUpTopic();
    }

    string EndStory(
        params string[] endings
    )
    {
        chat.storyActive = false;

        chat.ClearContext();

        chat.ChangeState(
            new CasualState(chat)
        );

        return chat.RandomChoice(
            endings
        )
        + ". "
        + chat.GetFollowUpTopic();
    }
}