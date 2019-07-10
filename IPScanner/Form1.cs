using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace IPScanner
{
    public partial class Form1 : Form
    {
        private Tester tester = new Tester();

        public Form1()
        {
            InitializeComponent();
            tester.OnIPTested = OnIPTested;
            tester.OnTestFinished = OnTesterFinish;
        }
        private readonly object lock_ = new object();

        private void OnIPTested(IPEndPoint address, Tester.IPTestStatus status)
        {
            lock (lock_)
            {

                if (!InvokeRequired)
                {
                    switch (status)
                    {
                        case Tester.IPTestStatus.Success: label1.Text = "Valid Connections: " + tester.Results.Valid; break;
                        case Tester.IPTestStatus.Failure: label2.Text = "Rejected Connections: " + tester.Results.Bad; break;
                        case Tester.IPTestStatus.Error: label3.Text = "Error: " + tester.Results.Error; break;
                    }
                }
                else
                {
                    Invoke(new Action(() => {
                        switch (status)
                        {
                            case Tester.IPTestStatus.Success: label1.Text = "Valid Connections: " + tester.Results.Valid; break;
                            case Tester.IPTestStatus.Failure: label2.Text = "Rejected Connections: " + tester.Results.Bad; break;
                            case Tester.IPTestStatus.Error: label3.Text = "Error: " + tester.Results.Error; break;
                        }
                    }));
                }
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        
        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.ReadOnly)
            {
                textBox1.Text = String.Empty;
                textBox1.ForeColor = Color.Black;
                textBox1.ReadOnly = false;
            }
        }

        private void textBox2_GotFocus(object sender, EventArgs e)
        {
            if (textBox2.ReadOnly)
            {
                textBox2.Text = String.Empty;
                textBox2.ForeColor = Color.Black;
                textBox2.ReadOnly = false;
            }
        }

        private void textBox3_GotFocus(object sender, EventArgs e)
        {
            if (textBox3.ReadOnly)
            {
                textBox3.Text = String.Empty;
                textBox3.ForeColor = Color.Black;
                textBox3.ReadOnly = false;
            }
        }

        private void toggle()
        {
            radioButton1.Enabled = !radioButton1.Enabled;
            radioButton3.Enabled = !radioButton3.Enabled;
            textBox1.Enabled = !textBox1.Enabled;
            textBox2.Enabled = !textBox2.Enabled;
            textBox3.Enabled = !textBox3.Enabled;
            textBox4.Enabled = !textBox4.Enabled;
            textBox5.Enabled = !textBox5.Enabled;
            button1.Enabled = !button1.Enabled;
            button2.Enabled = !button2.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Valid Connections: 0";
            label2.Text = "Rejected Connections: 0";
            label3.Text = "Error: 0";
            toggle();
            tester.Addresses = Formatter.Format(textBox1.Text, int.Parse(textBox2.Text), int.Parse(textBox3.Text));
            tester.Protocol = radioButton1.Checked ? Tester.IPScannerProtocol.Tcp : Tester.IPScannerProtocol.Icmp;
            tester.Threads = int.Parse(textBox4.Text);
            tester.Timeout = int.Parse(textBox5.Text);
            tester.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tester.Stop();
        }

        private void OnTesterFinish(Tester.TestResults results)
        {
            MessageBox.Show("Done!");
            var path = Directory.GetCurrentDirectory() + "\\IP Scanner";
            Directory.CreateDirectory(path);
            path += string.Format("\\{0} - {1} ({2}).txt", results.ValidEndPoints.First().Address.ToString(), results.ValidEndPoints.Last().Address.ToString(), DateTime.Now.ToString().Replace(":", "_"));
            var text = "Totally tested: " + results.TotalTested;
            text += "\r\nValid IPs: " + results.Valid;
            text += "\r\nInvalid IPs: " + results.Bad;
            text += "\r\nOther connection errors: " + results.Error + "\r\n\r\n";

            foreach (var endpoint in results.ValidEndPoints)
            {
                text += endpoint.Address.ToString() + ":" + endpoint.Port + "\r\n";
            }
            File.WriteAllText(path, text);
            if (!InvokeRequired)
            {
                toggle();
            }
            else
            {
                Invoke(new Action(() => {
                    toggle();
                }));
            }
                
        }
    }
}
