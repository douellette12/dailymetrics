using System;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using json = Newtonsoft.Json;
using System.Text;
using System.Net.Mail;
using CareConnectHuddle1.Models;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using OfficeOpenXml.Drawing.Chart;

namespace CareConnectHuddle
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);
        public static string Get_digest(CareConnectReportTeam team)
        {
            string context_info_url = $"{team.LocationHuddleBoard}/contextinfo";
            var context_info_request = new HttpRequestMessage(HttpMethod.Post, context_info_url);
            context_info_request.Headers.Add("Accept", "application/json;odata=verbose");
            var context_info_response = httpClient.SendAsync(context_info_request);
            var content_info_response_string = context_info_response.Result.Content.ReadAsStringAsync().Result;
            var context_info_response_json = json.Linq.JObject.Parse(content_info_response_string);
            string form_digest_value = context_info_response_json["d"]["GetContextWebInformation"]["FormDigestValue"].ToString();
            return form_digest_value;
        }
        private static void PostToSP(CareConnectReportTeam team, string filename, string inc_or_wo)
        {
            if (inc_or_wo == "INC")
            {
                string url = $"{team.LocationHuddleBoard}/Web/Lists/getbytitle('Drivers')/RootFolder/Files/add(url='{filename}',overwrite=true)";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Accept", "application/json;odata=verbose");
                request.Headers.Add("X-RequestDigest", Get_digest(team));
                request.Content = new StreamContent(File.OpenRead(team.LocationIncident));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                _ = httpClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            }
            else if (inc_or_wo == "WO")
            {
                string url = $"{team.LocationHuddleBoard}/Web/Lists/getbytitle('Drivers')/RootFolder/Files/add(url='{filename}',overwrite=true)";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Accept", "application/json;odata=verbose");
                request.Headers.Add("X-RequestDigest", Get_digest(team));
                request.Content = new StreamContent(File.OpenRead(team.LocationWorkOrder));
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                _ = httpClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            }
        }
        private static string PopulateBody(string inc, StringBuilder incSb, string wo, StringBuilder woSb, StringBuilder woBreachSB)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("Views/email.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{team.INCStorageLocation}", inc);
            body = body.Replace("{incSb}", incSb.ToString());
            body = body.Replace("{team.WOStorageLocation}", wo);
            body = body.Replace("{woSB}", woSb.ToString());
            body = body.Replace("{woBreachSB}", woBreachSB.ToString());
            return body;
        }
        private static void SendEmail(string body, IQueryable<TeamMember> team)
        {
            SmtpClient smtp = new SmtpClient("smtp.mailserver.org");
            smtp.UseDefaultCredentials = true;
            MailMessage message = new MailMessage();
            MailAddress from = new MailAddress("ITSDataDelivery@company.org");

            foreach (var email in team.Select(x => x.EmailAddress))
            {
                message.To.Add(email.ToString());
            }
            message.From = from;
            message.Subject = $"{DateTime.Now.ToString("MMMM dd, yyyy")} | BMC Daily Drivers | {team.First().SupportGroup}";
            message.IsBodyHtml = true;
            message.Body = body;
            smtp.Send(message);
        }
        private static (StringBuilder, StringBuilder, StringBuilder) BuildTableStrings(IQueryable<INCNumbers> incData, IQueryable<WONumbers> woData, IQueryable<WorkOrderSummary> woBreachData)
        {
            StringBuilder incSb = new StringBuilder();
            foreach (var item in incData)
            {
                if (item.ReportDate == DateTime.Now.Date)
                {
                    incSb.Append("<tr>" +
                        "<td>" + item.ReportDate.ToShortDateString() + "</td>" +
                        "<td>" + item.AssignedGroup + "</td>" +
                        "<td>" + item.IncidentsStillOpen +
                        "<td>" + item.TotalBreached + "</td>" +
                        "<td>" + Math.Round(item.IncidentBreachPercentage * 100, 0) + "%" + "</td>" +
                        "</tr>");
                }

            }

            StringBuilder woSB = new StringBuilder();
            foreach (var item in woData)
            {
                if (item.ReportDate == DateTime.Now.Date)
                {
                    woSB.Append("<tr>" +
                        "<td>" + item.ReportDate.ToShortDateString() + "</td>" +
                        "<td>" + item.AssignedGroup + "</td>" +
                        "<td>" + item.WorkOrdersStillOpen + "</td>" +
                        "<td>" + item.WorkOrdersOlder60Days + "</td>" +
                        "<td>" + Math.Round(item.WOBreachPercentage * 100, 0) + "%" + "</td>" +
                        "</tr>");
                }
            }
            StringBuilder woBreachSB = new StringBuilder();

            if (woBreachData.Any())
            {
                woBreachSB.Append("<h4>Work Orders - New Breaches</h4>" +
                    "<table>" +
                    "<tr>" +
                    "<th>WorkOrderID</th>" +
                    "<th>AssignedGroup</th>" +
                    "<th>Summary</th>" +
                    "<th>DaysOld</th></tr>");
                foreach (var item in woBreachData)
                {
                    woBreachSB.Append(
                        "<tr>" +
                        "<td><a href=\"serverurl" + item.WorkOrderId + "\">" + item.WorkOrderId + "</a></td>" +
                        "<td>" + item.AssignedGroup + "</td>" +
                        "<td>" + item.Summary + "</td>" +
                        "<td>" + item.DaysOld + "</td>" +
                        "</tr>");
                }
                woBreachSB.Append("</table>");
            }

            else
            {
                woBreachSB.Append("<h4>No new work order breaches</h4>");
            }
            return (incSb, woSB, woBreachSB);
        }

        public static void Main(string[] args)
        {
            var token = GetToken();
#pragma warning disable CA1416 // Validate platform compatibility
            WindowsIdentity.RunImpersonated(token, () =>
            {
                IQueryable<INCNumbers> incsum;
                IQueryable<WONumbers> wosum;
                IQueryable<WorkOrderSummary> wobreach;
                IQueryable<CareConnectReportTeam> teams;
                teams = GetTeamInfo();
                ITSDataContext db = new ITSDataContext();
                foreach (var team in teams)
                {
                    var people = (
                        from c in db.ActiveDirectoryRecords
                        join b in db.CareConnectReportPeople on c.EmployeeId equals b.EmployeeId
                        join d in db.CareConnectReportTeams on b.TeamId equals d.TeamId
                        where d.TeamId == team.TeamId
                        select new TeamMember()
                        {
                            SupportGroup = d.SupportGroup,
                            EmailAddress = c.EmailAddress
                        });
                    StringBuilder inctable, wotable, wobreachtable;
                    (incsum, wosum, wobreach) = ReportQuery(team);
                    (inctable, wotable, wobreachtable) = BuildTableStrings(incsum, wosum, wobreach);
                    string body = PopulateBody(team.LocationIncident, inctable, team.LocationWorkOrder, wotable, wobreachtable);
                    SendEmail(body, people);
                    WriteExcel(incsum, team, "INC");
                    WriteExcel(wosum, team, "WO");
                    var INCfilename = team.LocationIncident.Split('\\').Last();
                    var WOfilename = team.LocationWorkOrder.Split('\\').Last();
                    PostToSP(team, INCfilename, "INC");
                    PostToSP(team, WOfilename, "WO");
                }
            });
        }

        public static SafeAccessTokenHandle GetToken()
        { 
            SafeAccessTokenHandle safeTokenHandle;
            string userName, domainName, passwd;
            // Get the user token for the specified user, domain, and password using the unmanaged LogonUser method.
            domainName = "company.org";
            userName = "appUsername";
            passwd = "secret";
            const int LOGON32_PROVIDER_DEFAULT = 0;
            //This parameter causes LogonUser to create a primary token.
            const int LOGON32_LOGON_INTERACTIVE = 2;

            // Call LogonUser to obtain a handle to an access token.
            bool returnValue = LogonUser(userName, domainName, passwd,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                out safeTokenHandle);
            return safeTokenHandle;
        }

        public static IQueryable<CareConnectReportTeam> GetTeamInfo()
        {

            {
                ITSDataContext db = new();
                var teams = (
                from c in db.CareConnectReportTeams
                select new CareConnectReportTeam()
                {
                    TeamId = c.TeamId,
                    SupportGroup = c.SupportGroup,
                    LocationIncident = c.LocationIncident,
                    LocationWorkOrder = c.LocationWorkOrder,
                    LocationHuddleBoard = c.LocationHuddleBoard
                });
                return teams;
            }
                //IQueryable<CareConnectReportTeam> team;
#pragma warning disable CA1416 // Validate platform compatibility
                /*team = WindowsIdentity.RunImpersonated(token, () =>
                {*/
                
            /*});
            return team;*/
#pragma warning restore CA1416 // Validate platform compatibility
        }

        public static (IQueryable<INCNumbers>, IQueryable<WONumbers>, IQueryable<WorkOrderSummary>) ReportQuery(CareConnectReportTeam team)
        {
            SafeAccessTokenHandle token;
            token = GetToken();
            IQueryable<INCNumbers> incResults;
            IQueryable<WONumbers> woResults;
            IQueryable<WorkOrderSummary> woBreaches;

#pragma warning disable CA1416 // Validate platform compatibility
            /*(incResults, woResults, woBreaches) = WindowsIdentity.RunImpersonated(token, () =>
            {*/
                ITSDataContext db = new ITSDataContext();
                incResults = (
                     from c in db.DailySummaries
                     join b in db.DailySummaryBreeches on c.ReportDate equals b.ReportDate
                     where c.ReportDate.Year == DateTime.Now.Year &&
                                     c.ReportDate.Month == DateTime.Now.Month &&
                                     c.AssignedGroup == team.SupportGroup &&
                                     b.AssignedGroup == team.SupportGroup &&
                                     b.ReportDate.Year == DateTime.Now.Year &&
                                     b.ReportDate.Month == DateTime.Now.Month
                     select new INCNumbers()
                     {
                         ReportDate = c.ReportDate,
                         AssignedGroup = c.AssignedGroup,
                         IncidentsStillOpen = c.IncidentsStillOpen,
                         TotalBreached = (int)(b.HighBreech + b.MediumBreech + b.LowBreech),
                         IncidentBreachPercentage = (double)(b.HighBreech + b.MediumBreech + b.LowBreech) / (double)c.IncidentsStillOpen
                     });

                woResults = (
                    from c in db.DailySummaries
                    where c.ReportDate.Year == DateTime.Now.Year &&
                                        c.ReportDate.Month == DateTime.Now.Month &&
                                        c.AssignedGroup == team.SupportGroup
                    select new WONumbers()
                    {
                        ReportDate = c.ReportDate,
                        AssignedGroup = c.AssignedGroup,
                        WorkOrdersStillOpen = c.WorkOrdersStillOpen,
                        WorkOrdersOlder60Days = c.WorkOrdersOlder60Days,
                        WOBreachPercentage = (double)c.WorkOrdersOlder60Days / (double)c.WorkOrdersStillOpen
                    });

                woBreaches = (
                    from c in db.WorkOrderSummaries
                    where c.CurrentStatus != "Closed" &&
                    c.AssignedGroup == team.SupportGroup
                    select new WorkOrderSummary()
                    {
                        WorkOrderId = c.WorkOrderId,
                        AssignedGroup = c.AssignedGroup,
                        Summary = c.Summary,
                        DaysOld = c.DaysOld
                    });

                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    woBreaches = (from c in woBreaches where (c.DaysOld == 61 || c.DaysOld == 62 || c.DaysOld == 63) select c);
                }
                else
                {
                    woBreaches = (from c in woBreaches where c.DaysOld == 61 select c);
                }
                return (incResults, woResults, woBreaches);
            /*});*/
#pragma warning restore CA1416 // Validate platform compatibility
            /*return (incResults, woResults, woBreaches);*/
        }

        public static void WriteExcel(IQueryable<dynamic> data, CareConnectReportTeam team, string queryType)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string ws_title = DateTime.Now.ToString("MMM").ToUpper() + "'" + DateTime.Now.ToString("yy");
            string filename;
            string column3;
            string column4;
            string column5;
            if (queryType == "INC")
            {
                filename = team.LocationIncident;
                column3 = "IncidentsStillOpen";
                column4 = "TotalBreached";
                column5 = "IncBreachPercentage";

            }
            else
            {
                filename = team.LocationWorkOrder;
                column3 = "WorkOrdersStillOpen";
                column4 = "WorkOrdersOlder60Days";
                column5 = "WOBreachPercentage";
            }
            FileInfo fi = new FileInfo(filename);
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ReportDate", typeof(DateTime));
                dataTable.Columns.Add("AssignedGroup", typeof(string));
                dataTable.Columns.Add(column3, typeof(int));
                dataTable.Columns.Add(column4, typeof(int));
                dataTable.Columns.Add(column5, typeof(double));

                foreach (var item in data)
                {
                    if (queryType == "INC")
                    {
                        dataTable.Rows.Add(
                            item.ReportDate, 
                            item.AssignedGroup, 
                            item.IncidentsStillOpen,
                            item.TotalBreached,
                            item.IncidentBreachPercentage);
                    }
                    else
                    {
                        dataTable.Rows.Add(
                            item.ReportDate,
                            item.AssignedGroup,
                            item.WorkOrdersStillOpen,
                            item.WorkOrdersOlder60Days,
                            item.WOBreachPercentage);
                    }
                   
                }

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == ws_title);

                if (worksheet != null)
                {
                    excelPackage.Workbook.Worksheets.Delete(worksheet);
                    worksheet = excelPackage.Workbook.Worksheets.Add(ws_title);
                }
                else
                {
                    worksheet = excelPackage.Workbook.Worksheets.Add(ws_title);
                }
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true, OfficeOpenXml.Table.TableStyles.Light13);
                var goalcell = worksheet.Cells["A" + (DateTime.Now.Day + 2).ToString()];
                goalcell.Value = "Goal";
                goalcell.Style.Font.Bold = true;
                var fifty = worksheet.Cells["E" + (DateTime.Now.Day + 2).ToString()];
                fifty.Value = 0.5;
                var goalrange = worksheet.Cells["A" + (DateTime.Now.Day + 2).ToString() + ":" + "E" + (DateTime.Now.Day + 2).ToString()];
                goalrange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                goalrange.Style.Fill.BackgroundColor.SetColor(ExcelIndexedColor.Indexed13);
                worksheet.Column(1).Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Column(5).Style.Numberformat.Format = "0%";
                worksheet.Cells.AutoFitColumns();
                ExcelLineChart linechart = worksheet.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;
                linechart.Title.Text = DateTime.Now.ToString("Y").ToUpper();
                linechart.Title.Font.Size = 18;
                var percentrange = worksheet.Cells["E2:E" + (DateTime.Now.Day + 1).ToString()];
                var daterange = worksheet.Cells["A2:A" + (DateTime.Now.Day + 1).ToString()];
                var goal = fifty;
                var headers = worksheet.Cells["A1:E1"];
                headers.Style.Font.Bold = true;
                linechart.XAxis.Title.Text = "Date";
                linechart.RoundedCorners = false;
                linechart.YAxis.Title.Text = "% Breached";
                linechart.SetSize(1000, 500);
                linechart.SetPosition(3, 0, 6, 0);
                var dataseries = linechart.Series.Add(percentrange, daterange);
                //var goalseries = linechart.Series.Add(goal, daterange);
                linechart.Marker = true;
                dataseries.Header = team.SupportGroup;
                var marker = dataseries.Marker;
                marker.Style = eMarkerStyle.Circle;
                excelPackage.Save();
            }
        }
    }
}
