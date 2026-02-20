using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GigaNigaGame.Folders.Classes
{
    internal static class AnimationAndMoves
    {
        public static void BPress(StackPile owner, Cards cardView)
        {
            if (owner == null || cardView == null) return;

            int pileIndex = Lists.StackPiles.IndexOf(owner);
            int cardIndex = owner.Cards.IndexOf(cardView.Model);

            if (Application.Current?.MainWindow is MainWindow main)
            {
                main.Dispatcher.Invoke(() =>
                {
                    main.TempTest.Text = $"Pile {pileIndex} CardIdx {cardIndex} Num {cardView.Model?.Num}";

                    // Try to find a valid pile to move to
                    foreach (StackPile pile in Lists.StackPiles)
                    {
                        if (pile == owner) continue; // Don't move to the same pile
                        if (pile.Cards.Count == 0)
                        {
                            // Optionally allow moving to empty piles (e.g., only Kings)
                            // Example: if (cardView.Model.Num == 13)
                            // {
                            //     Move logic here
                            // }
                            continue;
                        }

                        CardInfo topCard = pile.Cards[pile.Cards.Count - 1];
                        // Standard Solitaire rule: move to a card of opposite color and one higher in value
                        if ((topCard.Num == cardView.Model.Num + 1) && (topCard.CardColor != cardView.Model.CardColor))
                        {
                            owner.Cards.Remove(cardView.Model);
                            pile.Cards.Add(cardView.Model);

                            var currentParent = cardView.Parent as Panel;
                            currentParent?.Children.Remove(cardView);

                            Panel targetContainer = FindPanelInObject(pile);

                            if (targetContainer == null)
                            {
                                targetContainer = FindNamedPanelOnMain(main, new[] { "MainCanvas", "MainGrid", "GameCanvas", "RootPanel" }) 
                                                  ?? FindPanelInObject(main);
                            }

                            if (targetContainer != null)
                            {
                                targetContainer.Children.Add(cardView);

                                if (targetContainer is Canvas)
                                {
                                    double topOffset = Math.Max(0, targetContainer.Children.Count - 1) * 20.0;
                                    Canvas.SetLeft(cardView, 0);
                                    Canvas.SetTop(cardView, topOffset);
                                }

                                Panel.SetZIndex(cardView, 1000);
                            }
                            else
                            {
                                if (currentParent != null)
                                {
                                    currentParent.Children.Add(cardView);
                                }
                            }

                            // Flip the new top card in the source pile if needed
                            if (owner.Cards.Count > 0)
                                owner.Cards[owner.Cards.Count - 1].FaceUp = true;

                            main.RenderAll();
                            return;
                        }
                    }
                });
            }
        }

        private static Panel FindPanelInObject(object obj)
        {
            if (obj == null) return null;

            Type t = obj.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var prop in t.GetProperties(flags))
            {
                try
                {
                    if (typeof(Panel).IsAssignableFrom(prop.PropertyType))
                    {
                        var val = prop.GetValue(obj) as Panel;
                        if (val != null) return val;
                    }
                }
                catch {}
            }

            foreach (var field in t.GetFields(flags))
            {
                try
                {
                    if (typeof(Panel).IsAssignableFrom(field.FieldType))
                    {
                        var val = field.GetValue(obj) as Panel;
                        if (val != null) return val;
                    }
                }
                catch {}
            }

            return null;
        }

        private static Panel FindNamedPanelOnMain(object mainObj, string[] names)
        {
            if (mainObj == null || names == null) return null;

            Type t = mainObj.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var name in names)
            {
                var prop = t.GetProperty(name, flags);
                if (prop != null && typeof(Panel).IsAssignableFrom(prop.PropertyType))
                {
                    try
                    {
                        var val = prop.GetValue(mainObj) as Panel;
                        if (val != null) return val;
                    }
                    catch { }
                }

                var field = t.GetField(name, flags);
                if (field != null && typeof(Panel).IsAssignableFrom(field.FieldType))
                {
                    try
                    {
                        var val = field.GetValue(mainObj) as Panel;
                        if (val != null) return val;
                    }
                    catch { }
                }
            }

            return null;
        }
    }
}
