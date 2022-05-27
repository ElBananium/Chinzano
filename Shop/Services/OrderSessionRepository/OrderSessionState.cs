using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderSessionRepository
{
    public enum OrderSessionState
    {
        HowManyNeed,
        WhatTime,
        GetCurrentTime,
        End
    }
}
