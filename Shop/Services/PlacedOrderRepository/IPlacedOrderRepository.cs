using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderRepository
{
    public interface IPlacedOrderRepository
    {
        public PlacedOrder GetOrder(int id);

        public PlacedOrder CreateOrder(string TradeRepoName,int HowManyOrdered, string WhatTime, bool IsRecived, ulong ChannelId, int price);

        public void DeleteOrder(int id);


    }
}
