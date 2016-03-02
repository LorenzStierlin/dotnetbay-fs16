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
            MainRepository = new FileSystemMainRepository("appdata.json");
            AuctionRunner = new AuctionRunner(MainRepository);

            MainRepository.SaveChanges();
            AuctionRunner.Start();

            var memberService = new SimpleMemberService(MainRepository);
            var service = new AuctionService(MainRepository, memberService);

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
