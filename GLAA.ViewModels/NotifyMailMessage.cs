using System;
using System.Collections.Generic;
using System.Text;

namespace GLAA.ViewModels
{
    public class NotifyMailMessage
    {
        public NotifyMailMessage(string to, Dictionary<string, dynamic> personalisation)
        {
            To = to;
            Personalisation = personalisation;
        }
 
        public string To { get; set; }

        public Dictionary<string, dynamic> Personalisation { get; set; }
    }
}
