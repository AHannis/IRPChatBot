using UnityEngine;

public class RoleplayState : ChatState
{
    int roleplayExchanges = 0;

    public RoleplayState(ChatManager manager)
        : base(manager)
    {
    }

    public override string HandleInput(
        string input
    )
    {
        roleplayExchanges++;

        string lower =
            input.ToLower();

        // exaggerated roleplay responses help reinforce personality consistency
        if (
            chat.ContainsAny(
                lower,
                "hero",
                "superhero",
                "saving the world",
                "fight crime"
            )
        )
        {
            if (
                chat.lifeStage == "school"
            )
            {
                return chat.RandomChoice(
                    "use your powers to delete homework first",
                    "can your superhero abilities erase exams too?",
                    "fight crime after revision yeah?",
                    "if you've got powers fix your sleep schedule next"
                );
            }

            if (
                chat.lifeStage == "youngAdult"
                || chat.lifeStage == "uni"
            )
            {
                return chat.RandomChoice(
                    "use your powers to destroy assignments",
                    "university students with superpowers would become unstoppable",
                    "fight crime after coursework",
                    "save society and then survive deadlines somehow"
                );
            }

            if (
                chat.lifeStage == "adult"
            )
            {
                return chat.RandomChoice(
                    "adult superheroes probably spend half their powers avoiding emails",
                    "use your powers to erase meetings permanently",
                    "even superheroes probably hate office jobs"
                );
            }

            return chat.RandomChoice(
                "right superhero explain your powers then",
                "finally somebody's protecting society",
                "can your abilities fix my wifi at least?",
                "you absolutely sound like somebody with a secret identity"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "villain",
                "evil",
                "destroy",
                "take over"
            )
        )
        {
            return chat.RandomChoice(
                "i knew you'd become the villain eventually",
                "you'd absolutely monologue before doing anything evil",
                "this is how every superhero film starts",
                "you sound like somebody dramatically standing on a rooftop in the rain"
            );
        }

        if (
            chat.ContainsAny(
                lower,
                "wizard",
                "magic",
                "dragon",
                "spell"
            )
        )
        {
            return chat.RandomChoice(
                "if you summon a dragon i'm leaving immediately",
                "magic sounds less stressful than real life",
                "you definitely sound like somebody with forbidden powers",
                "wizardry would solve about 80% of life problems"
            );
        }

        if (roleplayExchanges >= 3)
        {
            chat.ChangeState(
                new CasualState(chat)
            );

            return chat.RandomChoice(
                "right enough superhero lore",
                "back to reality for a minute",
                "you absolutely cannot be trusted with powers",
                "you'd become a menace with magic abilities"
            );
        }

        return chat.brain.GetRoleplayReply();
    }
}