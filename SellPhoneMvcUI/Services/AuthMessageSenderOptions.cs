﻿namespace SellPhoneMvcUI.Services
{
    public class AuthMessageSenderOptions
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } 
        public string SmtpPassword { get; set; } 
    }
}
