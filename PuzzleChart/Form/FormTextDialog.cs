using PuzzleChart.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleChart.Form
{
    public partial class FormTextDialog : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container components;
        private Button button1;
        private Button button2;
        private DataGrid myDataGrid;
        private DataTable myDataTable;
        private PuzzleObject obj;

        public PuzzleObject PuzzleObj
        {
            get
            {
                return this.obj;
            }
            set
            {
                this.obj = value;
            }
        }

        public FormTextDialog(PuzzleObject obj2)
        {
            this.obj = obj2;
            InitializeComponent();
            AddDataTable();
        }

        private void InitializeComponent()
        {
            
            if (obj is Line == false)
            {
                if (obj is Parallelogram)
                {
                    Parallelogram obj3 = (Parallelogram)obj;

                    myDataTable = obj3.table.Copy();
                    // Create the form and its controls.
                    this.components = new System.ComponentModel.Container();
                    this.button1 = new Button();
                    this.myDataGrid = new DataGrid();
                    this.Text = "Attribute " + obj3.text;

                    this.ClientSize = new Size(450, 330);

                    button1.Location = new Point(24, 16);
                    button1.Size = new Size(120, 24);
                    button1.Text = "Save";
                    button1.Click += new System.EventHandler(GetDataTable);


                    myDataGrid.Location = new Point(24, 50);
                    myDataGrid.Size = new Size(250, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(button2);
                    this.Controls.Add(myDataGrid);
                }
                else if(obj is Shapes.Rectangle)
                {
                    Shapes.Rectangle obj3 = (Shapes.Rectangle)obj;

                    myDataTable = obj3.table.Copy();
                    // Create the form and its controls.
                    this.components = new System.ComponentModel.Container();
                    this.button1 = new Button();
                    this.myDataGrid = new DataGrid();
                    this.Text = "Attribute " + obj3.text;

                    this.ClientSize = new Size(450, 330);

                    button1.Location = new Point(24, 16);
                    button1.Size = new Size(120, 24);
                    button1.Text = "Save";
                    button1.Click += new System.EventHandler(GetDataTable);


                    myDataGrid.Location = new Point(24, 50);
                    myDataGrid.Size = new Size(250, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(button2);
                    this.Controls.Add(myDataGrid);
                }
                else if (obj is Diamond)
                {
                    Diamond obj3 = (Diamond)obj;

                    myDataTable = obj3.table.Copy();
                    // Create the form and its controls.
                    this.components = new System.ComponentModel.Container();
                    this.button1 = new Button();
                    this.myDataGrid = new DataGrid();
                    this.Text = "Attribute " + obj3.text;

                    this.ClientSize = new Size(450, 330);

                    button1.Location = new Point(24, 16);
                    button1.Size = new Size(120, 24);
                    button1.Text = "Save";
                    button1.Click += new System.EventHandler(GetDataTable);


                    myDataGrid.Location = new Point(24, 50);
                    myDataGrid.Size = new Size(336, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(button2);
                    this.Controls.Add(myDataGrid);
                }
            }
        }

        private void AddDataTable()
        {
            myDataGrid.DataSource = myDataTable.Copy();
        }

        public void GetDataTable(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)myDataGrid.DataSource;

            if (obj is Line == false)
            {
                if (obj is Parallelogram)
                {

                    Parallelogram obj3 = (Parallelogram)obj;
                    obj3.table = dataTable.Copy();
                }
                else if (obj is Shapes.Rectangle)
                {

                    Shapes.Rectangle obj3 = (Shapes.Rectangle)obj;
                    obj3.table = dataTable.Copy();
                }
                else if (obj is Diamond)
                {

                    Diamond obj3 = (Diamond)obj;
                    obj3.table = dataTable.Copy();
                }
            }
        }

    }
}
