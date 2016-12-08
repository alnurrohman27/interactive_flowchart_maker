using PuzzleChart.Api.Shapes;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PuzzleChart.Api.Forms
{
    public partial class FormTextDialog : Form
    {
        private System.ComponentModel.Container components;
        private Button button1;
        private DataGrid myDataGrid;
        private DataTable myDataTable;
        private PuzzleObject obj;
        private TextBox boxName;

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
            if (obj2 is Oval == false && obj2 is Line == false)
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
                    button1.Click += new EventHandler(GetDataTable);

                    boxName = new TextBox();
                    boxName.Location = new Point(24, 45);
                    boxName.Size = new Size(150, 24);
                    boxName.Text = obj3.text;

                    myDataGrid.Location = new Point(24, 75);
                    myDataGrid.Size = new Size(250, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(boxName);
                    this.Controls.Add(myDataGrid);
                }
                else if (obj is Api.Shapes.Rectangle)
                {
                    Api.Shapes.Rectangle obj3 = (Api.Shapes.Rectangle)obj;

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

                    boxName = new TextBox();
                    boxName.Location = new Point(24, 45);
                    boxName.Size = new Size(150, 24);
                    boxName.Text = obj3.text;


                    myDataGrid.Location = new Point(24, 75);
                    myDataGrid.Size = new Size(250, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(boxName);
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

                    boxName = new TextBox();
                    boxName.Location = new Point(24, 45);
                    boxName.Size = new Size(150, 24);
                    boxName.Text = obj3.text;

                    myDataGrid.Location = new Point(24, 75);
                    myDataGrid.Size = new Size(336, 200);
                    myDataGrid.CaptionText = "List Data";
                    //myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

                    DataColumn ID = new DataColumn("ID", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn value = new DataColumn("Value", typeof(int));

                    this.Controls.Add(button1);
                    this.Controls.Add(boxName);
                    this.Controls.Add(myDataGrid);
                }

                else if (obj is Oval)
                {
                    Oval obj3 = (Oval)obj;

                    // Create the form and its controls.
                    this.components = new System.ComponentModel.Container();
                    this.Text = "Attribute " + obj3.text;
                    this.ClientSize = new Size(450, 330);

                    this.button1 = new Button();
                    button1.Location = new Point(24, 40);
                    button1.Size = new Size(50, 24);
                    button1.Text = "Save";
                    button1.Click += new EventHandler(GetDataTable);

                    boxName = new TextBox();
                    boxName.Location = new Point(80, 42);
                    boxName.Size = new Size(80, 24);
                    boxName.Text = obj3.text;

                    this.Controls.Add(button1);
                    this.Controls.Add(boxName);
                }
            }
        }

        private void AddDataTable()
        {
            myDataGrid.DataSource = myDataTable.Copy();
        }

        public void GetDataTable(object sender, EventArgs e)
        {
            if (obj is Line == false)
            {
                if (obj is Parallelogram)
                {
                    DataTable dataTable = (DataTable)myDataGrid.DataSource;
                    Parallelogram obj3 = (Parallelogram)obj;
                    obj3.table = dataTable.Copy();
                    obj3.text = boxName.Text;
                }
                else if (obj is Api.Shapes.Rectangle)
                {
                    DataTable dataTable = (DataTable)myDataGrid.DataSource;
                    Api.Shapes.Rectangle obj3 = (Api.Shapes.Rectangle)obj;
                    obj3.table = dataTable.Copy();
                    obj3.text = boxName.Text;
                }
                else if (obj is Diamond)
                {
                    DataTable dataTable = (DataTable)myDataGrid.DataSource;
                    Diamond obj3 = (Diamond)obj;
                    obj3.table = dataTable.Copy();
                    obj3.text = boxName.Text;
                }
                else if (obj is Oval)
                {
                    Oval obj3 = (Oval)obj;
                    obj3.text = boxName.Text;
                }
            }
        }

    }
}
