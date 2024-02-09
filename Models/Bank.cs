﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAPI;

public class Bank
{
    public string BankName { get; set; }
    public string BankId { get; set; }
    public decimal RTGSChargeSameBank { get; set; } = 0;
    public decimal IMPSChargeSameBank { get; set; } = (decimal)0.05;
    public decimal RTGSChargeOtherBank { get; set; } = (decimal)0.02;
    public decimal IMPSChargeOtherBank { get; set; } = (decimal)0.06;

}
