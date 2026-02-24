using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace GigaNigaGame.Folders.Classes
{
    internal static class AnimationAndMoves
    {
        public static void BPress(StackPile owner, Cards cardView)
        {
            
            int pileIndex = Lists.StackPiles.IndexOf(owner);
            int cardIndex = owner.Cards.IndexOf(cardView.Model);
            var ChosenCard = owner.Cards[cardIndex];
            for (int i = 0; i < Lists.StackPiles.Count; i++)
            {
                StackPile TargetPile = Lists.StackPiles[i];
                var TargetCard = Lists.StackPiles[i].Cards[Lists.StackPiles[i].Cards.Count - 1];
                string str = $"StackPile {i} has {Lists.StackPiles[i].Cards.Count} cards. Top card is {TargetCard.Num} with the color {TargetCard.CardColor}.";
                string str2 = $"StackPile {pileIndex} has {owner.Cards.Count} cards. Top card is {ChosenCard.Num} with the color {ChosenCard.CardColor}.";
                bool Run = (Lists.StackPiles[i] != owner && ChosenCard.Num + 1 == TargetCard.Num && ChosenCard.CardColor != TargetCard.CardColor);
                if (Run)
                {
                    if (owner.Cards.Count - 1 == cardIndex)
                    {
                        owner.RemoveCard(ChosenCard);
                        TargetPile.AddCard(ChosenCard);
                    }
                    else if (owner.Cards.Count -1 > cardIndex)
                    {
                        owner.MoveCards(cardIndex);
                        TargetPile.AddCards(Lists.MovingCards);
                    }
                    var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                    mainWindow.RenderAll();
                    if (owner.Cards.Count > 0)
                    {
                        owner.Cards[owner.Cards.Count - 1].FaceUp = true;
                        owner.CardViews[owner.Cards.Count - 1].Cover.Visibility = Visibility.Collapsed;
                    }
                    return;
                }
            }
        }
    }
}