using System;
using PuzzleChart.Api.Interfaces;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using PuzzleChart.Api;
using PuzzleChart.Api.Shapes;
using System.IO;

namespace PuzzleChart.Commands
{
    public class SaveCommand : ICommand
    {
        private ICanvas canvas;
        private IEditor editor;
        public SaveCommand(ICanvas canvas, IEditor editor)
        {
            this.canvas = canvas;
            this.editor = editor;
        }
        public void Execute()
        {
            Debug.WriteLine("Save File is selected");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Interactive Puzzle Document (*.ipd)|*.ipd";
            saveFileDialog1.Title = "Save an Document";
            saveFileDialog1.ShowDialog();
            try
            {
                if (saveFileDialog1.FileName != "")
                {
                    string name = saveFileDialog1.FileName;

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    XmlWriter writer = XmlWriter.Create(name, settings);
                    writer.WriteStartDocument();
                    writer.WriteStartElement("puzzle_object");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();

                    foreach (PuzzleObject obj in canvas.GetAllObjects())
                    {
                        if (obj is IOpenSave)
                        {
                            if (obj is Diamond)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Diamond");
                                Diamond diamondObj = (Diamond)obj;
                                diamondObj.Serialize(name);
                            }
                            else if (obj is Api.Shapes.Rectangle)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Rectangle");
                                Api.Shapes.Rectangle rectangleObj = (Api.Shapes.Rectangle)obj;
                                rectangleObj.Serialize(name);
                            }
                            else if (obj is Oval)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Oval");
                                Oval ovalObj = (Oval)obj;
                                ovalObj.Serialize(name);
                            }
                            else if (obj is Parallelogram)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Parallelogram");
                                Parallelogram parallelogramObj = (Parallelogram)obj;
                                parallelogramObj.Serialize(name);
                            }
                            else if (obj is Line)
                            {
                                Debug.WriteLine("ID: " + obj.ID.ToString() + " Type: Line");
                                Line lineObj = (Line)obj;
                                lineObj.Serialize(name);
                            }

                        }
                    }
                    canvas.Saved = true;
                    canvas.Name = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: File being used by another process or corrupt");
                Debug.WriteLine("Error Message: " + ex);
            }

            DefaultEditor defEditor = (DefaultEditor)editor;
            defEditor.SelectedTab.Text = this.canvas.Name; 
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}
