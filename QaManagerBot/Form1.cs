using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace QaManagerBot
{
    public partial class Form1 : Form
    {

        public string qa_table;
        public string work_dir;
        public string file_name;
        public string app_path;

        public int column_index;
        public int row_index;

        public DataTable dt;
        private DataSet ds1;

        public string quest_data_json;
        public string qaid;
        public string quest;
        public string answ;
        public string hyper;
        public string active;


        public Form1()
        {
            InitializeComponent();

            this.app_path = AppDomain.CurrentDomain.BaseDirectory;
            this.qa_table = AppSetting.GetQatableName();
            this.label1.Text = qa_table;

        }


        private void ViewHeaderQuestTable()
        {
            try
            {
                this.dataGridView1.Columns[0].HeaderText = "ID";
                this.dataGridView1.Columns[1].HeaderText = "Вопрос";
                this.dataGridView1.Columns[2].HeaderText = "Ответ";
                this.dataGridView1.Columns[3].HeaderText = "Ссылка";
                this.dataGridView1.Columns[4].HeaderText = "Номер заявки";
                this.dataGridView1.Columns[5].HeaderText = "Заявка";
                this.dataGridView1.Columns[6].HeaderText = "Статус заявки";
                this.dataGridView1.Columns[7].HeaderText = "Сообщение по статусу заявки";
                this.dataGridView1.Columns[8].HeaderText = "Активно";
                this.dataGridView1.Columns[9].HeaderText = "Организация";
                this.dataGridView1.Columns[10].HeaderText = "Контакт";
            }    
            catch
            {
                return;
            }

        }

        public static DataTable? UseNewtonsoftJson(string json)
        {
            DataTable? dataTable = new();
            if (string.IsNullOrWhiteSpace(json))
            {
                return dataTable;
            }

            try
            {
                dataTable = JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                dataTable = null;
            }

            return dataTable;
        }


        /*
        DataTable get_xls_dt(string s)
        {
            using (OleDbConnection conn = new OleDbConnection())
            {
                System.Data.DataTable dt = new System.Data.DataTable();               
                conn.ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + s + "; Extended Properties='Excel 12.0;HDR=YES'";               
                using (OleDbCommand comm = new OleDbCommand())
                {
                    //comm.CommandText = "Select [Организация] from [sheet1$];";
                    comm.CommandText = "Select * from [Лист1$];";
                    comm.Connection = conn;
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);                        
                        return dt;
                    }
                }
            }
        }
        */

        private void button3_Click(object sender, EventArgs e)
        {
            // загрузить из XML
            this.ds1 = new DataSet();
            this.ds1.ReadXml(this.qa_table + ".xml");
            this.dataGridView2.DataSource = this.ds1.Tables[0];
            this.row_index = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // import Excel
            OpenFileDialog d = new OpenFileDialog();
            d.InitialDirectory = this.work_dir;
            d.Title = "Выберите XLSX файл";
            d.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            if (d.ShowDialog() == DialogResult.OK)
            {
                this.file_name = d.FileName;

            }
            else return;

            //this.ds_excel.Clear();
            //this.ds_excel.Tables.Add(get_xls_dt(this.file_name));
            //this.dataGridView2.DataSource = this.ds_excel.Tables[0];

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // change cell
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("В списке вопросов-ответов нет записей");
                return;
            }
            if (this.dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Вы не выбрали запись");
                return;
            }

            if (this.column_index != 0)
            {
                int row = this.dataGridView1.CurrentCell.RowIndex;
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                ((DataTable)this.dataGridView1.DataSource).Rows[row][col] = this.textBox1.Text.Trim();
            }

            this.qaid = this.dt.Rows[this.row_index]["QaId"].ToString();
            this.quest = this.dt.Rows[this.row_index]["Quest"].ToString();
            this.answ = this.dt.Rows[this.row_index]["Answ"].ToString();
            this.hyper = this.dt.Rows[this.row_index]["Hyper"].ToString();
            this.active = this.dt.Rows[this.row_index]["Active"].ToString();

            button13.PerformClick();
            //button11.PerformClick(); refresh
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
            this.column_index = e.ColumnIndex;
            this.row_index = e.RowIndex;
            if (this.column_index != 0)
            {
                this.textBox1.Text = this.dataGridView1.CurrentCell.Value.ToString();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // перенести строку как новую
            if (this.dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("В списке вопросов-ответов нет записей");
                return;
            }
            if (this.dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Вы не выбрали запись");
                return;
            }

            DataRow row = this.dt.NewRow();
            this.qaid = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, 1000000000).ToString();
            row["Qaid"] = this.qaid;
            //if (this.ds1.Tables[0].Rows[this.row_index]["Quest"].ToString() != "")
            this.quest = this.ds1.Tables[0].Rows[this.row_index]["Quest"].ToString();
            row["Quest"] = this.quest;
            this.answ = this.ds1.Tables[0].Rows[this.row_index]["Answ"].ToString();
            row["Answ"] = this.answ;
            this.hyper = this.ds1.Tables[0].Rows[this.row_index]["Hyper"].ToString();
            row["Hyper"] = this.hyper;
            this.active = "0";
            row["Active"] = 0;
            this.dt.Rows.Add(row);

            this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Selected = true;
            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0];

            button12.PerformClick();
            //button11.PerformClick(); refresh
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.row_index = e.RowIndex;

        }


        private async void button7_Click(object sender, EventArgs e)
        {
            // delete
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("В списке вопросов-ответов нет записей");
                return;
            }
            if (this.dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Вы не выбрали запись");
                return;
            }

            var confirmResult = MessageBox.Show("Вы хотите удалить запись c ID = " + this.dt.Rows[this.row_index][0].ToString() + " ?", "Confirm Delete!!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                string qaid = this.dt.Rows[this.row_index][0].ToString();
                Task<string> t1 = this.CreateDataAsync("http://localhost/app1/api/qa/delete.php?qaid=" + qaid);
                await t1;

            }
            else
            {
                return;
            }
            button11.PerformClick();
        }


        public async Task<string> GetQaAsync(string url)
        {
            string ret = "";
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    ret = content;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetAsync : Exception : " + ex.Message);
                }
            }
            return ret;
        }



        private async void button6_Click(object sender, EventArgs e)
        {
            // method 1
            //DataTable dta = new DataTable();
            // "http://localhost/proga.php"
            Task<string> t1 = this.GetQaAsync("http://localhost/getquests.php");
            string sss = await t1;
            MessageBox.Show(sss);
            //this.dataGridView2.DataSource = dta; 
            this.dataGridView2.DataSource = UseNewtonsoftJson(sss);
        }


        private async void button8_Click(object sender, EventArgs e)
        {
            // Users
            Task<string> t1 = this.GetQaAsync("http://localhost/getusers.php");
            string sss = await t1;
            MessageBox.Show(sss);
            this.dataGridView2.DataSource = UseNewtonsoftJson(sss);

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            // Users ++
            Task<string> t1 = this.GetQaAsync("http://localhost/getusers.php");
            string sss = await t1;
            MessageBox.Show(sss);

        }


        private async Task<string> CreateDataAsync(string url)
        {
            string ret = "";
            using (var client = new HttpClient())
            {
                try
                {
                    HttpClientHandler handler = new HttpClientHandler();
                    HttpClient httpClient = new HttpClient(handler);
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    //var content = await response.Content.ReadAsStringAsync();
                    ret = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("CreateDataAsync : Exception : " + ex.Message);
                }
            }
            return ret;
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            // create
            // "http://localhost/app1/api/user/create.php?id=3&username=Useraaa"
            // "http://localhost/app1/api/user/create.php?id=6&username=тмта ОО р РРТТ"
            // nowork + # '
            // work , . ; / | \\ 
            // @"http://localhost/app1/api/user/create.php?id=8&username=РР ТТ ЖБЬйЙж "">,.;|"
            Task<string> t1 = this.CreateDataAsync(@"http://localhost/app1/api/user/create.php?id=1&username=ллввшо ор рг г");
            string sss = await t1;
            //MessageBox.Show(sss);

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            Task<string> t1 = this.GetQaAsync("http://localhost/app1/api/getquests.php");
            this.quest_data_json = await t1;
            //MessageBox.Show(this.quest_data_json);

            this.dt = UseNewtonsoftJson(this.quest_data_json);
            this.dataGridView1.DataSource = this.dt;

            this.ViewHeaderQuestTable();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // get data quest
            //this.button11.PerformClick();

            Task<string> t1 = this.GetQaAsync("http://localhost/app1/api/getquests.php");
            this.quest_data_json = await t1;
            //MessageBox.Show(this.quest_data_json);

            this.dt = UseNewtonsoftJson(this.quest_data_json);
            this.dataGridView1.DataSource = this.dt;

            this.ViewHeaderQuestTable();
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            // create qa
            // nowork + # '  work , . ; / | \\ 
            // @"http://localhost/app1/api/user/create.php?id=8&username=РР ТТ ЖБЬйЙж "">,.;|"
            Task<string> t1 = this.CreateDataAsync(@"http://localhost/app1/api/qa/create.php?qaid=" + this.qaid + "&quest=" + this.quest + "&answ=" + this.answ + "&hyper=" + this.hyper + "&active=0");
            string sss = await t1;
            //MessageBox.Show(sss);
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            // update qa
            // nowork + # '  work , . ; / | \\ 
            // @"http://localhost/app1/api/user/create.php?id=8&username=РР ТТ ЖБЬйЙж "">,.;|"
            Task<string> t1 = this.CreateDataAsync(@"http://localhost/app1/api/qa/update.php?qaid=" + this.qaid + "&quest=" + this.quest + "&answ=" + this.answ + "&hyper=" + this.hyper + "&active=" + this.active);
            string sss = await t1;
            //MessageBox.Show(sss);
        }


    }
}
