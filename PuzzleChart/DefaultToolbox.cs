﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using PuzzleChart.Api.Interfaces;

namespace PuzzleChart
{
    class DefaultToolbox : ToolStrip, IToolBox
    {
        private ITool activeTool;

        public ITool active_tool
        {
            get
            {
                return this.activeTool;
            }
            set
            {
                this.active_tool = value;
            }
        }

        public event ToolSelectedEventHandler tool_selected;

        public void AddSeparator()
        {
            this.Items.Add(new ToolStripSeparator());
        }

        public void AddTool(ITool tool)
        {
            Debug.WriteLine(tool.Name + " is added into toolbox.");

            if (tool is ToolStripButton)
            {
                ToolStripButton toggleButton = (ToolStripButton)tool;

                if (toggleButton.CheckOnClick)
                {
                    toggleButton.CheckedChanged += toggleButton_CheckedChanged;
                }

                this.Items.Add(toggleButton);
            }
        }

        public void Register(IPlugin plugin)
        {
            if (plugin != null)
            {
                Debug.WriteLine("Opening plugin: " + plugin.Name);

                if (plugin is ITool)
                {
                    ITool tool = (ITool)plugin;
                    AddTool(tool);
                }
            }
        }

        public void RemoveTool(ITool tool)
        {
            foreach (ToolStripItem i in this.Items)
            {
                if (i is ITool)
                {
                    if (i.Equals(tool))
                    {
                        this.Items.Remove(i);
                    }
                }
            }
        }

        private void toggleButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                ToolStripButton button = (ToolStripButton)sender;

                if (button.Checked)
                {
                    if (button is ITool)
                    {
                        this.activeTool = (ITool)button;
                        Debug.WriteLine(this.activeTool.Name + " is activated.");

                        if (tool_selected != null)
                        {
                            tool_selected(this.activeTool);
                        }

                        UncheckInactiveToggleButtons();
                    }
                    else
                    {
                        throw new InvalidCastException("The tool is not an instance of ITool.");
                    }
                }
            }
        }

        private void UncheckInactiveToggleButtons()
        {
            foreach (ToolStripItem item in this.Items)
            {
                if (item != this.activeTool)
                {
                    if (item is ToolStripButton)
                    {
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
        }
    }
}
