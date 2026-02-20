using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GigaNigaGame
{
    public class CardInfo
    {
        public int Num;
        public Brush CardColor;
        public Suit suit;
        public bool FaceUp = false;
        public string SuitSource;
        public Point point;
        static readonly Random random = new Random();
        
            
        


        internal static ImageSource Suits(CardInfo card)
        {
            string fileName;

            switch (card.suit)
            {
                case Suit.Hearts:
                    fileName = "Hearts.png";
                    break;
                case Suit.Spades:
                    fileName = "Spades.png";
                    break;
                case Suit.Clubs:
                    fileName = "Clubs.png";
                    break;
                case Suit.Diamonds:
                    fileName = "Diamonds.png";
                    break;
                default:
                    fileName = "Hearts.png";
                    break;
            }

            return new BitmapImage(new Uri($"pack://application:,,,/Folders/Images/{fileName}"));
        }
        internal static Suit SuitChoose(int num)
        {
            switch (num)
            {
                case 0:
                    return GigaNigaGame.Suit.Hearts;

                case 1:
                    return GigaNigaGame.Suit.Spades;

                case 2:
                    return GigaNigaGame.Suit.Clubs;

                case 3:
                    return GigaNigaGame.Suit.Diamonds;

                default:
                    return GigaNigaGame.Suit.Hearts;
            }
        }

        internal static List<Cards> Shuffle(List<Cards> list)
        { 
            for (int i = 0; i < 100; i++)
            {
                for (; i < list.Count; i++)
                {
                    var temp = list[i];
                    int j = random.Next(list.Count);
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
            return list;
        }

        internal static List<CardInfo> ShuffleModel(List<CardInfo> list)
        {
            for (int i = 0; i < 100; i++)
            {
                for (; i < list.Count; i++)
                {
                    var temp = list[i];
                    int j = random.Next(list.Count);
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
            return list;
        }
    }
}
