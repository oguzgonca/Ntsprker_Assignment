using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.NotificationService
{
    public class NotificationProvider
    {
        public List<INotifier> Notifiers { get; set; }

        public async Task NotifyAsync(NotifyTypes notificationType, string notification)
        {
            foreach (var notifier in Notifiers.Where(x=>x.Type == notificationType))
            {
                await notifier.NotifyAsync(notification);
            }
        }

    }
}
