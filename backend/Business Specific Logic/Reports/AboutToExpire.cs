using BusinessSpecificLogic.EF;
using Reusable;
using Reusable.Reports;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dapper;
using System;

namespace BusinessSpecificLogic.Reports
{
    public class AboutToExpire : Report, IReport
    {
        public AboutToExpire(string Owner) : base(Owner)
        {
        }

        public LoggedUser loggedUser { get; set; }

        public override string fileName => "About to Expire.xlsx";

        protected override void define()
        {
            using (var ctx = new TrainingContext())
            {
                #region DATASET

                var data = ctx.Database.Connection.Query("select * from vw_AllTrainings")
                    .Where(e => e.DateExpiresAt < DateTimeOffset.Now.AddDays(15))
                    .Select(e => new
                    {
                        e.Name,
                        e.LastName,
                        e.MotherLastName,
                        e.ClockNumber,
                        e.JobPosition,
                        e.Area,
                        e.Shift,
                        e.Supervisor,
                        e.Score,
                        e.ScoreNotes,
                        //e.TrainingCreatedAt,
                        e.DateProgrammed,
                        e.DateStart,
                        e.DateEnd,
                        e.DateCertification,
                        e.DateExpiresAt,
                        e.Trainer,
                        e.InternalExternal,
                        e.TrainingNotes,
                        //e.QuickTraining,
                        e.DurationInHours,
                        e.Certification,
                        e.ILUO,
                        e.AppliesToDC3,
                        e.LifecycleInMonths,
                        IsExpired = e.DateExpiresAt < DateTimeOffset.Now
                    })
                    .ToList();

                #endregion

                #region DOCUMENT
                InitWorkBook("About to Expire");
                #endregion

                CreateWorkSheet("About to Expire");
                InsertHeader("About to Expire");

                #region Detail
                CurrentCell().LoadFromCollection(data);
                ws.Cells[3, 1, CurrentRow() - 1, 23].AutoFilter = true;
                #endregion

                #region Layout
                ConfigLayout();
                #endregion
            }
        }

        private void InsertHeader(string title)
        {
            CurrentRow(1);
            InsertTitle(title);
            NewLine(2);

            InsertLabel("Employee Name", AlignMode: TextAlign.CENTER);
            InsertLabel("Employee Last Name", AlignMode: TextAlign.CENTER);
            InsertLabel("Employee Second Last Name", AlignMode: TextAlign.CENTER);
            InsertLabel("Clock Number", AlignMode: TextAlign.CENTER);
            InsertLabel("Job Position", AlignMode: TextAlign.CENTER);
            InsertLabel("Area", AlignMode: TextAlign.CENTER);
            InsertLabel("Shift", AlignMode: TextAlign.CENTER);
            InsertLabel("Supervidor", AlignMode: TextAlign.LEFT);
            InsertLabel("Score", AlignMode: TextAlign.LEFT);
            InsertLabel("Score Notes", AlignMode: TextAlign.LEFT);
            InsertLabel("Programmed", AlignMode: TextAlign.LEFT);
            InsertLabel("Start", AlignMode: TextAlign.LEFT);
            InsertLabel("End", AlignMode: TextAlign.LEFT);
            InsertLabel("Date Certification", AlignMode: TextAlign.LEFT);
            InsertLabel("Expires At", AlignMode: TextAlign.LEFT);
            InsertLabel("Trainer", AlignMode: TextAlign.LEFT);
            InsertLabel("Internal / External", AlignMode: TextAlign.LEFT);
            InsertLabel("Training Notes", AlignMode: TextAlign.LEFT);
            InsertLabel("Duration In Hours", AlignMode: TextAlign.LEFT);
            InsertLabel("Certification", AlignMode: TextAlign.LEFT);
            InsertLabel("ILUO", AlignMode: TextAlign.LEFT);
            InsertLabel("Applies to DC3", AlignMode: TextAlign.LEFT);
            InsertLabel("Lifecycle In Months", AlignMode: TextAlign.LEFT);
            InsertLabel("Is Expired", AlignMode: TextAlign.LEFT);
            NewLine();
        }

        private void ConfigLayout()
        {
            ws.Column(1).Width = 20;    //A
            ws.Column(2).Width = 15;    //B
            ws.Column(3).Width = 15;    //C
            ws.Column(4).Width = 15;    //D
            ws.Column(5).Width = 20;    //E
            ws.Column(6).Width = 10;    //F
            ws.Column(7).Width = 25;    //G
            ws.Column(8).Width = 30;    //H
            ws.Column(9).Width = 15;    //I
            ws.Column(10).Width = 8;    //J

            ws.Column(11).Width = 15;   //K
            ws.Column(11).Style.Numberformat.Format = "dd-mmm-yyyy";

            ws.Column(12).Width = 15;   //L
            ws.Column(12).Style.Numberformat.Format = "dd-mmm-yyyy";

            ws.Column(13).Width = 15;   //M
            ws.Column(13).Style.Numberformat.Format = "dd-mmm-yyyy";

            ws.Column(14).Width = 15;   //N
            ws.Column(14).Style.Numberformat.Format = "dd-mmm-yyyy";

            ws.Column(15).Width = 15;   //O
            ws.Column(15).Style.Numberformat.Format = "dd-mmm-yyyy";

            ws.Column(16).Width = 25;   //P
            ws.Column(17).Width = 9;    //Q
            ws.Column(18).Width = 12;   //R
            ws.Column(19).Width = 12;   //S
            ws.Column(20).Width = 40;   //T
            ws.Column(21).Width = 8;    //U
            ws.Column(22).Width = 8;    //V
            ws.Column(23).Width = 9;    //W
            ws.Column(24).Width = 9;    //X
        }
    }
}
