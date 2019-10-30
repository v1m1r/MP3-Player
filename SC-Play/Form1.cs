using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Un4seen.Bass;

namespace SC_Play
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.IPServ;
            string[] text = System.IO.File.ReadAllLines("bd.ini");
            comboBox1.Items.AddRange(text);
        }







        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Update();
            SC_Play.cs.Vars.Files.Clear();            
            string server = textBox1.Text;
            string database = comboBox1.Text;
            string uid = textBox3.Text;
            string password = textBox4.Text;
            
            string pool = "1500";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "charset=utf8" +";"+ "Connection Timeout=" + pool + "";

            string query = "SELECT * FROM SCData";
            MySqlConnection conDatabase = new MySqlConnection(connectionString);
            MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase);
            MySqlDataReader myreader;


            try
            {
                conDatabase.Open();
                myreader = cmdDatabase.ExecuteReader();
                /*this.dataGridView1.Columns.Add("ID", "ID");
                this.dataGridView1.Columns["ID"].Width = 20;
                this.dataGridView1.Columns.Add("year", "Дата");
                this.dataGridView1.Columns["year"].Width = 90;
                this.dataGridView1.Columns.Add("time", "Время");
                this.dataGridView1.Columns["time"].Width = 90;
                this.dataGridView1.Columns.Add("qa", "Частота");
                this.dataGridView1.Columns["qa"].Width = 90;
                this.dataGridView1.Columns.Add("path", "PLAY");
                this.dataGridView1.Columns["path"].Width = 90;*/
            listBox1.BeginUpdate();  
                
                while (myreader.Read())
                {

                //    dataGridView1.Rows.Add(myreader[0].ToString(), myreader[1].ToString(), myreader[2].ToString(), myreader[3].ToString(), myreader[4].ToString());

                   // listBox1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString() + " " + myreader[4].ToString());

                   // SC_Play.cs.Vars.Files.Add(myreader[5].ToString());
                   // listView1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString());

                    SC_Play.cs.Vars.Files.Add(@"\\" + textBox1.Text + @"\" + myreader[6].ToString());

                    listBox1.Items.Add(myreader[0].ToString() + "  |  " + myreader[1].ToString() + "  |  " + myreader[2].ToString() + "  |  " + myreader[3].ToString() + "  |  " + myreader[4].ToString() + "  |  " +myreader[5].ToString());
                  
                }
                
                listBox1.EndUpdate();
               
               // MessageBox.Show(@"\\"+textBox1.Text+@"\"+myreader[5].ToString());


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          
            conDatabase.Close();
            textBox7.Text = listBox1.Items.Count.ToString();
          
                listBox1.SetSelected(Convert.ToInt32(label12.Text), true);
           
        }
        //Кнопка плэй
        private void button2_Click(object sender, EventArgs e)
        {
            string name3 = listBox1.SelectedIndex.ToString();
            label12.Text = name3;
            if ((listBox1.Items.Count != 0)&&(listBox1.SelectedIndex!=-1))
            {
                string name_one = listBox1.SelectedItem.ToString();
                textBox5.Text = name_one;
               // string curent = SC_Play.cs.Vars.Files[listView1.SelectedIndices];
                string curent = SC_Play.cs.Vars.Files[listBox1.SelectedIndex];
                SC_Play.cs.Vars.CurrentTrackNumber = listBox1.SelectedIndex;
                
                SC_Play.cs.BassLike.Play(curent, SC_Play.cs.BassLike.Volume);
                textBox6.Text = TimeSpan.FromSeconds(SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream)).ToString();
               // slTime.Maximum = SC_Play.cs.BassLike.GetTimeOfStream(SC_Play.cs.BassLike.Stream);
              //  slTime.Value = SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream);
                timer1.Enabled = true;

            }
            groupBox1.Enabled = false;
         
        }

        //Кнопка соп
        private void button3_Click(object sender, EventArgs e)
        {
            SC_Play.cs.BassLike.Stop();
            timer1.Enabled = false;
            slTime.Value = 0;
            textBox6.Text = "00:00:00";
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox6.Text = TimeSpan.FromSeconds(SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream)).ToString();
            //slTime.Value = SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream);
          
            
            if (SC_Play.cs.BassLike.ToNextTrack())
            {
                if (listBox1.SelectedItem != null)
                {
                    listBox1.SelectedIndex = SC_Play.cs.Vars.CurrentTrackNumber;
                    // listBox1.SelectedItem = SC_Play.cs.Vars.CurrentTrackNumber;

                    textBox5.Text = TimeSpan.FromSeconds(SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream)).ToString();
                    string name = listBox1.SelectedItem.ToString();
                    textBox5.Text = name;
                    string name1 = listBox1.SelectedIndex.ToString();
                    label12.Text = name1;
                    //slTime.Maximum = SC_Play.cs.BassLike.GetTimeOfStream(SC_Play.cs.BassLike.Stream);
                    //slTime.Value = SC_Play.cs.BassLike.GetPosOfStream(SC_Play.cs.BassLike.Stream);
                    label1.Text = SC_Play.cs.Vars.CurrentTrackNumber.ToString();
                }
                
            }
            else if (SC_Play.cs.BassLike.EndPlayList)
            {
               
                listBox1.SelectedIndex = Convert.ToInt32(label12.Text);
                SC_Play.cs.Vars.CurrentTrackNumber = Convert.ToInt32(label12.Text);
                SC_Play.cs.BassLike.EndPlayList = false;
               listBox1.SelectedItem = SC_Play.cs.Vars.CurrentTrackNumber;
               button3_Click(this, new EventArgs());
                //string name = listBox1.SelectedItem.ToString();
                /*string name = listBox1.SelectedIndex.ToString();
                label8.Text = name;*/
               groupBox1.Enabled = true;
            }

       
        }

        private void slVol_Scroll(object sender, ScrollEventArgs e)
        {
            SC_Play.cs.BassLike.SetVolumeToStream(SC_Play.cs.BassLike.Stream, slVol.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SC_Play.cs.BassLike.Pause();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button6_Click(object sender, EventArgs e)       
        
        {
            if (textBox8.Text == string.Empty)
            {
                MessageBox.Show("Строка поиска не может быть пустой", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (textBox8.Text != string.Empty)
            {
                // Find the item in the list and store the index to the item.
                int index = listBox1.FindString(textBox8.Text);
                // Determine if a valid index is returned. Select the item if it is valid.
                if (index != -1)
                    listBox1.SetSelected(index, true);
                else
                    MessageBox.Show("Такая запись не найдена","",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string statka = listBox1.SelectedItem.ToString();
         //   textBox9.Text = statka;
            string[] words_stat = statka.Split(new[] { '|' });
           
               

                if (Convert.ToInt32(textBox9.Text.ToString()) > Convert.ToInt32(words_stat[0].ToString()))
                {
                    MessageBox.Show("Статистика не может быть отрицательной", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    Int64 result_smena = Convert.ToInt32(words_stat[0].ToString()) - Convert.ToInt32(textBox9.Text.ToString())+1;

                    MessageBox.Show("Результат работы ->" + " " + result_smena.ToString());
                }
            }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox11.Text != string.Empty)
            {
                // Find the item in the list and store the index to the item.
                int index = listBox1.FindString(textBox11.Text);
                // Determine if a valid index is returned. Select the item if it is valid.
                if (index != -1)
                {
                    listBox1.SetSelected(index, true);
                    string name_up = listBox1.SelectedItem.ToString();
                    //MessageBox.Show(name_up);
                    string[] words_update = name_up.Split(new[] { '|' });
                    string id = words_update[0];
                    string year = words_update[1];
                    string date = words_update[2];
                    string qa = words_update[3];
                    string name = words_update[4];



                    string interes = "Интересное";
                  // string path = "kjk";
                    string server = textBox1.Text;
                    string database = comboBox1.Text;
                    string uid = textBox3.Text;
                    string password = textBox4.Text;
                    string pool = "1500";
                    string connectionString;
                    connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "charset=utf8" + ";" + "Connection Timeout=" + pool + "";


                  //  string queryA = "insert SCData (year,date,qa,name,interes,path) VALUES('" +year + "','" + date + "','" + qa + "','" + name + "','" + interes + "','" + path +"');";


                    // fname = '"+textBoxFName.Text+"', 
                    //string queryA = "UPDATE SCData SET year = '" + year + "', date = '" + date + "', qa = " + qa + "',name='" + name + "',interes='" + interes +"',path='"+path+ " WHERE id=" + int.Parse(id);
                    string queryA = "UPDATE SCData SET interes='" + interes + "' WHERE id=" + int.Parse(id);
                    MySqlConnection conDatabaseA = new MySqlConnection(connectionString);
                    MySqlCommand cmdDatabaseA = new MySqlCommand(queryA, conDatabaseA);
                    MySqlDataReader myreaderA;
                    try
                    {
                        conDatabaseA.Open();
                        myreaderA = cmdDatabaseA.ExecuteReader();

                        while (myreaderA.Read())
                        { }
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    //

                    conDatabaseA.Close();

                }
                else
                    MessageBox.Show("Такая запись не найдена", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            listBox1.Items.Clear();
            listBox1.Update();
            SC_Play.cs.Vars.Files.Clear();
            string server1 = textBox1.Text;
            string database1 = comboBox1.Text;
            string uid1 = textBox3.Text;
            string password1= textBox4.Text;

            string pool1 = "1500";
            string connectionString1;
            connectionString1 = "SERVER=" + server1 + ";" + "DATABASE=" + database1 + ";" + "UID=" + uid1 + ";" + "PASSWORD=" + password1 + ";" + "charset=utf8" + ";" + "Connection Timeout=" + pool1 + "";

            string query1 = "SELECT * FROM SCData";
            MySqlConnection conDatabase1 = new MySqlConnection(connectionString1);
            MySqlCommand cmdDatabase1 = new MySqlCommand(query1, conDatabase1);
            MySqlDataReader myreader1;


            try
            {
                conDatabase1.Open();
                myreader1 = cmdDatabase1.ExecuteReader();
                /*this.dataGridView1.Columns.Add("ID", "ID");
                this.dataGridView1.Columns["ID"].Width = 20;
                this.dataGridView1.Columns.Add("year", "Дата");
                this.dataGridView1.Columns["year"].Width = 90;
                this.dataGridView1.Columns.Add("time", "Время");
                this.dataGridView1.Columns["time"].Width = 90;
                this.dataGridView1.Columns.Add("qa", "Частота");
                this.dataGridView1.Columns["qa"].Width = 90;
                this.dataGridView1.Columns.Add("path", "PLAY");
                this.dataGridView1.Columns["path"].Width = 90;*/
                listBox1.BeginUpdate();

                while (myreader1.Read())
                {

                    //    dataGridView1.Rows.Add(myreader[0].ToString(), myreader[1].ToString(), myreader[2].ToString(), myreader[3].ToString(), myreader[4].ToString());

                    // listBox1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString() + " " + myreader[4].ToString());

                    // SC_Play.cs.Vars.Files.Add(myreader[5].ToString());
                    // listView1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString());

                    SC_Play.cs.Vars.Files.Add(@"\\" + textBox1.Text + @"\" + myreader1[6].ToString());

                    listBox1.Items.Add(myreader1[0].ToString() + "  |  " + myreader1[1].ToString() + "  |  " + myreader1[2].ToString() + "  |  " + myreader1[3].ToString() + "  |  " + myreader1[4].ToString() + "  |  " + myreader1[5].ToString());

                }

                listBox1.EndUpdate();

                // MessageBox.Show(@"\\"+textBox1.Text+@"\"+myreader[5].ToString());


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            conDatabase1.Close();
            textBox7.Text = listBox1.Items.Count.ToString();

            listBox1.SetSelected(Convert.ToInt32(label12.Text), true);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {

                // Find the item in the list and store the index to the item.
                int index = listBox1.FindString(label12.Text);
                // Determine if a valid index is returned. Select the item if it is valid.
                if (index != -1)
                {
                    listBox1.SetSelected(index+1, true);
                    string name_up = listBox1.SelectedItem.ToString();
                    //MessageBox.Show(name_up);
                    string[] words_update = name_up.Split(new[] { '|' });
                    string id = words_update[0];
                    string year = words_update[1];
                    string date = words_update[2];
                    string qa = words_update[3];
                    string name = words_update[4];



                    string interes = "Интересное";
                    // string path = "kjk";
                    string server = textBox1.Text;
                    string database = comboBox1.Text;
                    string uid = textBox3.Text;
                    string password = textBox4.Text;
                    string pool = "1500";
                    string connectionString;
                    connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "charset=utf8" + ";" + "Connection Timeout=" + pool + "";


                    //  string queryA = "insert SCData (year,date,qa,name,interes,path) VALUES('" +year + "','" + date + "','" + qa + "','" + name + "','" + interes + "','" + path +"');";


                    // fname = '"+textBoxFName.Text+"', 
                    //string queryA = "UPDATE SCData SET year = '" + year + "', date = '" + date + "', qa = " + qa + "',name='" + name + "',interes='" + interes +"',path='"+path+ " WHERE id=" + int.Parse(id);
                    string queryA = "UPDATE SCData SET interes='" + interes + "' WHERE id=" + int.Parse(id);
                    MySqlConnection conDatabaseA = new MySqlConnection(connectionString);
                    MySqlCommand cmdDatabaseA = new MySqlCommand(queryA, conDatabaseA);
                    MySqlDataReader myreaderA;
                    try
                    {
                        conDatabaseA.Open();
                        myreaderA = cmdDatabaseA.ExecuteReader();

                        while (myreaderA.Read())
                        { }
                    }


                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    //

                    conDatabaseA.Close();

                }
              
            }
            listBox1.Items.Clear();
            listBox1.Update();
            SC_Play.cs.Vars.Files.Clear();
            string server1 = textBox1.Text;
            string database1 = comboBox1.Text;
            string uid1 = textBox3.Text;
            string password1 = textBox4.Text;

            string pool1 = "1500";
            string connectionString1;
            connectionString1 = "SERVER=" + server1 + ";" + "DATABASE=" + database1 + ";" + "UID=" + uid1 + ";" + "PASSWORD=" + password1 + ";" + "charset=utf8" + ";" + "Connection Timeout=" + pool1 + "";

            string query1 = "SELECT * FROM SCData";
            MySqlConnection conDatabase1 = new MySqlConnection(connectionString1);
            MySqlCommand cmdDatabase1 = new MySqlCommand(query1, conDatabase1);
            MySqlDataReader myreader1;


            try
            {
                conDatabase1.Open();
                myreader1 = cmdDatabase1.ExecuteReader();
                /*this.dataGridView1.Columns.Add("ID", "ID");
                this.dataGridView1.Columns["ID"].Width = 20;
                this.dataGridView1.Columns.Add("year", "Дата");
                this.dataGridView1.Columns["year"].Width = 90;
                this.dataGridView1.Columns.Add("time", "Время");
                this.dataGridView1.Columns["time"].Width = 90;
                this.dataGridView1.Columns.Add("qa", "Частота");
                this.dataGridView1.Columns["qa"].Width = 90;
                this.dataGridView1.Columns.Add("path", "PLAY");
                this.dataGridView1.Columns["path"].Width = 90;*/
                listBox1.BeginUpdate();

                while (myreader1.Read())
                {

                    //    dataGridView1.Rows.Add(myreader[0].ToString(), myreader[1].ToString(), myreader[2].ToString(), myreader[3].ToString(), myreader[4].ToString());

                    // listBox1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString() + " " + myreader[4].ToString());

                    // SC_Play.cs.Vars.Files.Add(myreader[5].ToString());
                    // listView1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString());

                    SC_Play.cs.Vars.Files.Add(@"\\" + textBox1.Text + @"\" + myreader1[6].ToString());

                    listBox1.Items.Add(myreader1[0].ToString() + "  |  " + myreader1[1].ToString() + "  |  " + myreader1[2].ToString() + "  |  " + myreader1[3].ToString() + "  |  " + myreader1[4].ToString() + "  |  " + myreader1[5].ToString());

                }

                listBox1.EndUpdate();

                // MessageBox.Show(@"\\"+textBox1.Text+@"\"+myreader[5].ToString());


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            conDatabase1.Close();
            textBox7.Text = listBox1.Items.Count.ToString();

            listBox1.SetSelected(Convert.ToInt32(label12.Text), true);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Update();
            SC_Play.cs.Vars.Files.Clear();
            string server = textBox1.Text;
            string database = comboBox1.Text;
            string uid = textBox3.Text;
            string password = textBox4.Text;
            string interes2 = "Интересное";
            string pool = "1500";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "charset=utf8" + ";" + "Connection Timeout=" + pool + "";

            string query = "SELECT * FROM SCData WHERE interes='" + interes2 + "'";
            MySqlConnection conDatabase = new MySqlConnection(connectionString);
            MySqlCommand cmdDatabase = new MySqlCommand(query, conDatabase);
            MySqlDataReader myreader;


            try
            {
                conDatabase.Open();
                myreader = cmdDatabase.ExecuteReader();
                /*this.dataGridView1.Columns.Add("ID", "ID");
                this.dataGridView1.Columns["ID"].Width = 20;
                this.dataGridView1.Columns.Add("year", "Дата");
                this.dataGridView1.Columns["year"].Width = 90;
                this.dataGridView1.Columns.Add("time", "Время");
                this.dataGridView1.Columns["time"].Width = 90;
                this.dataGridView1.Columns.Add("qa", "Частота");
                this.dataGridView1.Columns["qa"].Width = 90;
                this.dataGridView1.Columns.Add("path", "PLAY");
                this.dataGridView1.Columns["path"].Width = 90;*/
                listBox1.BeginUpdate();

                while (myreader.Read())
                {

                    //    dataGridView1.Rows.Add(myreader[0].ToString(), myreader[1].ToString(), myreader[2].ToString(), myreader[3].ToString(), myreader[4].ToString());

                    // listBox1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString() + " " + myreader[4].ToString());

                    // SC_Play.cs.Vars.Files.Add(myreader[5].ToString());
                    // listView1.Items.Add(myreader[0].ToString() + " " + myreader[1].ToString() + " " + myreader[2].ToString() + " " + myreader[3].ToString());

                    SC_Play.cs.Vars.Files.Add(@"\\" + textBox1.Text + @"\" + myreader[6].ToString());

                    listBox1.Items.Add(myreader[0].ToString() + "  |  " + myreader[1].ToString() + "  |  " + myreader[2].ToString() + "  |  " + myreader[3].ToString() + "  |  " + myreader[4].ToString() + "  |  " + myreader[5].ToString());

                }

                listBox1.EndUpdate();

                // MessageBox.Show(@"\\"+textBox1.Text+@"\"+myreader[5].ToString());


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            conDatabase.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.IPServ = this.textBox1.Text;
            Properties.Settings.Default.Save();
        }

      
        

      
     
        
       
    }
}
