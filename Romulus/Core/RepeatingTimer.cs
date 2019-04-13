using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Romulus.Core
{
    internal static class RepeatingTimer
    {
        private static Timer loopingTimer;

        internal static Task StartTimer()
        {
            loopingTimer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            //loopingTimer.Elapsed += LoopingTimer_Elapsed;

            return Task.CompletedTask;
        }

        //private static void LoopingTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
