using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.NotificationService
{
    public class SMSNotifer : INotifier
    {
        public NotifyTypes Type => NotifyTypes.SMS;

        public async Task NotifyAsync(string notification)
        {
             throw new NotImplementedException();
        }
    }
}
