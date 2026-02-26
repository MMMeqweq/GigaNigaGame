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
        internal static List<Cards> CardUC = new List<Cards>();
        internal static List<Cards> FaceOffCards = new List<Cards>();
        internal static List<Cards> FaceUpCards = new List<Cards>();
        internal readonly static List<StackPile> StackPiles = new List<StackPile>();
        internal static List<CardInfo> MovingCards = new List<CardInfo>();
    }
}
