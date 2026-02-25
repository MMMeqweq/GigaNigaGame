using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace GigaNigaGame.Folders.Classes
{
    internal static class AnimationAndMoves
    {
        public static void BPress(StackPile owner, Cards cardView)
        {
            if (cardView.TAG.Text == "pile")
            {
                MoveCards(owner, cardView);
            }
            else if (cardView.TAG.Text == "shop")
            {

            }
        }


        private static void ShopPress(Cards cardview)
        {

        }

        private static void MoveCards(StackPile owner, Cards cardView)
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
                bool DifPile = (Lists.StackPiles[i] != owner);
                bool NumPlusOne = (ChosenCard.Num + 1 == TargetCard.Num);
                bool DifColor = (ChosenCard.CardColor != TargetCard.CardColor);
                bool NotEmpty = (TargetPile.Cards.Count > 0);
                bool Run = (DifPile && NumPlusOne && DifColor && NotEmpty);
                if (!NotEmpty && DifColor && NotEmpty && NumPlusOne)
                    if (TargetCard.Num == 13)
                        Run = true;
                if (Run)
                {
                    if (owner.Cards.Count - 1 == cardIndex)
                    {
                        owner.RemoveCard(ChosenCard);
                        TargetPile.AddCard(ChosenCard);
                    }
                    else if (owner.Cards.Count - 1 > cardIndex)
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
                        mainWindow.RenderAll();
                    }
                    return;
                }
            }
        }
    }
}