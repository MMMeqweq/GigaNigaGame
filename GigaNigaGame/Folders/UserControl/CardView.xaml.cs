using GigaNigaGame.Folders.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GigaNigaGame
{
    public partial class CardView : UserControl
    {
        public CardInfo Model { get; private set; }
        public StackPile Owner { get; set; }
        public XAndY Location = new XAndY(0, 0);

        public CardView(CardInfo card)
        {
            InitializeComponent();
            Model = card;
            TAG.Text = card.set.ToString();
            Button.Content = card.Num;
            CardImage.Source = new BitmapImage(new Uri(card.SuitSource));
            Button.Foreground = (Brush)new BrushConverter().ConvertFromString(card.CardColor);
            Cover.Visibility = card.FaceUp ? Visibility.Collapsed : Visibility.Visible;
            Location = card.point;
        }
        public CardView(CardInfo card, XAndY Pos)
        {
            InitializeComponent();
            Model = card;
            TAG.Text = card.set.ToString();
            Button.Content = card.Num;
            CardImage.Source = new BitmapImage(new Uri(card.SuitSource));
            Button.Foreground = (Brush)new BrushConverter().ConvertFromString(card.CardColor);
            Cover.Visibility = card.FaceUp ? Visibility.Collapsed : Visibility.Visible;
            Location.SetX(Pos.GetX());
            Location.SetY(Pos.GetY());
        }

        private void Cover_Click(object sender, RoutedEventArgs e)
        {
            // safe call into animation/move logic using the explicit owner
            AnimationAndMoves.BPress(Owner, this);
        }


    }
}
