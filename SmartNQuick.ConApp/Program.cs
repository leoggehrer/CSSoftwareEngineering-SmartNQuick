﻿//@BaseCode
using System;

namespace SmartNQuick.ConApp
{
	partial class Program
    {
        static void Main(/*string[] args*/)
        {
            Console.WriteLine("Test SmartNQuick");

            BeforeRun();

            AfterRun();
        }

        static partial void BeforeRun();
        static partial void AfterRun();
    }
}
