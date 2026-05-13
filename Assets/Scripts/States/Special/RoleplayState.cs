using UnityEngine;

public class RoleplayState : ChatState
{
    int roleplayExchanges = 0;

    public RoleplayState(ChatManager manager) : base(manager)
    {
    }

    public override string HandleInput(string input)
    {
        roleplayExchanges++;

        string lower =
            input.ToLower();

        if (
            lower.Contains("saving the world")
            || lower.Contains("fight crime")
            || lower.Contains("hero")
            || lower.Contains("superhero")
            || lower.Contains("super hero")
        )
        {
            if (
                chat.lifeStage
                == "school"
            )
            {
                return chat.RandomChoice(
                    "ah fighting crime again are we? use your powers to delete homework honestly",
                    "can your superhero abilities erase exams too?",
                    "right captain universe. make school disappear next honestly",
                    "if you've got magic powers at least use them to fix your sleep schedule",
                    "you saving the world after finishing homework or before?"
                );
            }

            if (
                chat.lifeStage
                == "youngAdult"
                || chat.lifeStage
                == "uni"
            )
            {
                return chat.RandomChoice(
                    "ah fighting crime again are we? use your powers to delete assignments honestly",
                    "if you've got superpowers at least erase coursework for everyone",
                    "can your abilities defeat university stress too?",
                    "you out saving society while surviving deadlines somehow",
                    "right superhero. go destroy your assignments next"
                );
            }

            if (
                chat.lifeStage
                == "adult"
            )
            {
                return chat.RandomChoice(
                    "ah fighting crime again are we? make your coworkers vanish next honestly",
                    "if you've got powers can you erase meetings permanently?",
                    "adult superheroes probably spend half their powers avoiding emails honestly",
                    "save society and then fix your work schedule next",
                    "you using magic powers to survive adulthood too?"
                );
            }

            return chat.RandomChoice(
                "ah fighting crime again are we?",
                "finally somebody's protecting society honestly",
                "right superhero explain your powers then",
                "can your abilities fix my wifi at least?",
                "this sounds suspiciously made up honestly",
                "you absolutely sound like somebody with a secret identity"
            );
        }

        if (
            lower.Contains("villain")
            || lower.Contains("evil")
            || lower.Contains("destroy")
            || lower.Contains("take over")
        )
        {
            return chat.RandomChoice(
                "i knew you'd become the villain eventually honestly",
                "absolutely not we're not becoming supervillains",
                "you'd 100% monologue before doing anything evil",
                "this is how every superhero film starts honestly",
                "you sound like somebody dramatically standing on a rooftop in the rain"
            );
        }

        if (
            lower.Contains("power")
            || lower.Contains("super")
            || lower.Contains("abilities")
        )
        {
            return chat.RandomChoice(
                "okay captain universe",
                "what exactly ARE your powers then?",
                "this sounds suspiciously fake honestly",
                "you absolutely made this up in your bedroom",
                "can you at least fly or something useful?"
            );
        }

        if (
            lower.Contains("dragon")
            || lower.Contains("wizard")
            || lower.Contains("magic")
            || lower.Contains("spell")
        )
        {
            if (
                chat.lifeStage
                == "school"
            )
            {
                return chat.RandomChoice(
                    "if you learn magic make homework disappear first honestly",
                    "wizard school honestly sounds less stressful than real school",
                    "you'd absolutely use magic to avoid revision"
                );
            }

            if (
                chat.lifeStage
                == "youngAdult"
                || chat.lifeStage
                == "uni"
            )
            {
                return chat.RandomChoice(
                    "magic would genuinely help with assignments honestly",
                    "you'd absolutely use spells to finish coursework",
                    "wizard powers would carry university students honestly"
                );
            }

            if (
                chat.lifeStage
                == "adult"
            )
            {
                return chat.RandomChoice(
                    "adult wizardry would just be deleting emails honestly",
                    "you'd use magic entirely for avoiding responsibilities",
                    "if magic existed somebody would still schedule meetings somehow"
                );
            }

            return chat.RandomChoice(
                "you're absolutely the type to become a wizard honestly",
                "magic sounds less stressful than real life honestly",
                "if you summon a dragon i'm leaving immediately",
                "this conversation escalated unbelievably fast",
                "you definitely sound like somebody with forbidden powers"
            );
        }

        if (
            lower.Contains("batman")
            || lower.Contains("spiderman")
            || lower.Contains("marvel")
            || lower.Contains("dc")
            || lower.Contains("avenger")
        )
        {
            return chat.RandomChoice(
                "you definitely argue over superhero films online honestly",
                "batman needs therapy more than gadgets honestly",
                "spiderman genuinely cannot catch a break",
                "marvel films are just emotional damage and explosions honestly",
                "you absolutely sound like you'd try becoming an avenger"
            );
        }

        if (roleplayExchanges >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "anyway back to reality for a minute honestly",
                "right enough superhero lore",
                "you'd definitely abuse those powers honestly",
                "i still think you'd become a menace with abilities",
                "you absolutely cannot be trusted with magic powers honestly"
            );
        }

        return chat.brain.GetRoleplayReply();
    }
}