using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChat_Local_API.Objects
{
    public class EventListenerConfig
    {
        public bool DebugMode { get; set; } = true;
        public bool RequireGameRunning { get; set; } = true;
        public bool ListenToActiveEvent { get; set; } = true;
    }
}
