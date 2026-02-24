using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GigaNigaGame
{
    /// <summary>
    /// Interaction logic for StackPile.xaml
    /// </summary>
    public partial class StackPile : UserControl
    {
        public List<CardInfo> Cards = new List<CardInfo>();
        public List<Cards> CardViews = new List<Cards>();
        public double YOffset = 30;

        public event Action<StackPile, List<CardInfo>, Point> MoveRequested;

        public StackPile()
        {
            InitializeComponent();
        }

        public void Render()
        {
            PileStack.Children.Clear();

            double y = 0;
            for (int i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                var view = new Cards(card)
                {
                    Width = MainWindow.CardWidth,
                    Height = MainWindow.CardHeight,
                    // assign owner so the view can reference its pile
                    Owner = this
                };

                // ensure cover state follows model
                view.Cover.Visibility = card.FaceUp ? Visibility.Collapsed : Visibility.Visible;

                Canvas.SetLeft(view, 0);
                Canvas.SetTop(view, y);

                view.Tag = i;
                view.MouseLeftButtonDown += Card_MouseDown;

                PileStack.Children.Add(view);
                y += YOffset;
            }
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Cards View)) return;
            int index = (int)View.Tag;

            var moving = Cards.Skip(index).ToList();

            if (!moving[0].FaceUp)
                return;

            // get click position relative to MainWindow so the handler can choose destination pile
            Point pos = e.GetPosition(Application.Current.MainWindow);

            MoveRequested?.Invoke(this, moving, pos);
            e.Handled = true;
        }

        public void RemoveFrom(CardInfo card)
        {
            int index = Cards.IndexOf(card);
            if (index >= 0)
                Cards.RemoveRange(index, Cards.Count - index);
        }

        public void AddCards(IEnumerable<CardInfo> cards)
        {
            Cards.AddRange(cards);
        }

        public void AddCard(CardInfo card)
        {
            Cards.Add(card);
            Cards Card = new Cards(card);
            CardViews.Add(new Cards(card));
        }

        public void RemoveCard(CardInfo card)
        {
            int TmpInx = Cards.IndexOf(card);
            Cards.Remove(card);
            int num = 0;//test
            CardViews.RemoveAt(TmpInx);
            if (CardViews.Count > 0)
                CardViews[CardViews.Count - 1].Cover.Visibility = Visibility.Collapsed;
        }   

        public void MoveCards(int TmpInx)
        {
            Lists.MovingCards = Cards.Skip(TmpInx).ToList();
            for (int i = TmpInx; i < Cards.Count; i++)
                Cards.RemoveAt(i);
        }   

        public void MoveCards(CardInfo card)
        {
            int TmpInx = Cards.IndexOf(card);
            Lists.MovingCards = Cards.Skip(TmpInx).ToList();
            for (int i = TmpInx; i < Cards.Count; i++)
                Cards.RemoveAt(i);
        }   
    }
}
