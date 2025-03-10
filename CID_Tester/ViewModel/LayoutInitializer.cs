﻿using AvalonDock.Layout;
using System.Windows;

namespace CID_Tester.ViewModel
{
    class LayoutInitializer : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {

            var rightPane = new LayoutAnchorablePane
            {
                DockWidth = new GridLength(250),
                IsMaximized = true,
            };

            rightPane.Children.Add(anchorableToShow);
            layout.Descendents().OfType<LayoutPanel>().FirstOrDefault()?.Children.Add(rightPane);

            return true;

        }


        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }


        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {

        }
    }
}
