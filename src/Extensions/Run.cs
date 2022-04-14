using System;
using System.Diagnostics;

namespace Strada.Framework.Extensions
{
    /// <summary>
    /// Run
    /// </summary>
    public static class Run
    {
        /// <summary>
        /// Obtient une valeur qui indique si l'application expose des métriques Prometheus
        /// </summary>
        public static bool HasMetrics { get; internal set; }

        /// <summary>
        /// Obtient une valeur qui indique si le programme tourne manifestement en Debug<br/>
        /// (information basée sur l'attachement d'un Debugger au programme)
        /// </summary>
        /// <remarks>
        /// La constante de compilation DEBUG ne peut pas être exploitée dans la lib pour l'usage souhaité.
        /// </remarks>
        /// <see cref="Debugger.IsAttached"/>
        public static bool IsDevelopment { get; } = Debugger.IsAttached;

        /// <summary>
        /// Obtient une valeur qui indique si le programme tourne dans un environnement conteneurisé
        /// </summary>
        /// <remarks>
        /// Basé sur les variables d'environnement DOTNET_RUNNING_IN_CONTAINER et DOTNET_RUNNING_IN_CONTAINERS
        /// </remarks>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables#dotnet_running_in_container-and-dotnet_running_in_containers"/>
        public static bool IsInContainer { get; } = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.ToLower() == "true" ||
                                                    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINERS")?.ToLower() == "true";

        /// <summary>
        /// Retoure la valeur associée à un argument de ligne de commande
        /// </summary>
        /// <remarks>
        /// Accepte le format court ou long :<br/>
        /// court : -a "value"<br/>
        /// long : --arg "value"<br/>
        /// (les quotes sont optionnelles)
        /// </remarks>
        public static string GetArgumentValue(string argumentName)
        {
            string[] args = Environment.GetCommandLineArgs();

            string result = GetArgumentValue(args, argumentName);
            return result;
        }

        /// <summary>
        /// Retoure la valeur associée à un argument de ligne de commande
        /// </summary>
        /// <remarks>
        /// Accepte le format court ou long :<br/>
        /// court : -a "value"<br/>
        /// long : --arg "value"<br/>
        /// (les quotes sont optionnelles)
        /// </remarks>
        public static string GetArgumentValue(string[] args, string argumentName)
        {
            if (args?.Length > 0)
            {
                string fullName = $"--{argumentName}";
                string shortName = $"-{argumentName[0]}";

                for (int i = 0; i < args.Length - 1; i++)
                {
                    string arg = args[i];

                    if (arg == fullName || arg == shortName)
                    {
                        string result = args[i + 1];
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
