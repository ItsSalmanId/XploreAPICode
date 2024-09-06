﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOX.DataModels.Models.QualityAsuranceModel
{
    public class QADashboardSearch : BaseModel
    {
        public string CALL_HANDLING_ID { get; set; }
        public string TEAMS_NAMES { get; set; }
        public string CALL_TYPE { get; set; }
        public string EMPLOYEE_USER_NAME { get; set; }
        public string USER_FULL_NAME { get; set; }
        public string EVALUATION_NAME { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string TIME_FRAME { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
    }

    public class QADashboardData : BaseModel
    {
        public int CLIENT_GREATER_THAN { get; set; }
        public int CLIENT_LESS_THAN { get; set; }
        public int SYSTEM_GREATER_THAN { get; set; }
        public int SYSTEM_LESS_THAN { get; set; }
        public int GREATER_THAN { get; set; }
        public int LESS_THAN { get; set; }
        public int TOTAL_COUNT { get; set; }
        public int SURVEY_CLIENT_GREATER_THAN { get; set; }
        public int SURVEY_CLIENT_LESS_THAN { get; set; }
        public int SURVEY_SYS_GREATER_THAN { get; set; }
        public int SURVEY_SYS_LESS_THAN { get; set; }
    }

    public class LineGraphData : BaseModel
    {
        public int WEEK_NUMBER { get; set; }
        public string DATE_RANGE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
        public long EVALUATION_PERCENTAGE { get; set; }
        public String TEAM_NAME { get; set; }
        public string AGENT_NAME { get; set; }
        public series[] series { get; set; }
    }

    public class series
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<long> data { get; set; }
    }

    public class DashBoardMainModel : BaseModel
    {
        public QADashboardData PieChartData { get; set; }
        public LineGraphResponseModel LineChartData { get; set; }
    }

    public class LineGraphResponseModel : BaseModel
    {
        public LineGraphResponseModel()
        {
            series = new List<series>();
        }
        public List<LineGraphData> lineGraphData { get; set; }
        public List<series> series { get; set; }
        public List<string> dateRanges { get; set; }
    }
}
