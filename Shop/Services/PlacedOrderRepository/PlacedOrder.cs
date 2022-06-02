using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderRepository
{
    public class PlacedOrder
    {
        public virtual int Id { get; set; }

        public virtual string TradeRepoName { get; set; }

        public virtual int HowManyOrdered { get; set; }

        public virtual string WhatTime { get; set; }

        public virtual  bool IsRecived { get; set; }

        public virtual bool IsPicked { get; set; }

        public virtual ulong WhosPickedId { get; set; }

        public virtual string WhosPickedNickname { get; set; }

        public virtual int Price { get; set; }

        public virtual ulong ChannelId { get; set; }

    }
}
