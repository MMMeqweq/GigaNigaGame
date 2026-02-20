using GigaNigaGame.Folders.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GigaNigaGame
{
    public partial class Cards : UserControl
    {
        // model and owner references for the long-term approach
        public CardInfo Model { get; private set; }
        public StackPile Owner { get; set; }

        public Cards(CardInfo card)
        {
            InitializeComponent();
            Model = card;
            Button.Content = card.Num;
            BColor.Background = card.CardColor;
            CardImage.Source = new BitmapImage(new Uri(card.SuitSource));
            bool FaceUp = card.FaceUp;
        }

        private void Cover_Click(object sender, RoutedEventArgs e)
        {
            // call the long-term API: pass the owner and this view
            AnimationAndMoves.BPress(Owner, this);
        }
    }
}
