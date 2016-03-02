using System;
using System.Linq;
using System.Windows;
using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.FileStorage;
using DotNetBay.Interfaces;
using DotNetBay.Model;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public readonly IMainRepository MainRepository;
        public readonly IAuctionRunner AuctionRunner;


        public App()
        {
            this.MainRepository = new FileSystemMainRepository("appdata.json");
            this.AuctionRunner = new AuctionRunner(this.MainRepository);

            this.MainRepository.SaveChanges();
            this.AuctionRunner.Start();

            var memberService = new SimpleMemberService(this.MainRepository);
            var service = new AuctionService(this.MainRepository, memberService);

            if (!service.GetAll().Any())
            {
                var me = memberService.GetCurrentMember();

                for (var i = 0; i < 5; i++)
                {
                    service.Save(new Auction
                    {
                        Title = "My First Auction",
                        StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10),
                        EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                        StartPrice = 72,
                        Seller = me
                    });
                }
            }
        }
    }
}
