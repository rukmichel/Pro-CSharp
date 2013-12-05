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
        private List<PictureBox> elements = new List<PictureBox>();
        private PictureBox p;


        private void drawLights(Crossing[,] slots)
        {
           // addElement(pictureBox1.Location.X + 66 - 13, pictureBox1.Location.Y+66, "ped-red-light.png");
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<3;j++)
                {
                    Crossing c = slots[i, j];
                    int x, y;
                    x = pictureBox1.Location.X + (3*66*i); 
                    y = pictureBox1.Location.Y+ (3*66*j); //base values
                    //x = pictureBox1.Location.X + 66 - 13; y = pictureBox1.Location.Y + 2 * 22 - 10;
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

        private void addElement(int x, int y, string image)
        {
            p = new PictureBox();
            p.Image = new Bitmap(image + ".png");
            p.Location = new Point(x, y);
            p.SizeMode = PictureBoxSizeMode.AutoSize;
            p.Show();
            this.Controls.Add(p);
            p.BringToFront();
            elements.Add(p);
        }

        private void drawCar(Car c)
        {
            int x=pictureBox1.Location.X, y=pictureBox1.Location.Y;            
            if(c.Crossing.GetType() == typeof(Crossing_2))
                x += 3*66;
            
            switch(c.Street.Position)
                {
                    case Direction.North:
                            x += 6 + 66 + 22 * c.StreetIndex[0];
                            y += 6 + 22 * c.StreetIndex[1];                         
                            break; 
                    case Direction.West:
                            x += 6 + 22 * c.StreetIndex[1];
                            y += 66 * 2 - 6 - 22 * c.StreetIndex[0] - 9;                         
                            break;   
                    case Direction.South:
                            x += 66 * 2 - 6 - 22 * c.StreetIndex[0] - 9;
                            y += 66 * 3 - 6 - 22 * c.StreetIndex[1] - 9;                         
                            break;
                    case Direction.East:
                            x += 66 * 3 - 6 - 22 * c.StreetIndex[1];
                            y += 66 + 6 + 22 * c.StreetIndex[0];                         
                            break;
                    case Direction.Center:
                            x += 66 + 6 + 22 * c.StreetIndex[0];
                            y += 66 + 6 + 22 * c.StreetIndex[1];                         
                            break;
            }
            addElement(x, y, "Bitmap1");
                //MessageBox.Show("Street Location: " + c.Street.Position.ToString() + "\nIndex: " + c.StreetIndex[0] + " " + c.StreetIndex[0]);

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

        public void refreshScreen(Grid copyOfGrid)
        {
            if (copyOfGrid == null)
                return;

                foreach (PictureBox pb in elements)
                {
                    Controls.Remove(pb);
                }

                elements.Clear();

                foreach (Car c in copyOfGrid.ListOfCars) //moves every existing car by 1 position
                {
                    if (c != null)//&& c.HasExitedGrid==false && c.HasEnteredGrid==true)
                    {
                        drawCar(c);//draws lights
                        //Invoke(new drawCarHandler(drawCar), new object[] { c });//draws cars in case another thread called it
                    }
                }

                 drawLights(copyOfGrid.Slots);//draws lights
                //Invoke(new drawLightsHandler(drawLights), new object[] { copyOfGrid.Slots });//draws lights in case another thread called it

                //MessageBox.Show("a");
                Invalidate();

        }



        public GUI() 
        { 
            InitializeComponent();
            _controller.Gui = this;
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

             
        private void button1_Click(object sender, EventArgs e)// start/pause button click method
        {
            
            if (_controller.State != State.Running) //if simulation is not running
            {
                label1.Text = _controller.startSimulation();

                if (label1.Text == "")
                {
                    button1.Text = "Pause";
                    button2.Enabled = true;
                }                
                return;     //leave method
            }
            

            if(_controller.State == State.Running) //if simulation is running
            {
                label1.Text = _controller.pauseSimulation();
                if (label1.Text == "")
                {
                    button1.Text = "Start";
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)// stop button click method
        {
            label1.Text = _controller.stopSimulation();
            if (label1.Text == "") {
                button1.Text = "Start";
                button2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e) // make change button method
        {
            _controller.setCrossingProperty(null, null); //just a simulation of having changed data
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Crossing c = new Crossing_2();
            c.ID = "A0";

            Car c2 = new Car();
            c2.Crossing = c;
           // MessageBox.Show("Direction:" + c2.Street.Position.ToString());
                    c2.StreetIndex[0] = 1;
                //c2.Direction = Direction.West;
                c2.Street = c.StreetW;
                c2.StreetIndex[1] = 0;
                c2.HasEnteredGrid = true;
                c2.Crossing = c;
                c2.HasExitedGrid = false;
                drawCar(c2);
          
        }

        
        
        
    }
}
