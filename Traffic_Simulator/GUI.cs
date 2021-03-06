﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Traffic_Simulator
{
    public partial class GUI : Form
    {

        /// <summary>
        /// Controller element of the application.
        /// </summary>
        private SimulationController _controller = new SimulationController();
        private List<PictureBox> _elements = new List<PictureBox>();
        private PictureBox[,] _gui_slots = new PictureBox[4, 3];
        private List<PictureBox> _mergings = new List<PictureBox>();
        private PictureBox _p;
        private Crossing _copiedCrossing = null;
        private string _selectedSlot = "";
        private TextBox[] _crossingProperties = new TextBox[20];
        private bool _isReady = true, _checkTextBoxes;
        private Type _draggedCrossingType;
        private SimulationController Controller
        {
            get
            {
                return _controller;
            }
        }

        public string SelectedSlot 
        {
            get { return _selectedSlot; }
            set 
            {
                if (value != "")//selected another crossing and didnt make any changes to the previous one
                {
                    Controller.selectCrossing(value);                    
                }

                if (value == "" )//unselected crossing and didnt make any changes to it
                {
                    displayCrossingSettings(null);
                }

                _selectedSlot = value;
            } 
        }

        /// <summary>
        /// Weather GUI is ready to be refreshed
        /// </summary>
        public bool IsReady
        {
            get { return _isReady; }
            set { _isReady = value; }
        }

        /// <summary>
        /// Checks if each textbox has been modified and saves modifications
        /// </summary>
        /// <returns>
        /// Weather all textboxes have valid values
        /// </returns>
        private bool CheckTextBoxes()
        {
            _checkTextBoxes = true;

            foreach (TextBox tb in _crossingProperties)
            {
                if (tb.Modified)
                {
                    textBox_Leaving(tb, null);
                }

                if (_checkTextBoxes == false)
                    return false;
            }

            return _checkTextBoxes;
        }

        /// <summary>
        /// Starts browse file dialog (save or open)
        /// </summary>
        /// <param name="n"> 0 = "open a file" --- 1 = "save a file"</param>
        /// <returns>Path browsed if user clicked "save"</returns>
        /// <returns>Empty string (null) if user clicked "cancel"</returns>
        public string filePath(int n)
        {

            if (n == 0)//shows open file dialog
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {//if user clicks "ok" in the file dialog
                    return openFileDialog1.FileName;
                }
            }

            if (n == 1)//shows save file dialog
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {//if user clicks "ok" in the file dialog
                    return saveFileDialog1.FileName;
                }
            }

            return "";
        }

        /// <summary>
        /// Method called by controller when user closes program and there are unsaved modifications.
        /// </summary>
        /// <param name="title">Title of the message box</param>
        /// <param name="message">Message shown in the message box</param>
        /// <returns>True if user clics OK; False if user clicks CANCEL</returns>
        public string saveMessage(string title, string message)
        {
            DialogResult d = MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel);
            if (d == DialogResult.Yes)
            {
                if (CheckTextBoxes())
                    return d.ToString();//if users clicks "ok"
                else
                    return label1.Text;
            }
            else
            {
                return d.ToString();//if users clicks "ok"
            }
        }

        /// <summary>
        /// Displays all the info of a crossing on the right pane
        /// </summary>
        /// <param name="c">crossing whose properties will be displayed</param>
        public void displayCrossingSettings(Crossing c)
        {
            if (c != null)
            {                
                //if its stopped, enables textboxes, otherwise disables them
                foreach (TextBox tb in _crossingProperties)
                {
                    tb.Enabled = _controller.State == State.Stopped;
                    if(tb.Enabled)
                        tb.BackColor = Color.White;
                    else
                        tb.BackColor = TextBoxBase.DefaultBackColor;
                }

                if (c.GetType() == typeof(Crossing_1))
                {
                    Crossing_1 cr1 = (Crossing_1)c;
                    label5.Text = "N-->SW  and  S-->NE";
                    label6.Text = "N-->E      and  S-->W";

                    textBoxGLT1.Text = cr1.LightNtoWS._greenLightTime.ToString();
                    textBoxGLT2.Text = cr1.LightNtoE._greenLightTime.ToString();
                    textBoxGLT3.Text = cr1.LightWtoN._greenLightTime.ToString();
                    textBoxGLT4.Text = cr1.LightWtoSE._greenLightTime.ToString();
                    textBoxTF1.Text = cr1.FlowN.ToString();
                    textBoxTF2.Text = cr1.FlowE.ToString();
                    textBoxTF3.Text = cr1.FlowS.ToString();
                    textBoxTF4.Text = cr1.FlowW.ToString();
                    textBoxCTNE.Text = cr1.ProbNtoE.ToString();
                    textBoxCTNS.Text = cr1.ProbNtoS.ToString();
                    textBoxCTNW.Text = cr1.ProbNtoW.ToString();
                    textBoxCTEN.Text = cr1.ProbEtoN.ToString();
                    textBoxCTES.Text = cr1.ProbEtoS.ToString();
                    textBoxCTEW.Text = cr1.ProbEtoW.ToString();
                    textBoxCTSN.Text = cr1.ProbStoN.ToString();
                    textBoxCTSE.Text = cr1.ProbStoE.ToString();
                    textBoxCTSW.Text = cr1.ProbStoW.ToString();
                    textBoxCTWN.Text = cr1.ProbWtoN.ToString();
                    textBoxCTWE.Text = cr1.ProbWtoE.ToString();
                    textBoxCTWS.Text = cr1.ProbWtoS.ToString();
                }

                if (c.GetType() == typeof(Crossing_2))
                {
                    Crossing_2 cr2 = (Crossing_2)c;
                    label5.Text = "Pedestrians";
                    label6.Text = "N-->S      and  S-->N";


                    textBoxGLT1.Text = cr2.LightPedestrian._greenLightTime.ToString();
                    textBoxGLT2.Text = cr2.LightNtoS._greenLightTime.ToString();
                    textBoxGLT3.Text = cr2.LightWtoN._greenLightTime.ToString();
                    textBoxGLT4.Text = cr2.LightWtoSE._greenLightTime.ToString();
                    textBoxTF1.Text = cr2.FlowN.ToString();
                    textBoxTF2.Text = cr2.FlowE.ToString();
                    textBoxTF3.Text = cr2.FlowS.ToString();
                    textBoxTF4.Text = cr2.FlowW.ToString();
                    textBoxCTNE.Text = cr2.ProbNtoE.ToString();
                    textBoxCTNS.Text = cr2.ProbNtoS.ToString();
                    textBoxCTNW.Text = cr2.ProbNtoW.ToString();
                    textBoxCTEN.Text = cr2.ProbEtoN.ToString();
                    textBoxCTES.Text = cr2.ProbEtoS.ToString();
                    textBoxCTEW.Text = cr2.ProbEtoW.ToString();
                    textBoxCTSN.Text = cr2.ProbStoN.ToString();
                    textBoxCTSE.Text = cr2.ProbStoE.ToString();
                    textBoxCTSW.Text = cr2.ProbStoW.ToString();
                    textBoxCTWN.Text = cr2.ProbWtoN.ToString();
                    textBoxCTWE.Text = cr2.ProbWtoE.ToString();
                    textBoxCTWS.Text = cr2.ProbWtoS.ToString();
                }
            }
            else//if no crossing is selected
            {
                foreach (TextBox tb in _crossingProperties)
                {
                    if (tb.Focused)
                        buttonShowHideGLT.Focus();
                    tb.Text = "";
                    tb.Enabled = false;
                    tb.BackColor = TextBoxBase.DefaultBackColor;
                }
            }
            foreach (TextBox tb in _crossingProperties)
            {
                tb.Modified = false;
            }
        }

        /// <summary>
        /// Re-draws everything on the screen with updated values
        /// </summary>
        /// <param name="copyOfGrid">Grid from which the information will be extracted</param>
        public void refreshScreen(Grid copyOfGrid)
        {
            if (copyOfGrid == null)
                return;

            _isReady = false;
            foreach (PictureBox pb in _elements)
            {

                Controls.Remove(pb);
            }

            _elements.Clear();

            if(_controller.State==State.Stopped)
                drawCrossings(copyOfGrid.Slots);

            if (Controller.State != State.Stopped)
            {
                drawLights(copyOfGrid.Slots);//draws lights

                foreach (Car c in copyOfGrid.ListOfCars) //moves every existing car by 1 position
                {
                    if (c != null)//&& c.HasExitedGrid==false && c.HasEnteredGrid==true)
                    {
                        drawCar(c);//draws lights
                    }
                }
            }

            Invalidate();
            _isReady = true;

        }

        private void drawCrossings(Crossing[,] slots)
        {
            SelectedSlot = "";//clears selected slot

            //clears all mergings
            foreach (PictureBox pb in _mergings )
            {
                pb.Click -= slot_click;
                Controls.Remove(pb);
            }
            _mergings.Clear();

            foreach (PictureBox pb in _gui_slots)
            {
                pb.Image = null;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Crossing c = slots[i, j];
                    int x, y;
                    x = pictureBoxSlotA0.Location.X + (3 * 66 * i);
                    y = pictureBoxSlotA0.Location.Y + (3 * 66 * j);
                    if (c != null)
                    {
                        if (c.GetType() == typeof(Crossing_1))
                            _gui_slots[i, j].Image = Properties.Resources.Traffic_Simulator_Crossing_1;
                        else
                            _gui_slots[i, j].Image = Properties.Resources.Traffic_Simulator_Crossing_2;

                        _gui_slots[i, j].BorderStyle = BorderStyle.None;
                        _gui_slots[i, j].SendToBack();

                        if ((i + 1) < 4 && slots[i + 1, j] != null)//check merging East
                        {
                            PictureBox pb = addElement(x + 2 * 66, y + 66, "mergingE");
                            pb.Tag = _gui_slots[i, j].Tag;
                            pb.Click += slot_click;
                            _mergings.Add(pb);
                        }
                        if ((i - 1) >= 0 && slots[i - 1, j] != null)//check merging West
                        {
                            PictureBox pb = addElement(x, y + 66, "mergingW");
                            pb.Tag = _gui_slots[i, j].Tag;
                            pb.Click += slot_click;
                            _mergings.Add(pb);
                        }
                        if ((j + 1) < 3 && slots[i, j + 1] != null && c.GetType() == typeof(Crossing_1))//check merging South
                        {
                            PictureBox pb = addElement(x + 66, y + 2 * 66, "mergingS");
                            pb.Tag = _gui_slots[i, j].Tag;
                            pb.Click += slot_click;
                            _mergings.Add(pb);
                        }
                        if ((j - 1) >= 0 && slots[i, j - 1] != null && c.GetType() == typeof(Crossing_1))//check merging North
                        {
                            PictureBox pb = addElement(x + 66, y, "mergingN");
                            pb.Tag = _gui_slots[i, j].Tag;
                            pb.Click += slot_click;
                            _mergings.Add(pb);
                        }

                    }
                }
            }
            foreach (PictureBox pb in _mergings)
                _elements.Remove(pb);
        }

        private void drawLights(Crossing[,] slots)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Crossing c = slots[i, j];
                    int x, y;
                    x = pictureBoxSlotA0.Location.X + (3 * 66 * i);
                    y = pictureBoxSlotA0.Location.Y + (3 * 66 * j); //base values
                    string str = "";


                    if (c != null)
                    {

                        if (c.LightEtoNW._color != Color.Gray)//if lights are NOT disabled
                        {
                            addElement(x + 2 * 66, y + 66 + 6, c.LightEtoNW._color.ToString()); //add LightEtoNW
                            addElement(x + 2 * 66, y + 66 + 6 + 22, c.LightEtoS._color.ToString()); //add LightEtoS
                            addElement(x + 66 - 6, y + 2 * 66 - 14 - 22, c.LightWtoN._color.ToString()); //add LightWtoN
                            addElement(x + 66 - 6, y + 2 * 66 - 14, c.LightWtoSE._color.ToString()); //add LightWtoSE
                        }

                    }



                    if (c != null && c.GetType() == typeof(Crossing_1))
                    {
                        Crossing_1 c1 = (Crossing_1)c;
                        if (c.LightEtoNW._color != Color.Gray)//if lights are NOT disabled
                        {
                            //draw the following lights:
                            //LightStoEN, LightStoW, LightNtoE, LightNtoWS
                            addElement(x + 2 * 66 - 14, y + 2 * 66, c1.LightStoEN._color.ToString());//LightStoEN
                            addElement(x + 2 * 66 - 14 - 22, y + 2 * 66, c1.LightStoW._color.ToString());//LightStoW
                            addElement(x + 66 + 22 - 14, y + 66 - 6, c1.LightNtoWS._color.ToString());//LightNtoWS
                            addElement(x + 2 * 66 - 14 - 22, y + 66 - 6, c1.LightNtoE._color.ToString());//LightNtoE
                        }

                    }
                    if (c != null && c.GetType() == typeof(Crossing_2))
                    {
                        Crossing_2 c2 = (Crossing_2)c;
                        if (c2.LightEtoS._color != Color.Gray)//if lights are not disabled
                        {
                            str = "ped" + c2.LightPedestrian._color.ToString();
                            addElement(x + 66 - 14, y + 2 * 22, str);
                            addElement(x + 2 * 66 + 4, y + 2 * 22, str);
                            addElement(x + 66 - 14, y + 2 * 66 + 15, str);
                            addElement(x + 2 * 66 + 4, y + 2 * 66 + 15, str);
                            addElement(x + 2 * 66 - 14, y + 2 * 66, c2.LightStoN._color.ToString());//LightStoN
                            addElement(x + 66 + 22 - 14, y + 66 - 6, c2.LightNtoS._color.ToString());//LightNtoS
                        }
                    }
                }
            }
        }

        private PictureBox addElement(int x, int y, string image)
        {
            _p = new PictureBox();
            switch (image)
            {
                case "car North":
                    _p.Image = Properties.Resources.carN;
                    break;

                case "car East":
                    _p.Image = Properties.Resources.carE;
                    break;

                case "car West":
                    _p.Image = Properties.Resources.carW;
                    break;

                case "car South":
                    _p.Image = Properties.Resources.carS;
                    break;

                case "Color [Red]":
                    _p.Image = Properties.Resources.redLight;
                    break;

                case "Color [Green]":
                    _p.Image = Properties.Resources.greenLight;
                    break;

                case "mergingN":
                    _p.Image = Properties.Resources.mergingN;
                    break;

                case "mergingE":
                    _p.Image = Properties.Resources.mergingE;
                    break;

                case "mergingS":
                    _p.Image = Properties.Resources.mergingS;
                    break;

                case "mergingW":
                    _p.Image = Properties.Resources.mergingW;
                    break;

                case "Traffic_Simulator.Crossing_1":
                    _p.Image = Properties.Resources.Traffic_Simulator_Crossing_1;
                    break;

                case "Traffic_Simulator.Crossing_2":
                    _p.Image = Properties.Resources.Traffic_Simulator_Crossing_2;
                    break;

                case "pedColor [Green]":
                    _p.Image = Properties.Resources.pedColor__Green_;
                    break;

                case "pedColor [Red]":
                    _p.Image = Properties.Resources.pedColor__Red_;
                    break;

            }
            //_p.Image = new Bitmap(image + ".png");
            _p.Location = new Point(x, y);
            _p.SizeMode = PictureBoxSizeMode.AutoSize;
            _p.Show();
            this.Controls.Add(_p);
            _p.BringToFront();
            _elements.Add(_p);
            return _p;
        }

        private void drawCar(Car c)
        {
            if (c != null && c.HasEnteredGrid && !c.HasExitedGrid) //initial check
            {
                int x = pictureBoxSlotA0.Location.X + (int)(c.Crossing.ID[0] - 'A') * 3 * 66;
                int y = pictureBoxSlotA0.Location.Y + (int)(c.Crossing.ID[1] - '0') * 3 * 66;

                switch (c.Street.Position)
                {
                    case Direction.North:
                        x += 6 + 66 + 22 * c.StreetIndex[0];
                        y += 6 + 22 * c.StreetIndex[1];
                        break;
                    case Direction.West:
                        x += 6 + 22 * c.StreetIndex[1];
                        y += 66 * 2 - 6 - 22 * c.StreetIndex[0] - 10;
                        break;
                    case Direction.South:
                        x += 66 * 2 - 6 - 22 * c.StreetIndex[0] - 10;
                        y += 66 * 3 - 6 - 22 * c.StreetIndex[1] - 10;
                        break;
                    case Direction.East:
                        x += 66 * 3 - 6 - 22 * c.StreetIndex[1] - 10;
                        y += 66 + 6 + 22 * c.StreetIndex[0];
                        break;
                    case Direction.Center:
                        x += 66 + 6 + 22 * c.StreetIndex[0];
                        y += 66 + 6 + 22 * c.StreetIndex[1];
                        break;
                }
                addElement(x, y, "car " + c.Direction.ToString());
            }
        }

        public GUI() 
        { 
            InitializeComponent();
            _controller.Gui = this;

            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.label2, "Time in seconds for which the traffic light will stay green");

            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.label3, "Number of cars that will enter the crossing");

            System.Windows.Forms.ToolTip ToolTip3 = new System.Windows.Forms.ToolTip();
            ToolTip3.SetToolTip(this.label4, "Chance of a car to make a turn");



            //add pictureboxes to array
            _gui_slots[0, 0] = pictureBoxSlotA0;
            _gui_slots[0, 1] = pictureBoxSlotA1;
            _gui_slots[0, 2] = pictureBoxSlotA2;
            _gui_slots[1, 0] = pictureBoxSlotB0;
            _gui_slots[1, 1] = pictureBoxSlotB1;
            _gui_slots[1, 2] = pictureBoxSlotB2;
            _gui_slots[2, 0] = pictureBoxSlotC0;
            _gui_slots[2, 1] = pictureBoxSlotC1;
            _gui_slots[2, 2] = pictureBoxSlotC2;
            _gui_slots[3, 0] = pictureBoxSlotD0;
            _gui_slots[3, 1] = pictureBoxSlotD1;
            _gui_slots[3, 2] = pictureBoxSlotD2;

            //add textboxes to array
            _crossingProperties[0] = textBoxGLT1;
            _crossingProperties[1] = textBoxGLT2;
            _crossingProperties[2] = textBoxGLT3;
            _crossingProperties[3] = textBoxGLT4;
            _crossingProperties[4] = textBoxTF1;
            _crossingProperties[5] = textBoxTF2;
            _crossingProperties[6] = textBoxTF3;
            _crossingProperties[7] = textBoxTF4;
            _crossingProperties[8] = textBoxCTNE;
            _crossingProperties[9] = textBoxCTNS;
            _crossingProperties[10] = textBoxCTNW;
            _crossingProperties[11] = textBoxCTEN;
            _crossingProperties[12] = textBoxCTES;
            _crossingProperties[13] = textBoxCTEW;
            _crossingProperties[14] = textBoxCTSN;
            _crossingProperties[15] = textBoxCTSE;
            _crossingProperties[16] = textBoxCTSW;
            _crossingProperties[17] = textBoxCTWN;
            _crossingProperties[18] = textBoxCTWE;
            _crossingProperties[19] = textBoxCTWS;

            // allow crossings to be dragged out of sidebar
            this.Crossing_1.AllowDrop = true;
            this.Crossing_2.AllowDrop = true;

            // allow crossings to be dragged onto slots
            foreach (PictureBox pb in _gui_slots)
            {
                pb.AllowDrop = true;
            }
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            FormClosing += closeToolStripMenuItem_Click; //subscribe close-button method to close "X"
            displayCrossingSettings(null);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
                label1.Text = _controller.save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
                label1.Text = _controller.saveAs();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.load();
            if (label1.Text == "Simulation has been loaded")
            {
                foreach (TextBox tb in _crossingProperties)
                    tb.Modified = false;

                _controller.timerHasTriggered(null, null);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ToolStripMenuItem))//if this method was called by the menu bar
            {
                this.Close();
            }
            else 
            { 
                label1.Text = _controller.close();
                if (label1.Text == "" || label1.Text == "Error: Make sure the value is a positive whole number.")
                {
                    ((CancelEventArgs)e).Cancel = true;
                }
            }
        }
             
        private void buttonStartPause_Click(object sender, EventArgs e)// start/pause button click method
        {
            if (CheckTextBoxes())
            {
                if (_controller.State != State.Running) //if simulation is not running
                {
                    label1.Text = _controller.startSimulation();

                    if (label1.Text == "")//if started sucsessfully
                    {
                        label1.Text = "Simulation is running...";
                        buttonStartPause.Text = "ll";
                        buttonStartPause.TextAlign = ContentAlignment.MiddleCenter;
                        buttonStop.Enabled = true;
                        stopToolStripMenuItem.Enabled = true;
                        startToolStripMenuItem.Enabled = false;
                        pauseToolStripMenuItem.Enabled = true;

                        buttonClear.Enabled = false;
                        buttonDelete.Enabled = false;

                        fileToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                        insertToolStripMenuItem.Enabled = false;
                    }

                    //locks crossings properties
                    if (SelectedSlot != "")
                        _controller.selectCrossing(SelectedSlot);

                    return;     //leave method
                }


                if (_controller.State == State.Running) //if simulation is running
                {
                    label1.Text = _controller.pauseSimulation();
                    if (label1.Text == "")
                    {

                        label1.Text = "Simulation is paused.";
                        buttonStartPause.Text = "►";
                        buttonStartPause.TextAlign = ContentAlignment.MiddleLeft;

                        buttonStop.Enabled = true;

                        stopToolStripMenuItem.Enabled = true;
                        startToolStripMenuItem.Enabled = true;
                        pauseToolStripMenuItem.Enabled = false;


                        buttonClear.Enabled = false;
                        buttonDelete.Enabled = false;

                        fileToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                        insertToolStripMenuItem.Enabled = false;
                    }

                    //locks crossings properties
                    if (SelectedSlot != "")
                        _controller.selectCrossing(SelectedSlot);

                    return;
                }
            }

        }

        public void buttonStop_Click(object sender, EventArgs e)// stop button click method
        {
            label1.Text = _controller.stopSimulation();
            if (_controller.State == State.Stopped && label1.Text=="") {
                buttonStartPause.Text = "►";
                buttonStartPause.TextAlign = ContentAlignment.MiddleLeft;
                
                stopToolStripMenuItem.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;

                buttonStop.Enabled = false;

                buttonClear.Enabled = true;
                buttonDelete.Enabled = true;

                fileToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
                insertToolStripMenuItem.Enabled = true;

                if (sender == null)
                    label1.Text = "End of simulation.";
            }

            if(SelectedSlot!="")
                _controller.selectCrossing(SelectedSlot);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (_controller.State == State.Stopped)
            {
                _controller.clearGrid();
                _controller.timerHasTriggered(null, null);
                foreach (PictureBox pb in _mergings)
                {
                    Controls.Remove(pb);
                }
                _mergings.Clear();

                foreach (PictureBox pb in _gui_slots)
                {
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.Image = null;
                    pb.BringToFront();
                }
                SelectedSlot = "";
                label1.Text = "";
            }          
        }

        private void buttonShowHideGLT_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
            {
                int height = panel3.Size.Height;
                if (buttonShowHideGLT.Text == "Hide")
                {
                    buttonShowHideGLT.Text = "Show";
                    panel3.Visible = false;
                    label3.Location = new Point(label3.Location.X, label3.Location.Y - height);
                    buttonShowHideTF.Location = new Point(buttonShowHideTF.Location.X, buttonShowHideTF.Location.Y - height);
                    panel4.Location = new Point(panel4.Location.X, panel4.Location.Y - height);
                    label4.Location = new Point(label4.Location.X, label4.Location.Y - height);
                    buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y - height);
                    panel5.Location = new Point(panel5.Location.X, panel5.Location.Y - height);

                    panel5.Size = new Size(151, 437);

                }
                else
                {

                    if (buttonShowHideTF.Text == "Show")
                        panel5.Size = new Size(151, 437);
                    else
                        panel5.Size = new Size(151, 333);

                    buttonShowHideGLT.Text = "Hide";
                    panel3.Visible = true;
                    label3.Location = new Point(label3.Location.X, label3.Location.Y + height);
                    buttonShowHideTF.Location = new Point(buttonShowHideTF.Location.X, buttonShowHideTF.Location.Y + height);
                    panel4.Location = new Point(panel4.Location.X, panel4.Location.Y + height);
                    label4.Location = new Point(label4.Location.X, label4.Location.Y + height);
                    buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y + height);
                    panel5.Location = new Point(panel5.Location.X, panel5.Location.Y + height);
                }
            }
        }

        private void buttonShowHideTF_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
            {
                int height = panel4.Size.Height;

                if (buttonShowHideTF.Text == "Hide")
                {

                    buttonShowHideTF.Text = "Show";
                    panel4.Visible = false;
                    label4.Location = new Point(label4.Location.X, label4.Location.Y - height);
                    buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y - height);
                    panel5.Location = new Point(panel5.Location.X, panel5.Location.Y - height);
                    panel5.Size = new Size(151, 437);
                }
                else
                {
                    if (buttonShowHideGLT.Text == "Show")
                        panel5.Size = new Size(151, 437);
                    else
                        panel5.Size = new Size(151, 333);

                    buttonShowHideTF.Text = "Hide";
                    panel4.Visible = true;
                    label4.Location = new Point(label4.Location.X, label4.Location.Y + height);
                    buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y + height);
                    panel5.Location = new Point(panel5.Location.X, panel5.Location.Y + height);
                }
            }
        }

        private void buttonShowHideCT_Click(object sender, EventArgs e)
        {

            if (CheckTextBoxes())
            {
                if (buttonShowHideCT.Text == "Hide")
                {
                    buttonShowHideCT.Text = "Show";
                    panel5.Visible = false;

                    if (buttonShowHideGLT.Text == "Show" || buttonShowHideTF.Text == "Show")
                        panel5.Size = new Size(151, 437);
                    else
                        panel5.Size = new Size(151, 333);

                }
                else
                {
                    buttonShowHideCT.Text = "Hide";
                    panel5.Visible = true;
                }
            }
        }

        private void crossing_mouseDown(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Name == "Crossing_1")
                _draggedCrossingType = typeof(Crossing_1);
            else
                _draggedCrossingType = typeof(Crossing_2);

            ((PictureBox)sender).DoDragDrop((PictureBox)sender, DragDropEffects.Copy);
        }

        private void slot_DragDrop(object sender, DragEventArgs e)
        {
            // get dragged slot's ID
            string slotID = ((PictureBox)sender).Name.Substring(((PictureBox)sender).Name.Length - 2);

            // check if slot is empty
            if (this.Controller.slotIsAvailable(slotID))
            {
                this.label1.Text = this.Controller.addCrossing(slotID, _draggedCrossingType);
                _controller.timerHasTriggered(null, null);
            }
            else
            {
                Console.WriteLine("Cannot add crossing");
                //Console.WriteLine("Slot " + slotID + " is occupied");
            }
        }

        private void slot_DragEnter(object sender, DragEventArgs e)
        {
            string slotID = ((PictureBox)sender).Tag.ToString();
            if (this.Controller.slotIsAvailable(slotID))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void slot_click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
            {
                string clickedElement = ((Control)sender).Tag.ToString();
                PictureBox picbox = _gui_slots[((int)clickedElement[0]) - (int)'A', Convert.ToInt32(clickedElement[1].ToString())];

                if (_copiedCrossing != null)//if there is an object to be pasted
                {
                    if (Controller.slotIsAvailable(picbox.Tag.ToString()))
                    {
                        label1.Text = Controller.addCrossing(picbox.Tag.ToString(), _copiedCrossing.GetType());
                        if (label1.Text == "")
                        {
                            int[] values = new int[20];

                            if (_copiedCrossing.GetType() == typeof(Crossing_1))
                            {
                                Crossing_1 c1 = (Crossing_1)_copiedCrossing;

                                values[0] = c1.LightNtoWS._greenLightTime;
                                values[1] = c1.LightStoW._greenLightTime;
                            }

                            if (_copiedCrossing.GetType() == typeof(Crossing_2))
                            {
                                Crossing_2 c2 = (Crossing_2)_copiedCrossing;

                                values[0] = c2.LightPedestrian._greenLightTime;
                                values[1] = c2.LightStoN._greenLightTime;
                            }

                            values[2] = _copiedCrossing.LightWtoN._greenLightTime;
                            values[3] = _copiedCrossing.LightWtoSE._greenLightTime;
                            values[4] = _copiedCrossing.FlowN;
                            values[5] = _copiedCrossing.FlowE;
                            values[6] = _copiedCrossing.FlowS;
                            values[7] = _copiedCrossing.FlowW;
                            values[8] = (int)_copiedCrossing.ProbNtoE;
                            values[9] = (int)_copiedCrossing.ProbNtoS;
                            values[10] = (int)_copiedCrossing.ProbNtoW;
                            values[11] = (int)_copiedCrossing.ProbEtoN;
                            values[12] = (int)_copiedCrossing.ProbEtoS;
                            values[13] = (int)_copiedCrossing.ProbEtoW;
                            values[14] = (int)_copiedCrossing.ProbStoN;
                            values[15] = (int)_copiedCrossing.ProbStoE;
                            values[16] = (int)_copiedCrossing.ProbStoW;
                            values[17] = (int)_copiedCrossing.ProbWtoN;
                            values[18] = (int)_copiedCrossing.ProbWtoE;
                            values[19] = (int)_copiedCrossing.ProbWtoS;

                            Controller.setCrossingProperties(picbox.Tag.ToString(), values);
                            _copiedCrossing = null;
                            Controller.timerHasTriggered(null, null);
                        }
                    }
                    else
                    {
                        label1.Text = "Please select an available slot.";
                    }
                }
                else//if there is no object to be pasted
                {
                    //if clicked on a (not null) crossing
                    if (picbox.Image != null)
                    {
                        //if clicked on an unselected slot
                        if (clickedElement != SelectedSlot)
                        {
                            //unselect pevious slot
                            if (SelectedSlot != "")
                            {
                                _gui_slots[((int)SelectedSlot[0]) - (int)'A', Convert.ToInt32(SelectedSlot[1].ToString())].BorderStyle = BorderStyle.None;
                                _gui_slots[((int)SelectedSlot[0]) - (int)'A', Convert.ToInt32(SelectedSlot[1].ToString())].SendToBack();
                            }

                            //selects clicked slot
                            _gui_slots[((int)picbox.Tag.ToString()[0]) - (int)'A', Convert.ToInt32(picbox.Tag.ToString()[1].ToString())].BorderStyle = BorderStyle.Fixed3D;
                            _gui_slots[((int)picbox.Tag.ToString()[0]) - (int)'A', Convert.ToInt32(picbox.Tag.ToString()[1].ToString())].BringToFront();

                            foreach (PictureBox pb in _mergings)
                            {
                                if (pb.Tag.ToString() == picbox.Tag.ToString())
                                    pb.BringToFront();
                            }

                            SelectedSlot = clickedElement;//selects and displays crossing info
                        }
                        else //clicked on an already selected crossing
                        {
                            _gui_slots[((int)picbox.Tag.ToString()[0]) - (int)'A', Convert.ToInt32(picbox.Tag.ToString()[1].ToString())].BorderStyle = BorderStyle.None;
                            _gui_slots[((int)SelectedSlot[0]) - (int)'A', Convert.ToInt32(SelectedSlot[1].ToString())].SendToBack();

                            SelectedSlot = "";
                        }
                    }
                }
                label1.Text = "";
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedSlot != ""&& _controller.State==State.Stopped)
            {
                if(_controller.removeCrossing(SelectedSlot)=="ok") 
                {
                List<PictureBox> removeList = new List<PictureBox>();

                for (int i = 0; i < _mergings.Count;i++ )
                {
                    if (_mergings[i].Tag.ToString() == SelectedSlot)
                    {
                        removeList.Add(_mergings[i]);
                    }
                }
                foreach (PictureBox pb in removeList)
                {
                    Controls.Remove(pb);
                    _mergings.Remove(pb);
                }

                int a = ((int)SelectedSlot[0]) - (int)'A';
                int b = Convert.ToInt32(SelectedSlot[1].ToString());
                _gui_slots[a, b].BorderStyle = BorderStyle.FixedSingle;
                SelectedSlot = "";
                _controller.timerHasTriggered(null, null);
                Invalidate();
                }
            }
            
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
            {
                if (SelectedSlot == "")
                {
                    label1.Text = "Please select a crossing first.";
                }
                else
                {
                    _copiedCrossing = ObjectCopier.Clone<Crossing>(Grid.getCrossing(SelectedSlot));
                    label1.Text = "Selected crossing has been copied.\nPlease select an available slot to paste.";
                }
            }

        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckTextBoxes())
            {
                if (SelectedSlot == "")
                {
                    label1.Text = "Please select a crossing first.";
                }
                else
                {
                    _copiedCrossing = ObjectCopier.Clone<Crossing>(Grid.getCrossing(SelectedSlot));
                    Controller.removeCrossing(SelectedSlot);
                    _gui_slots[((int)SelectedSlot[0]) - (int)'A', Convert.ToInt32(SelectedSlot[1].ToString())].BorderStyle = BorderStyle.FixedSingle;
                    _selectedSlot = "";
                    Controller.timerHasTriggered(null, null);


                    label1.Text = "Selected crossing has been cut.\nPlease select an available slot to paste.";
                }
            }
        }

        private void addCrossing1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Please select an available slot.";
            _copiedCrossing = new Crossing_1("XX");
        }

        private void addCrossing2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Please select an available slot.";
            _copiedCrossing = new Crossing_2("XX");
        }

        private void textBox_Leaving(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Modified)
            {
                try
                {
                    int value = 0;
                    if (tb.Text != "")//empty strings are not allowed to be values
                        value = Convert.ToInt32(tb.Text);
                    if (value < 0)
                        throw (new FormatException());

                    label1.Text = Controller.setCrossingProperty(SelectedSlot, sender);
                    tb.BackColor = Color.White;
                    tb.Modified = false;
                }
                catch
                {
                    label1.Text = "Error: Make sure the value is a positive whole number.";
                    tb.BackColor = Color.DarkSalmon;
                    tb.Modified = true;
                    _checkTextBoxes = false;
                    tb.Focus();
                }
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                textBox_Leaving(sender, e);
                tb.Modified = true;
                _checkTextBoxes = false;
                if (label1.Text != "Error: Make sure the value is a positive whole number.")
                    SendKeys.Send("{TAB}");
            }
            else
            {
                tb.Modified = true;
                _checkTextBoxes = false;
            }
        }
    }
}
