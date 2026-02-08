namespace SeizureTrackerService.Constants;

internal static class AppSettings
{
    internal const string SeizureTrackerSchema = "SeizureTrackerSchema";
    internal const string ServerDomain = "IdentityPasskey:ServerDomain";
}

internal static class ApiRoutes
{
    internal const string GetHeaders = "headers";
    internal const string GetDetailsByHeaderId = "details/{headerId}";
}

internal static class Tables
{
    internal const string SeizureActivityHeader = "SeizureActivityHeader";
    internal const string SeizureActivityDetail = "SeizureActivityDetail";
    internal const string WhiteList = "WhiteList";
}

internal static class Views
{
    internal const string GetManageLogsView = "vwGetManageLogHeaders";
}

internal static class Schema
{
    internal const string Dev = "dev";
    internal const string Prod = "st";
}

internal static class StoredProcedures
{
    internal const string DevGetActivityLogDetailsByHeaderId = "EXEC dev.GetActivityLogDetailsByHeaderId @HeaderId=";
    internal const string CheckWhiteListSprocDev = "EXEC dev.usp_IsEmailWhitelisted @Email, @IsAuthorized OUTPUT";
    internal const string CheckWhiteListSproc = "EXEC st.usp_IsEmailWhitelisted @Email, @IsAuthorized OUTPUT";
}