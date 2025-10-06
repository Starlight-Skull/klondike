using Globals.Entities;
using Globals.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KlondikeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogic _cardLogic;

        public MainWindow(ILogic logic)
        {
            _cardLogic = logic;
            InitializeComponent();
            LoadCards();
        }

        private void LoadCards()
        {
            Errors.Content = "";
            CountLabel.Content = _cardLogic.TotalCards;
            MainDeckBox.SelectedIndex = -1;
            Pile1Box.SelectedIndex = -1;
            Pile2Box.SelectedIndex = -1;
            Pile3Box.SelectedIndex = -1;
            Pile4Box.SelectedIndex = -1;
            Pile5Box.SelectedIndex = -1;
            Pile6Box.SelectedIndex = -1;
            Pile7Box.SelectedIndex = -1;
            ClubsFoundationBox.SelectedIndex = -1;
            DiamondsFoundationBox.SelectedIndex = -1;
            HeartsFoundationBox.SelectedIndex = -1;
            SpadesFoundationBox.SelectedIndex = -1;
            MainDeckBox.ItemsSource = _cardLogic.Stock.ToStringArray();
            Pile1Box.ItemsSource = _cardLogic.Piles["Pile 1"].ToStringArray();
            Pile2Box.ItemsSource = _cardLogic.Piles["Pile 2"].ToStringArray();
            Pile3Box.ItemsSource = _cardLogic.Piles["Pile 3"].ToStringArray();
            Pile4Box.ItemsSource = _cardLogic.Piles["Pile 4"].ToStringArray();
            Pile5Box.ItemsSource = _cardLogic.Piles["Pile 5"].ToStringArray();
            Pile6Box.ItemsSource = _cardLogic.Piles["Pile 6"].ToStringArray();
            Pile7Box.ItemsSource = _cardLogic.Piles["Pile 7"].ToStringArray();
            ClubsFoundationBox.ItemsSource = _cardLogic.Foundations["Clubs"].ToStringArray();
            DiamondsFoundationBox.ItemsSource = _cardLogic.Foundations["Diamonds"].ToStringArray();
            HeartsFoundationBox.ItemsSource = _cardLogic.Foundations["Hearts"].ToStringArray();
            SpadesFoundationBox.ItemsSource = _cardLogic.Foundations["Spades"].ToStringArray();
        }

        private void PrintError(string message)
        {
            Errors.Content = message;
        }

        private void SelectCard(Deck pile, ListBox box)
        {
            try
            {
                _cardLogic.SelectCard(pile, box.SelectedIndex);
            }
            catch (Exception ex)
            {
                box.SelectedIndex = -1;
                PrintError(ex.Message);
            }
        }

        private void SelectSpecial(Deck pile, ListBox box)
        {
            try
            {
                box.SelectedIndex = pile.LastIndex;
                _cardLogic.SelectCard(pile, box.SelectedIndex);
            }
            catch (Exception ex)
            {
                box.SelectedIndex = -1;
                PrintError(ex.Message);
            }
        }

        private void MoveTo(Deck deck)
        {
            try
            {
                _cardLogic.MoveTo(deck);
                LoadCards();
            }
            catch (Exception ex) { PrintError(ex.Message); }
        }

        // draw card
        private void DrawCardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _cardLogic.DrawCard();
                LoadCards();
            }
            catch (Exception ex) { PrintError(ex.Message); }
        }

        // selection changed
        private void MainDeckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSpecial(_cardLogic.Stock, MainDeckBox);
        }

        private void ClubsFoundationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSpecial(_cardLogic.Foundations["Clubs"], ClubsFoundationBox);
        }

        private void DiamondsFoundationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSpecial(_cardLogic.Foundations["Diamonds"], DiamondsFoundationBox);
        }

        private void HeartsFoundationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSpecial(_cardLogic.Foundations["Hearts"], HeartsFoundationBox);
        }

        private void SpadesFoundationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSpecial(_cardLogic.Foundations["Spades"], SpadesFoundationBox);
        }

        private void Pile1Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 1"], Pile1Box);
        }

        private void Pile2Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 2"], Pile2Box);
        }

        private void Pile3Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 3"], Pile3Box);
        }

        private void Pile4Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 4"], Pile4Box);
        }

        private void Pile5Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 5"], Pile5Box);
        }

        private void Pile6Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 6"], Pile6Box);
        }

        private void Pile7Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCard(_cardLogic.Piles["Pile 7"], Pile7Box);
        }

        // doubleClick
        private void Pile1Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 1"]);
        }

        private void Pile2Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 2"]);
        }

        private void Pile3Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 3"]);
        }

        private void Pile4Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 4"]);
        }

        private void Pile5Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 5"]);
        }

        private void Pile6Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 6"]);
        }

        private void Pile7Box_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Piles["Pile 7"]);
        }

        private void ClubsFoundationBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Foundations["Clubs"]);
        }

        private void DiamondsFoundationBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Foundations["Diamonds"]);
        }

        private void HeartsFoundationBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Foundations["Hearts"]);
        }

        private void SpadesFoundationBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MoveTo(_cardLogic.Foundations["Spades"]);
        }
    }
}
