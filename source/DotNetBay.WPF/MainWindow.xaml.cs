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
                this.Auctions.Add(item);
            }
        }
    }
}
