using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using DotNetBay.Core;
using DotNetBay.Model;
using DotNetBay.WPF.Views;
using DotNetBay.Core.Execution;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Auction> Auctions { get; } = new ObservableCollection<Auction>();
        private AuctionService auctionService;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var app = App.Current as App;

            var memberService = new SimpleMemberService(app.MainRepository);
            auctionService = new AuctionService(app.MainRepository, memberService);

            foreach (var item in auctionService.GetAll())
            {
                Auctions.Add(item);       
            }

            app.AuctionRunner.Auctioneer.AuctionStarted += Auctioneer_AuctionStarted;
            app.AuctionRunner.Auctioneer.BidAccepted += Auctioneer_BidAccepted;
            app.AuctionRunner.Auctioneer.BidDeclined += Auctioneer_BidDeclined;
            app.AuctionRunner.Auctioneer.AuctionEnded += Auctioneer_AuctionEnded;
        }

        private void Auctioneer_AuctionEnded(object sender, AuctionEventArgs e)
        {
            LoadData();
        }

        private void Auctioneer_BidDeclined(object sender, ProcessedBidEventArgs e)
        {
            LoadData();
        }

        private void Auctioneer_BidAccepted(object sender, ProcessedBidEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            Auctions.Clear();

            foreach (var item in auctionService.GetAll())
            {
                Auctions.Add(item);
            }
        }
        private void Auctioneer_AuctionStarted(object sender, AuctionEventArgs e)
        {
            LoadData();
        }

        private void NewAuction_Click(object sender, RoutedEventArgs e)
        {
            var sellView = new SellView();
            sellView.ShowDialog(); // Blocking 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var buyView = new BuyView();
            buyView.ShowDialog();
        }
    }
}
