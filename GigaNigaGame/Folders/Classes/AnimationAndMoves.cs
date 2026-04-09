using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            bool Run = false;
            for (int i = 0; i < Lists.StackPiles.Count; i++)
            {
                StackPile TargetPile = Lists.StackPiles[i];
                bool NotEmpty = (TargetPile.Cards.Count > 0);
                if (!NotEmpty)
                {
                    if (ChosenCard.Num == 13)
                        Run = true;
                }
                else if (NotEmpty)
                {
                    var TargetCard = Lists.StackPiles[i].Cards[Lists.StackPiles[i].Cards.Count - 1];
                    bool DifPile = (Lists.StackPiles[i] != owner);
                    bool NumPlusOne = (ChosenCard.Num + 1 == TargetCard.Num);
                    bool DifColor = (ChosenCard.CardColor != TargetCard.CardColor);
                    Run = (DifPile && NumPlusOne && DifColor && NotEmpty);
                }
                if (Run)
                {
                    cardView.TAG.Text = Set.pile.ToString();
                    cardView.Model.set = Set.pile;
                    Lists.FaceUpCards.Remove(cardView);
                    mainWindow.FaceUpCards.Children.Remove(cardView);
                    ChosenCard.FaceUp = true;
                    TargetPile.AddCard(ChosenCard);
                    mainWindow.RenderAll();
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
            cardview.Model.set = Set.faceupshop;
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
                    card.Model.set = Set.shop;
                    mainWindow.FaceUpCards.Children.Remove(card);
                    mainWindow.FaceOffCards.Children.Add(card);
                }
                Lists.FaceOffCards[Lists.FaceOffCards.Count - 1].Cover.Visibility = Visibility.Collapsed;
            }

        }

        private static async Task MoveCards(StackPile owner, CardView cardView)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            int pileIndex = Lists.StackPiles.IndexOf(owner);
            int cardIndex = owner.Cards.IndexOf(cardView.Model);
            var ChosenCard = owner.Cards[cardIndex];
            bool Run = false;
            for (int i = 0; i < Lists.StackPiles.Count; i++)
            {
                StackPile TargetPile = Lists.StackPiles[i];
                bool NotEmpty = (TargetPile.Cards.Count > 0);
                if (!NotEmpty)
                {
                    if (ChosenCard.Num == 13)
                    {
                        XAndY point = new XAndY(i*(800/7), 140);
                        AnimateCard(cardView, cardView.Location, point);
                        Run = true;
                    }
                }
                else if (NotEmpty)
                {
                    var TargetCard = Lists.StackPiles[i].Cards[Lists.StackPiles[i].Cards.Count - 1];
                    string str = $"StackPile {i} has {Lists.StackPiles[i].Cards.Count} cards. Top card is {TargetCard.Num} with the color {TargetCard.CardColor}.";
                    string str2 = $"StackPile {pileIndex} has {owner.Cards.Count} cards. Top card is {ChosenCard.Num} with the color {ChosenCard.CardColor}.";
                    bool DifPile = (Lists.StackPiles[i] != owner);
                    bool NumPlusOne = (ChosenCard.Num + 1 == TargetCard.Num);
                    bool DifColor = (ChosenCard.CardColor != TargetCard.CardColor);
                    Run = (DifPile && NumPlusOne && DifColor && NotEmpty);
                }
                mainWindow.TempTest.Text = cardView.Location.ToString();
                if (Run)
                {
                    XAndY TargetPoint = new XAndY(TargetPile.Cards[TargetPile.Cards.Count - 1].point);
                    TargetPoint.SetToMove();
                    cardView.TAG.Text = Set.pile.ToString();
                    if (owner.Cards.Count - 1 == cardIndex)
                    {
                        AnimateCard(cardView, ChosenCard.point, TargetPoint);
                        await Task.Delay(300);
                        owner.RemoveCard(ChosenCard);
                        TargetPile.AddCard(ChosenCard);
                    }
                    else if (owner.Cards.Count - 1 > cardIndex)
                    {
                        owner.MoveCards(cardIndex);
                        AnimateSeveralCards(Lists.MovingCardsView, ChosenCard.point, TargetPoint);
                        await Task.Delay(300);
                        TargetPile.AddCards(Lists.MovingCards);
                    }
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


        static void AnimateCard(CardView cardView, XAndY from, XAndY to)
        {
            double dx = to.GetX() - from.GetX();
            double dy = to.GetY() - from.GetY();
            cardView.RenderTransform = new TranslateTransform();
            const double duration = 300; 
            Storyboard SB = new Storyboard();

            DoubleAnimation AnimX = new DoubleAnimation
            {
                To = dx,
                Duration = TimeSpan.FromMilliseconds(duration)
            };

            DoubleAnimation AnimY = new DoubleAnimation
            {
                To = dy,
                Duration = TimeSpan.FromMilliseconds(duration)
            };

            Storyboard.SetTarget(AnimX, cardView);
            Storyboard.SetTarget(AnimY, cardView);

            Storyboard.SetTargetProperty(AnimX,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            Storyboard.SetTargetProperty(AnimY,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            SB.Children.Add(AnimX);
            SB.Children.Add(AnimY);

            SB.Begin();
        }
        

        static void AnimateSeveralCards(List<CardView> cards, XAndY from, XAndY to)
        {
            double dx = to.GetX() - from.GetX();
            double dy = to.GetY() - from.GetY();
            for (int i = 0; i < cards.Count; i++)
            {
                Storyboard SB = new Storyboard();

                CardView card = cards[i];

                card.RenderTransform = new TranslateTransform();

                DoubleAnimation AnimX = new DoubleAnimation
                {
                    To = dx,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                DoubleAnimation AnimY = new DoubleAnimation
                {
                    To = dy,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                Storyboard.SetTarget(AnimX, card);
                Storyboard.SetTarget(AnimY, card);

                Storyboard.SetTargetProperty(AnimX,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

                Storyboard.SetTargetProperty(AnimY,
                    new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                SB.Children.Add(AnimX);
                SB.Children.Add(AnimY);

                SB.Begin();

                //dy += 40;
            }
        }
    }
    
}