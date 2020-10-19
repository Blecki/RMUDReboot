using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
    public class Topic : MudObject
    {
        public Topic()
        {
            SetProperty("article", "");
        }

        public Topic Available(Func<MudObject, MudObject, MudObject, CheckResult> Func, String Name = "")
        {
            Check<MudObject, MudObject, MudObject>("topic available?").Do(Func).Name(Name);
            return this;
        }

        public Topic Available(Func<bool> Func, String Name = "")
        {
            Check<MudObject, MudObject, MudObject>("topic available?").Do((a, b, c) =>
                {
                    if (!Func()) return CheckResult.Disallow;
                    return CheckResult.Continue;
                }).Name(Name);
            return this;
        }

        public bool Discussed
        {
            get { return this.GetProperty<bool>("topic-discussed"); }
        }

        public Topic Follows(Topic Previous)
        {
            return this.Available(() => Previous.Discussed, "B follows A topic ordering rule");
        }
    }

    public static class ResponseExtensionMethods
    {
        public static List<MudObject> InitializeConversationTopics(this MudObject To)
        {
            if (!To.HasProperty("conversation-topics"))
                To.SetProperty("conversation-topics", new List<MudObject>());

            return To.GetProperty<List<MudObject>>("conversation-topics");
        }

        public static Topic Response(this MudObject To, String Topic, String StringResponse)
        {
            return Response(To, Topic, (actor, npc, topic) =>
                {
                    Core.SendMessage(actor, StringResponse, npc);
                    return PerformResult.Stop;
                });
        }

        public static Topic Response(this MudObject To, String Topic, Func<MudObject, MudObject, MudObject, PerformResult> FuncResponse)
        {
            var topics = To.InitializeConversationTopics();

            var response = new Topic();
            topics.Add(response);
            response.SimpleName(Topic);
            response.Perform<MudObject, MudObject, MudObject>("topic response").Do(FuncResponse);
            return response;
        }

        public static void DefaultResponse(this MudObject To, Func<MudObject, MudObject, MudObject, PerformResult> FuncResponse)
        {
            To.InitializeConversationTopics();

            To.Perform<MudObject, MudObject, MudObject>("topic response").When((actor, npc, topic) => topic == null).Do(FuncResponse);
        }

        public static void DefaultResponse(this MudObject To, String StringResponse)
        {
            DefaultResponse(To, (actor, npc, topic) =>
            {
                Core.SendMessage(actor, StringResponse);
                return PerformResult.Stop;
            });
        }

        public static RuleBuilder<MudObject, MudObject, PerformResult> PerformNoTopicsToDiscuss(this MudObject To)
        {
            To.InitializeConversationTopics();

            return To.Perform<MudObject, MudObject>("no topics to discuss").ThisOnly(1);
        }

        public static RuleBuilder<MudObject, MudObject, CheckResult> CheckCanConverse(this MudObject To)
        {
            return To.Check<MudObject, MudObject>("can converse?").ThisOnly(1);
        }

        public static RuleBuilder<MudObject, MudObject, PerformResult> PerformGreet(this MudObject To)
        {
            return To.Perform<MudObject, MudObject>("greet").ThisOnly(1);
        }
    }
}
