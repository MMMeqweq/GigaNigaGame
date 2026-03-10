using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GigaNigaGame.Folders.Classes
{
    internal static class AnimationAndMoves
    {
        public static void BPress(StackPile owner, CardView cardView)
        {
             string str = $"Card {cardView.Model.Num} with the color {cardView.Model.CardColor} was pressed part of the set {cardView.TAG.Text}.";
            if (cardView.TAG.Text == "pile")
            {
                MoveCards(owner, cardView);
            }
            else if (cardView.TAG.Text == "shop")
            {
                ShopPress(cardView);
            }
            else if (cardView.TAG.Text == "faceupshop")
            {
               FaceUpShopPress(cardView, owner); 
            }
        }


        private static void FaceUpShopPress(CardView cardView, StackPile owner)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            var ChosenCard = cardView.Model;
            for (int i = 0; i < Lists.StackPiles.Count; i++)
            {
                StackPile TargetPile = Lists.StackPiles[i];
                var TargetCard = Lists.StackPiles[i].Cards[Lists.StackPiles[i].Cards.Count - 1];
                string str = $"StackPile {i} has {Lists.StackPiles[i].Cards.Count} cards. Top card is {TargetCard.Num} with the color {TargetCard.CardColor}.";
                bool NumPlusOne = (ChosenCard.Num + 1 == TargetCard.Num);
                bool DifColor = (ChosenCard.CardColor != TargetCard.CardColor);
                bool NotEmpty = (TargetPile.Cards.Count > 0);
                bool Run = (NumPlusOne && DifColor && NotEmpty);
                if (!NotEmpty)
                    if (ChosenCard.Num == 13)
                        Run = true;
                if (Run)
                {
                    cardView.TAG.Text = Set.pile.ToString();
                    Lists.FaceUpCards.Remove(cardView);
                    mainWindow.FaceUpCards.Children.Remove(cardView);
                    ChosenCard.FaceUp = true;
                    TargetPile.AddCard(ChosenCard);
                    mainWindow.RenderAll();
                    mainWindow.TempTest.Text = Lists.FaceOffCards.Count.ToString();
                    return;
                }
            }
        }


        private static void ShopPress(CardView cardview)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            mainWindow.FaceOffCards.Children.Remove(cardview);
            mainWindow.FaceUpCards.Children.Add(cardview);
            cardview.TAG.Text = Set.faceupshop.ToString();
            Lists.FaceOffCards.Remove(cardview);
            Lists.FaceUpCards.Add(cardview);
            if (Lists.FaceOffCards.Count > 0)
                Lists.FaceOffCards[Lists.FaceOffCards.Count - 1].Cover.Visibility = Visibility.Collapsed;
            mainWindow.TempTest.Text = Lists.FaceOffCards.Count.ToString();
            if (Lists.FaceOffCards.Count == 0)
            {
                for (int i = Lists.FaceUpCards.Count; i > 0; i--)
                {
                    CardInfo.Shuffle(Lists.FaceUpCards);
                    var card = Lists.FaceUpCards[0];
                    Lists.FaceUpCards.Remove(card);
                    card.Cover.Visibility = Visibility.Visible;
                    Lists.FaceOffCards.Add(card);
                    card.TAG.Text = Set.shop.ToString();
                    mainWindow.FaceUpCards.Children.Remove(card);
                    mainWindow.FaceOffCards.Children.Add(card);
                }
                Lists.FaceOffCards[Lists.FaceOffCards.Count - 1].Cover.Visibility = Visibility.Collapsed;
            }

        }

        private static void MoveCards(StackPile owner, CardView cardView)
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
                if (!NotEmpty)
                    if (ChosenCard.Num == 13)
                        Run = true;
                if (Run)
                {
                    cardView.TAG.Text = Set.pile.ToString();
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
                    mainWindow.TempTest.Text = Lists.FaceOffCards.Count.ToString();
                    return;
                }
            }
        }
    }
}