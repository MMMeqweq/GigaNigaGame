using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GigaNigaGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum Suit { Hearts, Spades, Clubs, Diamonds }
    public partial class MainWindow : Window
    {
        internal const int CardWidth = 75;
        internal const int CardHeight = 125;
        static Random Yoff = new Random();
        static Random Xoff = new Random();


        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 52; i++)
            {
                int number = i / 4 + 1;
                CardInfo card = new CardInfo()
                {
                    Num = number,
                    suit = CardInfo.SuitChoose(i % 4),
                    CardColor = CardInfo.ColorChoose(i % 4)
                };
                card.SuitSource = CardInfo.Suits(card).ToString();
                Lists.CardSet.Add(card);
                Cards cards = new Cards(card)
                {
                    Width = CardWidth,
                    Height = CardHeight
                };
                Lists.CardUC.Add(cards);
            }
            CardInfo.Shuffle(Lists.CardUC);
            CardInfo.ShuffleModel(Lists.CardSet);
            //int counter = 0;
            //int cardamount = 1;   
            for (int i = 1; i < 8; i++)
            {

                /*Canvas canvas = new Canvas();
                canvas.Background = ColorDecide(i%2);
                Grid.SetColumn(canvas, i-1);
                Grid.SetRow(canvas, 1);
                int offset = 0;
                for (int j = 0; j < cardamount; j++)
                {
                    var ca = Lists.CardUC[counter];
                    Canvas.SetLeft(ca, 0);
                    Canvas.SetTop(ca, offset);
                    if (j == cardamount-1)
                        ca.Cover.Visibility = Visibility.Collapsed;
                    canvas.Children.Add(ca);
                    offset += 30;
                    counter++;

                }

                MainGrid.Children.Add(canvas);
                cardamount++;
                */

                var pile = new StackPile();
                pile.MoveRequested += OnMoveRequested; ;

                Grid.SetRow(pile, 1);
                Grid.SetColumn(pile, i - 1);
                MainGrid.Children.Add(pile);

                Lists.StackPiles.Add(pile);
            }

            Deal();
            RenderAll();
            int CardsLeft = Lists.CardUC.Count;
            for (int i = CardsLeft - 1; i >= 0; i--)
            {
                var card = Lists.CardUC[i];
                int Yoffset = Yoff.Next(-7, 7);
                int Xoffset = Xoff.Next(-7, 7);
                Canvas.SetLeft(card, Yoffset);
                Canvas.SetTop(card, Xoffset);
                FaceOffCards.Children.Add(card);
                Lists.CardUC.RemoveAt(i);
                Lists.CardSet.RemoveAt(i);
                if (i == CardsLeft)
                    card.Cover.Visibility = Visibility.Collapsed;
            }

            TempTest.Text = Lists.CardSet.Count.ToString() + " and" + Lists.CardUC.Count.ToString();
        }

        private void OnMoveRequested(StackPile arg1, List<CardInfo> arg2, Point arg3)
        {
            
        }

        private void Deal()
        {  
            const int totalPiles = 7;
            for (int pileIndex = 0; pileIndex < totalPiles; pileIndex++)
            {
                int cardsToDeal = pileIndex + 1; 

                for (int j = 0; j < cardsToDeal; j++)
                {
                    if (Lists.CardSet.Count == 0)
                        return; 
                    var card = Lists.CardSet[0];
                    card.FaceUp = (j == cardsToDeal - 1);
                    Lists.StackPiles[pileIndex].Cards.Add(card);
                    Lists.CardSet.RemoveAt(0);

                    if (Lists.CardUC.Count > 0)
                        Lists.CardUC.RemoveAt(0);
                }
            }
        }

        private void OnMoveRequested(StackPile From, List<CardInfo> Moving)
        {
            int FromIndexer = Lists.StackPiles.IndexOf(From);
            int ToIndex = Math.Min(FromIndexer + 1, Lists.StackPiles.Count - 1);
            var to = Lists.StackPiles[ToIndex];

            From.RemoveFrom(Moving[0]);

            to.AddCards(Moving);

            if (From.Cards.Count > 0)
                From.Cards[From.Cards.Count - 1].FaceUp = true;

            RenderAll();
        }

        internal void RenderAll()
        {
            foreach (var pile in Lists.StackPiles)
                pile.Render();
        }

        private void BuildDeck()
        {
            Lists.CardSet.Clear();
            for (int i = 0; i < 52; i++)
            {
                int num = i / 4 + 1;
                Lists.CardSet.Add(new CardInfo
                {
                    Num = num,
                    suit = CardInfo.SuitChoose(i % 4),
                    FaceUp = false
                });
            }
        }

        private static readonly Random _rng = new Random();
        private static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        static Brush ColorDecide(int num)
        {
            switch (num)
            {
                case 0:
                    return Brushes.LightGreen;

                case 1:
                    return Brushes.DarkGreen;

                default:
                    return Brushes.LightGreen;

            }
        }
    }
}
