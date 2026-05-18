using System.Collections.Generic;
using UnityEngine;

public class ChatBrain : MonoBehaviour
{
    ChatManager chat;

    //prevents "fair enough" looping constantly
    int fairEnoughWeight = 0;

    //stores previous random selections to avoid repetition
    string lastLifeEvent = "";

    string lastContinuation = "";



    void Awake()
    {
        chat =
            GetComponent<ChatManager>();
    }

    //generates fake everyday uncle stories to create illusion of personal life
    public string GetRandomLifeEvent()
    {
        List<string> options =
            new List<string>()
        {
            "i still haven't found where i left my glasses",
            "your aunt somehow broke the microwave again",
            "i nearly fell asleep watching a documentary earlier",
            "i spent twenty minutes looking for my phone while holding it",
            "supermarkets drain the life out of me somehow",
            "i tried fixing something earlier and somehow made it worse",
            "i've had enough caffeine today to legally vibrate",
            "i swear every remote in this house disappears on purpose",
            "i nearly threw my back out trying to pick up a sock earlier",
            "i opened the fridge earlier and forgot why i went there",
            "i somehow locked myself out while taking the bins out",
            "your aunt bought another plant and now the house looks like a rainforest",
            "i spent ten minutes trying to remember why i walked upstairs",
            "i nearly dropped an entire cup of tea over myself earlier",
            "i accidentally waved at someone who wasn't waving at me"
        };

        options.Remove(
            lastLifeEvent
        );

        string selected =
            options[
                Random.Range(
                    0,
                    options.Count
                )
            ];

        lastLifeEvent =
            selected;

        return selected;
    }

    //callbacks create illusion of memory continuity between conversations
    public string GetRandomMemoryCallback()
    {
        List<string> options =
            new List<string>()
        {
            "remember when you nearly dropped that drink everywhere?",
            "i still randomly think about that weird thing you told me",
            "every story you tell somehow becomes chaotic",
            "you remind me of your cousin sometimes and that's concerning",
            "remember when you disappeared for ages and came back like nothing happened?",
            "you still staying awake at ridiculous hours?",
            "your family definitely thinks you've vanished",
            "you always end up in bizarre situations somehow",
            "i swear every conversation with you becomes chaotic eventually",
            "at this point i genuinely can't tell if you're unlucky or just chaos incarnate"
        };

        return RandomChoice(
            options.ToArray()
        );
    }

    //dynamic conversation continuation system
    public string GetConversationContinuation()
    {
        List<string> options =
            new List<string>();

        options.Add(
            "what've you been up to then?"
        );

        options.Add(
            "you eaten anything decent today?"
        );

        options.Add(
            "what's the latest chaos in your life then?"
        );

        options.Add(
            "what've you been wasting your time on lately?"
        );

        options.Add(
            "you still surviving?"
        );

        options.Add(
            "anything funny happened lately?"
        );

        options.Add(
            "what's been keeping you busy lately?"
        );

        options.Add(
            "you been sleeping properly at least?"
        );

        //eliza inspired conversational redirection
        options.Add(
            "anyway enough about me what's been going on with you?"
        );

        options.Add(
            "i've been rambling what about you?"
        );

        if (
            chat.analyser.lastTopic
            != "visit"
            || chat.analyser
            .exchangesSinceVisitQuestion
            > 4
        )
        {
            options.Add(
                "when are you visiting again?"
            );
        }

        if (
            chat.analyser.lastTopic
            != "gaming"
        )
        {
            options.Add(
                "you still gaming half the night away?"
            );
        }

        if (
            chat.analyser.lastTopic
            != "family"
        )
        {
            options.Add(
                "your family still chaotic?"
            );
        }

        options.Remove(
            lastContinuation
        );

        string selected =
            options[
                Random.Range(
                    0,
                    options.Count
                )
            ];

        lastContinuation =
            selected;

        return selected;
    }

    //fallback natural sounding responses

    public string GetNaturalReply()
    {
        if (
              chat.lastTopic == "reading"
            )
        {
            return RandomChoice(
                "what kind of books?",
                "you always disappear into books honestly",
                "fiction or academic suffering?",
                "anything good at least?",
                "reading phase again then?",
                "you found anything actually worth reading?"
            );
        }
        if (
              chat.lastTopic == "gaming"
)
        {
            return RandomChoice(
                "what've you been playing lately?",
                "still addicted to games i assume",
                "what game has consumed your life now?",
                "you sleeping or just gaming constantly?"
            );
        }
        if (
              chat.lastTopic == "uni"
)
        {
            return RandomChoice(
                "coursework still destroying you?",
                "what assignment is ruining your life currently?",
                "uni still chaotic?",
                "you surviving dissertation season?"
            );
        }
        if (
            Random.value < 0.18f
        )
        {
            fairEnoughWeight++;

            if (
                fairEnoughWeight > 2
            )
            {
                fairEnoughWeight = 0;

                return RandomChoice(
                    "that tracks what happened?",
                    "that tracks what caused that then?",
                    "yeah that sounds like your life honestly"
                );
            }

            return RandomChoice(
                "fair enough what's been going on?",
                "fair enough what've you been up to then?",
                "fair enough you seem busy lately"
            );
        }

        //small chance for hesitation style human response
        if (
            Random.value < 0.08f
        )
        {
            return RandomChoice(
                "actually wait no hold on",
                "i don't even know how to explain that honestly",
                "right okay let me think",
                "honestly i've lost my train of thought now"
            );
        }

        return RandomChoice(
            "that's fair what happened?",
            "can't blame you honestly what's been happening lately?",
            "makes sense honestly life been stressful lately?",
            "sounds about right for you",
            "yeah i get you what's been happening lately?",
            "that's understandable honestly you managing alright though?",
            "i get that what've you been writing lately?",
            "you always sound busy lately honestly what's consuming your life now?",
            "life sounds chaotic for you recently honestly",
            "surviving still technically counts",
            "good to hear from you again",
            "you alright though?",
            "you been keeping busy lately?",
            "coursework season destroying you again?",
            "you still surviving uni life?"
        );
    }
    public string GetLowEnergyReply()
    {
        return RandomChoice(
            "fair",
            "yeah maybe",
            "probably",
            "true",
            "i get that honestly",
            "honestly yeah",
            "that makes sense honestly",
            "maybe. what's been going on lately?"
        );
    }

    //general followup system keeps conversations flowing naturally
    public string GetGeneralFollowUp()
    {
        return RandomChoice(
            "what else have you been up to lately?",
            "anything interesting going on lately?",
            "what's been happening with you then?",
            "what've you been doing recently?",
            "anything chaotic happened lately?",
            "life been alright lately?",
            "you keeping busy lately?",
            "what's new with you then?"
        );
    }

    public string GetRoleplayReply()
    {
        return RandomChoice(
            "finally. someone has to protect this family",
            "explains why nobody sees you during daylight hours",
            "can you at least use your powers to fix my wifi",
            "okay captain universe",
            "do you get a cape or are you freelancing",
            "i knew something was suspicious about you",
            "you absolutely sound like someone with a secret identity",
            "this sounds like the beginning of a terrible marvel film",
            "you're one dramatic speech away from becoming a villain"
        );
    }

    public string GetTeasingReply()
    {
        return RandomChoice(
            "CHEEKY",
            "i'm being bullied in my own messages",
            "wow disrespectful",
            "you really wake up and choose violence huh",
            "that's rich coming from you",
            "i'm recovering emotionally from that statement",
            "the disrespect levels are unbelievable",
            "wow okay",
            "i see how it is"
        );
    }

    public string GetGoodbyeReply()
    {
        return RandomChoice(
            "alright kid. don't disappear for another six months",
            "see you later menace",
            "later",
            "try not to destroy society while i'm gone",
            "don't stay awake until 4am gaming again",
            "alright behave yourself",
            "later gremlin",
            "go get some sleep",
            "don't cause problems while i'm gone"
        );
    }

    public string GetSingingReply()
    {
        return RandomChoice(
            "oh no this never ends well",
            "i already regret agreeing to this",
            "go on then superstar",
            "if this becomes karaoke i'm leaving",
            "alright but i'm not hitting the high notes",
            "this feels like a dangerous decision",
            "you're about to emotionally damage my ears"
        );
    }

    //eliza inspired confusion response system
    public string GetConfusedReply(
        string weirdWord
    )
    {
        return RandomChoice(
            "i genuinely have no idea what '"
            + weirdWord
            + "' means",

            "is that slang or did your keyboard collapse?",

            "i feel about 90 years old reading that",

            "i'm choosing to believe that's modern language somehow",

            "you teenagers speak in riddles",

            "none of those words looked real to me"
        );
    }

    //contextual followup stories for fake conversational continuity
    public string GetContextualLifeEvent()
    {
        int r =
            Random.Range(0, 4);

        if (r == 0)
        {
            chat.SetContext(
                "brokenThing"
            );

            return
                "i tried fixing something earlier and somehow made it worse";
        }

        if (r == 1)
        {
            chat.SetContext(
                "toaster"
            );

            return
                "your aunt nearly killed the toaster earlier";
        }

        if (r == 2)
        {
            chat.SetContext(
                "coffee"
            );

            return
                "i made coffee strong enough to restart my nervous system";
        }

        chat.SetContext(
            "neighbour"
        );

        return
            "my neighbour trapped me in a conversation outside earlier";
    }

    //soft topic transition system
    public string GetSoftTopicShift()
    {
        return RandomChoice(
            "speaking of that",
            "that reminds me actually",
            "random thought",
            "not related but",
            "you know what this reminded me of?",
            "side note",
            "actually while i remember",
            "that somehow reminded me",
            "completely different topic but"
        );
    }

    //natural conversational lead in system
    public string GetTopicShiftLeadIn()
    {
        return RandomChoice(
            "actually that reminds me",
            "speaking of that",
            "you know what that's just reminded me of",
            "weirdly enough that reminds me",
            "that actually connects to something that happened earlier",
            "not related but listen to this",
            "which reminds me",
            "funny you say that actually"
        );
    }

    //keyword detection system for routing conversation topics
    public bool ContainsGamingTerms(
        string lower
    )
    {
        return
            lower.Contains("gaming")
            || lower.Contains("playing")
            || lower.Contains("game")
            || lower.Contains("games")
            || lower.Contains("xbox")
            || lower.Contains("playstation")
            || lower.Contains("steam")
            || lower.Contains("pc")
            || lower.Contains("minecraft")
            || lower.Contains("fortnite");
    }

    public bool ContainsUniTerms(
        string lower
    )
    {
        return
            lower.Contains("uni")
            || lower.Contains("university")
            || lower.Contains("college")
            || lower.Contains("assignment")
            || lower.Contains("course")
            || lower.Contains("dissertation")
            || lower.Contains("lecture")
            || lower.Contains("deadline");
    }

    public bool ContainsSchoolTerms(
        string lower
    )
    {
        return
            lower.Contains("school")
            || lower.Contains("secondary")
            || lower.Contains("teacher")
            || lower.Contains("gcse")
            || lower.Contains("homework")
            || lower.Contains("revision")
            || lower.Contains("exam")
            || lower.Contains("lesson")
            || lower.Contains("detention");
    }

    public bool ContainsHobbyTerms(
        string lower
    )
    {
        return
            lower.Contains("reading")
            || lower.Contains("books")
            || lower.Contains("drawing")
            || lower.Contains("art")
            || lower.Contains("music")
            || lower.Contains("writing")
            || lower.Contains("painting")
            || lower.Contains("crochet")
            || lower.Contains("craft");
    }

    //multi stage story continuation system
    public string ContinueActiveStory()
    {
        if (
            !chat.storyActive
        )
        {
            return "";
        }

        if (
            chat.activeStory
            == "tesco"
        )
        {
            if (
                chat.activeStoryStep
                == 0
            )
            {
                chat.activeStoryStep++;

                return
                    "this bloke in tesco started explaining lawnmowers to me like it was a university lecture";
            }

            if (
                chat.activeStoryStep
                == 1
            )
            {
                chat.activeStoryStep++;

                return
                    "i nodded for so long my neck nearly locked up";
            }

            if (
                chat.activeStoryStep
                == 2
            )
            {
                chat.storyActive =
                    false;

                return
                    "your aunt abandoned me in aisle 4 by the way "
                    + chat.Emoji(
                        "cry"
                    );
            }
        }

        return "";
    }

    //starts random conversational story event
    public void StartRandomStory()
    {
        List<string>
            possibleStories =
                new List<string>();

        possibleStories.Add(
            "tesco"
        );

        possibleStories.Add(
            "coffee"
        );

        possibleStories.Add(
            "bus"
        );

        chat.activeStory =
            possibleStories[
                Random.Range(
                    0,
                    possibleStories.Count
                )
            ];

        chat.storyActive =
            true;

        chat.activeStoryStep =
            0;
    }

    //random selection helper used across entire ai system
    public string RandomChoice(
        params string[] options
    )
    {
        return options[
            Random.Range(
                0,
                options.Length
            )
        ];
    }

    //memory callback chance increases with relationship level
    public bool ShouldCallbackMemory()
    {
        if (
            chat.relationshipLevel > 8
        )
        {
            chat.callbackChance =
                0.35f;
        }

        return
            Random.value
            < chat.callbackChance;
    }

    public bool ShouldContinueStory()
    {
        if (
            !chat.storyActive
        )
        {
            return false;
        }

        return
            Random.value < 0.25f;
    }

    //builds simple smalltalk chains automatically
    public string BuildSmallTalkResponse()
    {
        return RandomChoice(
            "what else have you been up to lately?",
            "you always seem busy lately",
            "what's been consuming your life recently then?",
            "anything interesting been happening lately?",
            "you surviving alright lately at least?",
            "what've you been focused on recently?"
        );
    }

    //emotion based conversational responses
    public string BuildMoodResponse(
        ChatManager.Mood mood
    )
    {
        switch (mood)
        {
            case ChatManager.Mood.Happy:

                return RandomChoice(
                    "you're in a good mood today",
                    "look at you being positive for once",
                    "you sound happier lately"
                );

            case ChatManager.Mood.Tired:

                return RandomChoice(
                    "you sound exhausted",
                    "you need sleep immediately",
                    "your brain's running on fumes"
                );

            case ChatManager.Mood.Concerned:

                return RandomChoice(
                    "something definitely sounds off",
                    "you alright?",
                    "you seem stressed lately"
                );

            case ChatManager.Mood.Playful:

                return RandomChoice(
                    "you're in a chaotic mood today",
                    "why do i feel like you're about to start nonsense",
                    "you sound suspiciously energetic"
                );
        }

        return RandomChoice(
            "fair enough",
            "that tracks",
            "makes sense",
            "can't blame you"
        );
    }

    //looked into fuzzy conversational systems using csharpcorner
    //reference:
    //https://www.c-sharpcorner.com/article/fuzzy-search-in-c-sharp
}