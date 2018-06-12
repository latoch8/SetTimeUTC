using SetTimeUTC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gazpar_History
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SQL sql = new SQL(); //logowanie do bazy przy starcie programu (nie widac takiego laga po kliknieciu w szukaj)

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.Select();
            }
        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click_1((object)sender, (EventArgs)e);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((textBox1.Text.Length > 0) && (textBox2.Text.Length > 0))
            {
                Analysis analysis = new Analysis();
                string sn = board_sn(textBox1.Text, textBox2.Text);
                if (sn != null)
                {
                    if (sql.GetHistory(sn) == bb_sn(textBox1.Text, textBox2.Text))
                    {
                        label3.Text = "Pass";
                        label3.ForeColor = Color.Green;
                    }
                    else
                    {
                        label3.Text = "Fail";
                        label3.ForeColor = Color.Red;
                    }
                    timer1.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) 
            {
                if (textBox2.Text.Length > 0)
                {
                    string sn = board_sn(textBox1.Text, textBox2.Text);
                    if (sn != null)
                    {
                        dataGridView1.DataSource = sql.GetHistoryTable(sn);
                    }
                }
                else
                {
                    dataGridView1.DataSource = sql.GetHistoryTable(textBox1.Text);
                }
            }
            else if (textBox2.Text.Length > 0)
            {
                dataGridView1.DataSource = sql.GetHistoryTable(textBox2.Text);
            }
        }

        private string board_sn (string sn1, string sn2)
        {
            string board_sn = null;
            var status = if_sn(sn1, sn2);
            if (if_sn(sn1, sn2))
            {
                board_sn = sn1;
            }
            else if(if_sn(sn2, sn1))
            {
                board_sn = sn2;
            }
            else
            {
                MessageBox.Show("Złe barcody!", "Error");
            }
            return board_sn;
        }

        private string bb_sn(string sn1, string sn2)
        {
            string bb_sn = null;
            if (if_sn(sn1, sn2))
            {
                bb_sn = sn2;
            }
            else if (if_sn(sn2, sn1))
            {
                bb_sn = sn1;
            }
            else
            {
                MessageBox.Show("Złe barcody!", "Error");
            }
            return bb_sn;
        }

        private bool if_sn(string sn1, string sn2)
        {
            return (((sn1[0] == 'J') || (sn1[0] == 'j')) && ((sn2[0] != 'J') || (sn2[0] != 'j')));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = "Result";
            label3.ForeColor = Color.Black;
            timer1.Enabled = false;
        }
    }
}
