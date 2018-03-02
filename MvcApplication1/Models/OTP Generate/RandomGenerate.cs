using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models.OTP_Generate
{
    public static class RandomGenerate
    {
        public static string Otp
        {
            get
            {
                Random generator = new Random();
                String randomNumber = generator.Next(0, 1000000).ToString("D6");
                return randomNumber;
            }
        }
    }
}