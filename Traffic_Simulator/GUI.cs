using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Traffic_Simulator
{
    public partial class GUI : Form
    {
        /// <summary>
        /// Controller element of the application.
        /// </summary>
        private SimulationController _controller = new SimulationController();
        private PictureBox p;

        private void drawCar(Car c)
        {     
            Point position = new Point(6+108,6+75);
            
            switch(c.Direction)
                {
                    case Direction.North:
                        {
                            int n = 0;
                            
                            p = new PictureBox();
                            p.Image = new Bitmap(@"C:\Users\Gustavo\Documents\GitHub\Pro-CSharp\Bitmap1.bmp");
                            //p.Location = new Point(position.X + 66 + c.;
                            p.Size = new System.Drawing.Size(10, 10);
                            p.Show();
                            this.Controls.Add(p);
                            p.BringToFront();  
                        }    
                    case Direction.West:
                    
                    case Direction.South:
                    
                    case Direction.East:

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

        public void refreshScreen(Grid copyOfGrid)
        {
            foreach (Car c in copyOfGrid.ListOfCars) //moves every existing car by 1 position
                if (c.HasEnteredGrid && !c.HasExitedGrid)
                    drawCar(c);

            foreach (Crossing c in copyOfGrid.Slots) //'ticks' all crossings and add new cars to crossings
            {
                c.timeTick(); //ticks the crossings clock
                Car car = new Car();
            }
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
                Invalidate();
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

        
        
        
    }
}
