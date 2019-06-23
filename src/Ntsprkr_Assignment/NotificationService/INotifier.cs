using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.NotificationService
{
    public interface INotifier
    {
        NotifyTypes Type { get; }
        Task NotifyAsync(string notification);
    }
}
