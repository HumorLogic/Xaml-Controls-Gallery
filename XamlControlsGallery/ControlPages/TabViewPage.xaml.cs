﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using AppUIBasics.SamplePages;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.ViewManagement;
using Windows.UI.Core;
using AppUIBasics.TabViewPages;

namespace AppUIBasics.ControlPages
{
    public sealed partial class TabViewPage : Page
    {
        public TabViewPage()
        {
            this.InitializeComponent();

            for (int i = 0; i < 3; i++)
            {
                TabView1.TabItems.Add(CreateNewTab(i));
                TabView2.TabItems.Add(CreateNewTab(i));
            }
        }

        private void TabView_AddButtonClick(TabView sender, object args)
        {
            sender.TabItems.Add(CreateNewTab(sender.TabItems.Count));
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private TabViewItem CreateNewTab(int index)
        {
            TabViewItem newItem = new TabViewItem();

            newItem.Header = "Document " + index;
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document};

            Frame frame = new Frame();

            switch (index % 3)
            {
                case 0:
                    frame.Navigate(typeof(SamplePage1));
                    break;
                case 1:
                    frame.Navigate(typeof(SamplePage2));
                    break;
                case 2:
                    frame.Navigate(typeof(SamplePage3));
                    break;
            }

            newItem.Content = frame;

            return newItem;
        }

        private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var senderTabView = args.Element as TabView;
            senderTabView.TabItems.Add(CreateNewTab(senderTabView.TabItems.Count));
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            // Only close the selected tab if it is closeable
            if (((TabViewItem)InvokedTabView.SelectedItem).IsClosable)
            {
                InvokedTabView.TabItems.Remove(InvokedTabView.SelectedItem);
            }
        }

        private void NavigateToNumberedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            int tabToSelect = 0;

            switch (sender.Key)
            {
                case Windows.System.VirtualKey.Number1:
                    tabToSelect = 0;
                    break;
                case Windows.System.VirtualKey.Number2:
                    tabToSelect = 1;
                    break;
                case Windows.System.VirtualKey.Number3:
                    tabToSelect = 2;
                    break;
                case Windows.System.VirtualKey.Number4:
                    tabToSelect = 3;
                    break;
                case Windows.System.VirtualKey.Number5:
                    tabToSelect = 4;
                    break;
                case Windows.System.VirtualKey.Number6:
                    tabToSelect = 5;
                    break;
                case Windows.System.VirtualKey.Number7:
                    tabToSelect = 6;
                    break;
                case Windows.System.VirtualKey.Number8:
                    tabToSelect = 7;
                    break;
                case Windows.System.VirtualKey.Number9:
                    // Select the last tab
                    tabToSelect = InvokedTabView.TabItems.Count - 1;
                    break;
            }

            // Only select the tab if it is in the list
            if (tabToSelect < InvokedTabView.TabItems.Count)
            {
                InvokedTabView.SelectedIndex = tabToSelect;
            }
        }

        private void TabWidthBehaviorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string widthModeString = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            TabViewWidthMode widthMode = TabViewWidthMode.Equal;
            switch (widthModeString)
            {
                case "Equal":
                    widthMode = TabViewWidthMode.Equal;
                    break;
                case "SizeToContent":
                    widthMode = TabViewWidthMode.SizeToContent;
                    break;
            }
            TabView3.TabWidthMode = widthMode;
        }

        private async void TabViewWindowingButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(TabViewWindowingSamplePage), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }
    }
}