using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace RMUD
{
    public class HeartbeatRules 
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.DeclarePerformRuleBook<MudObject>("heartbeat", "[Object] : Considered every tick.", "Object");
        }
    }

    internal class Timer
    {
        public DateTime StartTime;
        public TimeSpan Interval;
        public Action Action;
    }

    public static partial class Core
    {
        public static PerformResult ConsiderLocalOnlyPerformRule(MudObject On, String Name, Object[] Arguments)
        {
            if (Arguments == null) Arguments = new Object[] { null };
            var ruleset = On.Rules;
            return ruleset.ConsiderPerformRule(Name, Arguments);
        }

        internal static int CurrentHeartbeat = 1;
        internal static HashSet<MudObject> HeartbeatSet = new HashSet<MudObject>();

        internal static List<Timer> ActiveTimers = new List<Timer>();

        public static void AddTimer(TimeSpan Interval, Action Action)
        {
            if (Interval < TimeSpan.FromSeconds(1))
            {
                Interval = TimeSpan.FromSeconds(1);
                Core.LogWarning("Timer created with interval less than minimum value.");
            }

            ActiveTimers.Add(new Timer
            {
                StartTime = DateTime.Now,
                Interval = Interval,
                Action = Action
            });
        }

        internal static DateTime TimeOfLastHeartbeat = DateTime.Now;

        /// <summary>
        /// Process the Heartbeat. It is assumed that this function is called periodically by the command processing loop.
        /// When called, this function will invoke the "heartbeat" rulebook if enough time has passed since the last
        /// invokation. 
        /// </summary>
        internal static void Heartbeat()
        {
            var now = DateTime.Now;

            var timeSinceLastBeat = now - TimeOfLastHeartbeat;
            if (timeSinceLastBeat.TotalMilliseconds >= SettingsObject.HeartbeatInterval)
            {
                Core.TimeOfDay += Core.SettingsObject.ClockAdvanceRate;
                TimeOfLastHeartbeat = now;
                CurrentHeartbeat += 1;

                var heartbeatObjects = new HashSet<MudObject>();
                foreach (var client in Clients.ConnectedClients)
                    foreach (var visibleObject in Core.EnumerateVisibleTree(Core.FindLocale(client.Player)))
                        heartbeatObjects.Add(visibleObject);

                foreach (var heartbeatObject in heartbeatObjects)
                {
                    HeartbeatSet.Add(heartbeatObject);
                    heartbeatObject.CurrentHeartbeat = CurrentHeartbeat;
                }

                HeartbeatSet.RemoveWhere(o => o.CurrentHeartbeat < (CurrentHeartbeat - Core.SettingsObject.LiveHeartbeats));

                foreach (var heartbeatObject in HeartbeatSet)
                    GlobalRules.ConsiderPerformRule("heartbeat", heartbeatObject);

                //In case heartbeat rules emitted messages.
                Core.SendPendingMessages();
            }

            for (var i = 0; i < ActiveTimers.Count;)
            {
                var timerFireTime = ActiveTimers[i].StartTime + ActiveTimers[i].Interval;
                if (timerFireTime <= now)
                {
                    ActiveTimers[i].Action();
                    SendPendingMessages();
                    ActiveTimers.RemoveAt(i);
                }
                else
                    ++i;
            }
        }
    }
}
