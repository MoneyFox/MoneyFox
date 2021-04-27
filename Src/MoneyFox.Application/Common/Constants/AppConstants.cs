using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Application.Common.Constants
{
    /// <summary>
    /// String Constants for usage in the app
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// URL to the Apply Solutions website
        /// </summary>
        public static string WebsiteUrl => "https://www.apply-solutions.ch";

        /// <summary>
        /// Mail address for support
        /// </summary>
        public static string SupportMail => "mobile.support@apply-solutions.ch";


        /// <summary>
        /// URL to the GitHub repository
        /// </summary>
        public static string GitHubRepositoryUrl => "https://github.com/MoneyFox/MoneyFox";

        /// <summary>
        /// URL to the GitHub repository
        /// </summary>
        public static string TranslationProjectUrl => "https://crowdin.com/project/money-fox";

        /// <summary>
        /// URL to the Twitter AccountViewModel of the icon.
        /// </summary>
        public static string IconDesignerTwitterUrl => "https://twitter.com/vandert9";

        /// <summary>
        /// URL to the Twitter AccountViewModel of the icon.
        /// </summary>
        public static string GithubContributionUrl => "https://github.com/MoneyFox/MoneyFox/graphs/contributors";

        /// <summary>
        /// ID string for add payment shortcuts
        /// </summary>
        public static string AddPaymentId => "AddIncomeId";

        public static string LogFileName => "moneyfox.log";

        public static string MSAL_APPLICATION_ID => "00a3e4cd-b4b0-4730-be62-5fcf90a94a1d";

    }
}
