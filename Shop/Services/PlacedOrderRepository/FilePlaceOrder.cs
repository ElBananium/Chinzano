using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderRepository
{
    public class FilePlaceOrder  : PlacedOrder
    {
        private PlacedOrder _placedorder;

        public override string TradeRepoName { get
            {
                return _placedorder.TradeRepoName;
            } set
            {
                _placedorder.TradeRepoName = value;
                Serialize();

            }
        }

        public override int Id { get => _placedorder.Id; set => throw new NotImplementedException(); }

        public override int HowManyOrdered { get => _placedorder.HowManyOrdered; set
            {
                _placedorder.HowManyOrdered = value;
                Serialize();
            }
        }

        public override string WhatTime
        {
            get => _placedorder.WhatTime; 
            set
            {
                _placedorder.WhatTime = value;
                Serialize();
            }
        }

        public override bool IsRecived { get => _placedorder.IsRecived; set
            {
                _placedorder.IsRecived = value;
                Serialize();

            }
        }

        public override bool IsPicked { get => _placedorder.IsPicked; set
            {
                _placedorder.IsPicked = value;
                Serialize();
            }
        }

        public override ulong WhosPickedId { get => _placedorder.WhosPickedId; set
            {
                _placedorder.WhosPickedId = value;
                Serialize();


            }
        }

        public override string WhosPickedNickname { get => _placedorder.WhosPickedNickname; set
            {
                _placedorder.WhosPickedNickname = value;
                Serialize();

            }
        }

        public override ulong ChannelId { get => _placedorder.ChannelId; set
            {
                _placedorder.ChannelId = value;
                Serialize();
            }
        }


        public void Serialize()
        {
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/PlacedOrders/" + _placedorder.Id + ".json"))
            {
                sw.Write(JsonConvert.SerializeObject(_placedorder));
            }
        }

        


        public FilePlaceOrder(PlacedOrder order)
        {
            _placedorder = order;
        }
    }
}
