using System;
using UnityEngine;

namespace Core.Banking
{
    public class IBankService
    {
        ISurveillanceWallet SurveillanceWallet { get; }
        IOverworldWallet OverworldWallet { get; }
    }

    public delegate void BalanceChange(int changeAmount); 
}

