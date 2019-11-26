using BusinessSpecificLogic.EF;
using Reusable;
using Reusable.Reports;
using System.Linq;
using Dapper;

namespace BusinessSpecificLogic.Reports
{
    public class Headcount : Report, IReport
    {
        public Headcount(string Owner) : base(Owner)
        {
        }

        public LoggedUser loggedUser { get; set; }

        public override string fileName => "Headcount.xlsx";

        protected override void define()
        {
            using (var ctx = new TrainingContext())
            {
                #region DATASET

                var entities = ctx.Database.Connection.Query("select * from vw_Headcount where name is not null")
                    .Select(e => new
                    {
                        e.Shift,
                        e.Area,
                        e.JobPosition,
                        e.Supervisor,
                        e.ClockNumber,
                        e.Name,
                        e.LastName,
                        e.MotherLastName,
                        e.CURP,
                        e.HireDate
                    })
                    .ToList();

                #endregion

                #region DOCUMENT
                InitWorkBook("Headcount");
                #endregion

                CreateWorkSheet("Headcount");
                InsertHeader("Headcount");

                #region Detail
                CurrentCell().LoadFromCollection(entities);
                ws.Cells[3, 1, CurrentRow() - 1, 10].AutoFilter = true;
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

            InsertLabel("Shift", AlignMode: TextAlign.CENTER);
            InsertLabel("Area", AlignMode: TextAlign.CENTER);
            InsertLabel("Job Position", AlignMode: TextAlign.CENTER);
            InsertLabel("Supervisor", AlignMode: TextAlign.CENTER);
            InsertLabel("Clock Number", AlignMode: TextAlign.CENTER);
            InsertLabel("Name", AlignMode: TextAlign.CENTER);
            InsertLabel("Last Name", AlignMode: TextAlign.CENTER);
            InsertLabel("Second Last Name", AlignMode: TextAlign.CENTER);
            InsertLabel("CURP", AlignMode: TextAlign.LEFT);
            InsertLabel("Hire Date", AlignMode: TextAlign.LEFT);
            NewLine();
        }

        private void ConfigLayout()
        {
            ws.Column(1).Width = 25;                //A
            ws.Column(2).Width = 10;                //B
            ws.Column(3).Width = 20;                //C
            ws.Column(4).Width = 25;                //D
            ws.Column(5).Width = 15;                //E
            ws.Column(6).Width = 22;                //F
            ws.Column(7).Width = 15;                //G
            ws.Column(8).Width = 20;                //H
            ws.Column(9).Width = 20;                //I
            ws.Column(10).Width = 15;               //J
            ws.Column(10).Style.Numberformat.Format = "dd-mmm-yyyy";
        }
    }
}
