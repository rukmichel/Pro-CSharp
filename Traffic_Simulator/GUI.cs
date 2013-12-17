using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Traffic_Simulator
{
    public partial class GUI : Form
    {
       // private delegate void drawCarHandler(Car c);
       // private delegate void drawLightsHandler(Crossing[,] slots);
        /// <summary>
        /// Controller element of the application.
        /// </summary>
        private SimulationController _controller = new SimulationController();
        private List<PictureBox> _elements = new List<PictureBox>();
        private PictureBox[,] _gui_slots = new PictureBox[4, 3];
        private List<PictureBox> _mergings = new List<PictureBox>();
        private PictureBox _p;
        public bool _isReady = true;

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
                        //_gui_slots[i,j].Image = new Bitmap(c.GetType().ToString()+".png");
                        _gui_slots[i,j].BorderStyle=BorderStyle.None;
                        _gui_slots[i, j].SendToBack();

                        if ((i + 1) < 4 && slots[i + 1, j] != null)//check merging East
                        {
                            _mergings.Add(addElement(x + 2 * 66, y + 66, "mergingE"));
                        }
                        if ((i - 1) >= 0 && slots[i - 1, j] != null)//check merging West
                        {
                            _mergings.Add(addElement(x, y + 66, "mergingW"));
                        }
                        if ((j + 1) < 3 && slots[i, j + 1] != null && c.GetType() == typeof(Crossing_1))//check merging South
                        {
                            _mergings.Add(addElement(x + 66, y + 2 * 66, "mergingS"));
                        }
                        if ((j - 1) >= 0 && slots[i, j - 1] != null && c.GetType() == typeof(Crossing_1))//check merging North
                        {
                            _mergings.Add(addElement(x + 66, y, "mergingN"));
                        }
                        
                    }                    
                }
            }
            foreach (PictureBox pb in _mergings)
                _elements.Remove(pb);
        }

        private void drawLights(Crossing[,] slots)
        {
           // addElement(pictureBox1.Location.X + 66 - 13, pictureBox1.Location.Y+66, "ped-red-light.png");
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
                        //draw the following lights:
                        //c.LightEtoNW, c.LightEtoS, c.LightWtoN, c.LightWtoSE

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
                //MessageBox.Show("Street Location: " + c.Street.Position.ToString() + "\nIndex: " + c.StreetIndex[0] + " " + c.StreetIndex[0]);
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
            e.Effect = DragDropEffects.Copy;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonClear_Click_1(object sender, EventArgs e)
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
        } 
    }
}
