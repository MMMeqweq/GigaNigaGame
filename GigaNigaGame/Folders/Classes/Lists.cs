using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaNigaGame
{
    public static class Lists
    {
        internal static List<CardInfo> CardSet = new List<CardInfo>();
        internal static List<CardView> CardUC = new List<CardView>();
        internal static List<CardView> FaceOffCards = new List<CardView>();
        internal static List<CardView> FaceUpCards = new List<CardView>();
        internal static List<StackPile> StackPiles = new List<StackPile>();
        internal static List<CardInfo> MovingCards = new List<CardInfo>();
        internal static List<CardView> MovingCardsView = new List<CardView>();
        internal static List<CardInfo> Spades = new List<CardInfo>();
        internal static List<CardInfo> Diamonds = new List<CardInfo>();
        internal static List<CardInfo> Hearts = new List<CardInfo>();
        internal static List<CardInfo> Clubs = new List<CardInfo>();
        internal static List<StackPile> Piles = new List<StackPile>();
    }
}
