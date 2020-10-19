using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace RMUD
{
    public enum StartupFlags
    {
        NoFlags = 0,
        Silent = 1,
        SingleThreaded = 2,
        NoLog = 4
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class RunAtStartupAttribute : Attribute
    {

    }

    public static partial class Core
    {
        private static void InitializeEngine(Assembly Assembly)
        {
            foreach (var type in Assembly.GetTypes())
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    var isStartupFunction = false;
                    var attribute = method.GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(RunAtStartupAttribute));
                    if (attribute != null)
                        isStartupFunction = true;

                    if (method.Name == "AtStartup")
                        isStartupFunction = true;

                    if (!isStartupFunction) continue;

                    var methodParameters = method.GetParameters();

                    if (methodParameters.Length == 0)
                    {
                        try
                        {
                            method.Invoke(null, null);
                        }
                        catch (Exception e)
                        {
                            LogWarning("Error while initializing engine: " + e.Message);
                        }
                    }
                    else if (methodParameters.Length == 1 && methodParameters[0].ParameterType == typeof(RuleEngine))
                    {
                        try
                        {
                            method.Invoke(null, new Object[] { GlobalRules });
                        }
                        catch (Exception e)
                        {
                            LogWarning("Error while initializing engine: " + e.Message);
                        }
                    }
                    else
                        LogWarning("Error while initializing engine: AtStartup method had incompatible signature.");
                        // Todo: Record function name.
                }
        }

        /// <summary>
        /// Start the mud engine.
        /// </summary>
        /// <param name="Flags">Flags control engine functions</param>
        /// <param name="Database"></param>
        /// <param name="Assemblies">Modules to integrate</param>
        /// <returns></returns>
        public static bool Start(StartupFlags Flags, String DatabasePath, WorldDataService Database, Assembly AdditionalAssembly = null)
        {
            Core.DatabasePath = DatabasePath;

            ShuttingDown = false;
            Core.Flags = Flags;

            try
            {
                // Setup the rule engine and some basic rules.
                GlobalRules = new RuleEngine(NewRuleQueueingMode.QueueNewRules);
                GlobalRules.DeclarePerformRuleBook("at startup", "[] : Considered when the engine is started.");
                GlobalRules.DeclarePerformRuleBook<MudObject>("singleplayer game started", "Considered when a single player game is begun");

                DefaultParser = new CommandParser();

                InitializeEngine(Assembly.GetExecutingAssembly());
                if (AdditionalAssembly != null)
                    InitializeEngine(AdditionalAssembly);
                InitializeCommandProcessor();

                GlobalRules.FinalizeNewRules();

                Core.Database = Database;
                Database.Initialize();

                GlobalRules.ConsiderPerformRule("at startup");

                if ((Flags & StartupFlags.SingleThreaded) == 0)
                    StartThreadedCommandProcesor();
            }
            catch (Exception e)
            {
                LogError("Failed to start mud engine.");
                LogError(e.Message);
                LogError(e.StackTrace);
                throw;
            }
            return true;
        }
    }
}
