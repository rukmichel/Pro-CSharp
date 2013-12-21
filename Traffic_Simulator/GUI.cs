using System;
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
        private string selectedSlot = "";
        public bool _isReady = true;
        private const int MinimumDragDistance = 4;
        private Point _origMousePoint;

        private SimulationController Controller
        {
            get
            {
                return _controller;
            }
        }

        private void drawCrossings(Crossing[,] slots)
        {
            for (int i = 0; i < _mergings.Count;i++ )
            {
                PictureBox pb = _mergings[i];
                _mergings.Remove(pb);
                Controls.Remove(pb);
            }
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
                            _gui_slots[i,j].Image = Properties.Resources.Traffic_Simulator_Crossing_1;
                        else 
                            _gui_slots[i,j].Image = Properties.Resources.Traffic_Simulator_Crossing_2;

                        _gui_slots[i,j].BorderStyle=BorderStyle.None;
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
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<3;j++)
                {
                    Crossing c = slots[i, j];
                    int x, y;
                    x = pictureBoxSlotA0.Location.X + (3*66*i); 
                    y = pictureBoxSlotA0.Location.Y+ (3*66*j); //base values
                    string str="";


                    if (c != null )
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
                        Crossing_1 c1 = (Crossing_1) c;
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
                        Crossing_2 c2 = (Crossing_2) c;
                        if (c2.LightEtoS._color!=Color.Gray)//if lights are not disabled
                        {
                            str = "ped" + c2.LightPedestrian._color.ToString();
                            addElement(x + 66 - 14, y + 2 * 22, str);
                            addElement(x + 2 * 66 + 4, y + 2 * 22, str);
                            addElement(x + 66 - 14, y + 2 * 66 + 15, str );
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
                case "car":
                    _p.Image = Properties.Resources.car;
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
                        x += 66 * 3 - 6 - 22 * c.StreetIndex[1]-10;
                        y += 66 + 6 + 22 * c.StreetIndex[0];
                        break;
                    case Direction.Center:
                        x += 66 + 6 + 22 * c.StreetIndex[0];
                        y += 66 + 6 + 22 * c.StreetIndex[1];
                        break;
                }
                addElement(x, y, "car");
            }
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
            return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel).ToString();//if users clicks "ok"
           
        }

        /// <summary>
        /// Displays all the info of a crossing on the right pane
        /// </summary>
        /// <param name="c">crossing whose properties will be displayed</param>
        public void displayCrossingSettings(Crossing c)
        {
            if (c != null)
            {
                if (c.GetType() == typeof(Crossing_1))
                {

                }
                if (c.GetType() == typeof(Crossing_2))
                {

                }
            }
            else
            {
                textBoxGLT1.Text = "";
                textBoxGLT2.Text = "";
                textBoxGLT3.Text = "";
                textBoxGLT4.Text = "";
                textBoxTF1.Text = "";
                textBoxTF3.Text = "";
                textBoxTF4.Text = "";
                textBoxTF2.Text = "";
                textBoxCTNE.Text = "";
                textBoxCTNS.Text = "";
                textBoxCTNW.Text = "";
                textBoxCTEN.Text = "";

                textBoxGLT1.Enabled = false;
                textBoxGLT2.Enabled = false;
                textBoxGLT3.Enabled = false;
                textBoxGLT4.Enabled = false;
                textBoxTF1.Enabled = false;
                textBoxTF3.Enabled = false;
                textBoxTF4.Enabled = false;
                textBoxTF2.Enabled = false;
                textBoxCTNE.Enabled = false;
                textBoxCTNS.Enabled = false;
                textBoxCTNW.Enabled = false;
                textBoxCTEN.Enabled = false;
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



        public GUI() 
        { 
            InitializeComponent();
            _controller.Gui = this;

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

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.saveAs();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.load();
            _controller.timerHasTriggered(null, null);
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
                if (label1.Text == "") 
                {
                    ((CancelEventArgs)e).Cancel = true;   
                }
            }
        }

             
        private void buttonStartPause_Click(object sender, EventArgs e)// start/pause button click method
        {
            
            if (_controller.State != State.Running) //if simulation is not running
            {
                label1.Text = _controller.startSimulation();

                if (label1.Text == "")
                {
                    buttonStartPause.Text = "ll";
                    buttonStartPause.TextAlign = ContentAlignment.MiddleCenter;
                    button2.Enabled = true;
                }                
                return;     //leave method
            }
            

            if(_controller.State == State.Running) //if simulation is running
            {
                label1.Text = _controller.pauseSimulation();
                if (label1.Text == "")
                {
                    buttonStartPause.Text = "►";
                    buttonStartPause.TextAlign = ContentAlignment.MiddleLeft;
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)// stop button click method
        {
            label1.Text = _controller.stopSimulation();
            if (label1.Text == "") {
                buttonStartPause.Text = "►";
                buttonStartPause.TextAlign = ContentAlignment.MiddleLeft;
                button2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e) // make change button method
        {
            _controller.setCrossingProperty(null, null); //just a simulation of having changed data
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Crossing c = new Crossing_2("C2");

            Car c2 = new Car(c);
           // MessageBox.Show("Direction:" + c2.Street.Position.ToString());
                    c2.StreetIndex[0] = 1;
                //c2.Direction = Direction.West;
                c2.Street = c.StreetW;
                c2.StreetIndex[1] = 0;
                c2.HasEnteredGrid = true;
                c2.HasExitedGrid = false;
                drawCar(c2);
          
        }

        private void buttonShowHideGLT_Click(object sender, EventArgs e)
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
             
            }
            else 
            {
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

        private void buttonShowHideTF_Click(object sender, EventArgs e)
        {
            int height = panel4.Size.Height;

            if (buttonShowHideTF.Text == "Hide")
            {
                buttonShowHideTF.Text = "Show";
                panel4.Visible = false;
                label4.Location = new Point(label4.Location.X, label4.Location.Y - height);
                buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y - height);
                panel5.Location = new Point(panel5.Location.X, panel5.Location.Y - height);
            }
            else
            {
                buttonShowHideTF.Text = "Hide";
                panel4.Visible = true;
                label4.Location = new Point(label4.Location.X, label4.Location.Y + height);
                buttonShowHideCT.Location = new Point(buttonShowHideCT.Location.X, buttonShowHideCT.Location.Y + height);
                panel5.Location = new Point(panel5.Location.X, panel5.Location.Y + height);
            }
        }

        private void buttonShowHideCT_Click(object sender, EventArgs e)
        {
            if (buttonShowHideCT.Text == "Hide")
            {
                buttonShowHideCT.Text = "Show";
                panel5.Visible = false;
                
            }
            else
            {
                buttonShowHideCT.Text = "Hide";
                panel5.Visible = true;
            }
        }

        Type draggedCrossingType;

        private void crossing_mouseDown(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).Name == "Crossing_1")
                draggedCrossingType = typeof(Crossing_1);
            else
                draggedCrossingType = typeof(Crossing_2);

            ((PictureBox)sender).DoDragDrop((PictureBox)sender, DragDropEffects.Copy);
        }

        private void slot_DragDrop(object sender, DragEventArgs e)
        {
            // get dragged slot's ID
            string slotID = ((PictureBox)sender).Name.Substring(((PictureBox)sender).Name.Length - 2);

            // check if slot is empty
            if (this.Controller.gridIsAvailable(slotID)&&_controller.State==State.Stopped)
            {
                this.label1.Text = this.Controller.addCrossing(slotID, draggedCrossingType);
                this.Controller.timerHasTriggered(null, null);
            }
            else
            {
                Console.WriteLine("Cannot add crossing");
                //Console.WriteLine("Slot " + slotID + " is occupied");
            }

            draggedCrossingType = null;
        }

        private void slot_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void slot_click(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox)sender;
            
            if (picbox.Image != null)
            {
                string str = selectedSlot;
                int a, b;

                //if ((selectedSlot == "") || (selectedSlot != picbox.Tag.ToString()))
                if(picbox.BorderStyle==BorderStyle.None)
                {
                    //unselect pevious slot
                    if (selectedSlot != "")
                    {
                        str = selectedSlot;
                        a = ((int)str[0]) - (int)'A';
                        b = Convert.ToInt32(str[1].ToString());
                        _gui_slots[a, b].BorderStyle = BorderStyle.None;
                    }

                    str = picbox.Tag.ToString();
                    a = ((int)str[0]) - (int)'A';
                    b = Convert.ToInt32(str[1].ToString());
                    _gui_slots[a, b].BorderStyle = BorderStyle.Fixed3D;

                    selectedSlot = picbox.Tag.ToString();
                }
                else
                {
                    str = picbox.Tag.ToString();
                    a = ((int)str[0]) - (int)'A';
                    b = Convert.ToInt32(str[1].ToString());
                    _gui_slots[a, b].BorderStyle = BorderStyle.None;
                }
            }
            // if there is no picturebox, and user wants to add a crossing
            else if(picbox.Image == null && this.draggedCrossingType != null)
            {
                string slotID = picbox.Name.Substring(picbox.Name.Length - 2);

                if (this.Controller.gridIsAvailable(slotID) && this.Controller.State == State.Stopped)
                {
                    this.label1.Text = this.Controller.addCrossing(slotID, draggedCrossingType);
                    this.Controller.timerHasTriggered(null, null);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (selectedSlot != ""&& _controller.State==State.Stopped)
            {
                if(_controller.removeCrossing(selectedSlot)=="ok") 
                {
                List<PictureBox> removeList = new List<PictureBox>();

                for (int i = 0; i < _mergings.Count;i++ )
                {
                    if (_mergings[i].Tag.ToString() == selectedSlot)
                    {
                        removeList.Add(_mergings[i]);
                    }
                }
                foreach (PictureBox pb in removeList)
                {
                    Controls.Remove(pb);
                    _mergings.Remove(pb);
                }

                _controller.timerHasTriggered(null, null);
                int a = ((int)selectedSlot[0]) - (int)'A';
                int b = Convert.ToInt32(selectedSlot[1].ToString());
                _gui_slots[a, b].BorderStyle = BorderStyle.FixedSingle;
                selectedSlot = "";
                Invalidate();
                }
            }
            
        }

        private void buttonClear_Click_1(object sender, EventArgs e)
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
                selectedSlot = "";
            }
        }

        private void MenuItemInsertCrossing1_Click(object sender, EventArgs e)
        {
            this.draggedCrossingType = typeof(Crossing_1);
            this.label1.Text = "Click to add Crossing1";
        }

        private void MenuItemInsertCrossing2_Click(object sender, EventArgs e)
        {
            this.draggedCrossingType = typeof(Crossing_2);
            this.label1.Text = "Click to add Crossing2";
        }

        private void slot_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
        //        e.UseDefaultCursors = false;
        //        Cursor.Current = new Cursor(((Bitmap)this.Crossing_1.Image).GetHicon());
            }
        } 
    }
}
