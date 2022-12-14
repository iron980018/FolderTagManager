using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//use to read txt
using System.IO.Ports;
using static System.Net.Mime.MediaTypeNames;

namespace FolderTagManager
{
    public partial class Form1 : Form
    {
        //public string[] local = { "C:\\Users\\iron0\\OneDrive\\桌面\\novel\\轉生未來AI", null, null }; 
        private string start_adress;
        private string[] tag_kind = new string[100];
        private string input_search;
        private string input_tag_name;
        private int tag_kind_size = 0;
        private int selectedIndex;
        private string[] empty_tags = new string[5];

        public void change_adress(string new_adr)
        {
            start_adress = @new_adr;
            MessageBox.Show(start_adress);

            StreamWriter sw = new StreamWriter(@".\settings.txt");   //error         
            sw.WriteLine(start_adress);
            for (int i = 0; i < 100; i++)
            {
                sw.WriteLine(tag_kind[i]);
            }
            sw.Close();
        }

        private void set_empty_tags()// (done?
        {
            for(int i = 0; i < 5; i++)
            {
                empty_tags[i] = " ";
            }
        }

        public class floders_type// (done?
        {
            public string folder_name;
            public string folder_adress;
            public string[] tags = new string[5];
            public string get_folder_name()
            {
                return folder_name;
            }
            public string get_folder_adress()
            { 
                return folder_adress; 
            }
            public string[] get_tags()
            {
                return tags;
            }
            public void set_tags(string[] new_tags)
            {
                tags = new_tags;
            }
        }

        List<floders_type> floders_list = new List<floders_type>();

        public static bool IsTextEqualPatternOrQuestionMark(char character, char pattern)// (done?
        {
            if (character == pattern || pattern == '?')
                return true;
            return false;
        }

        private void show_all_datagird()// (done?
        {
            string[] show_tags = new string[5];
            foreach (floders_type f_list in floders_list)
            {
                show_tags = f_list.get_tags();
                //tag全變最後1個資料夾的tag ---------------------------------- error
                dataGridView1.Rows.Add(new object[] { f_list.folder_name, show_tags[0], show_tags[1], show_tags[2], show_tags[3], show_tags[4] });
            }
        }

        private void clear_datagrid()// (done?
        {
            dataGridView1.Rows.Clear();
        }

        private void show_target_tag_datagrid(int target_index)// (done?
        {
            string set_taget_tag = tag_kind[target_index];
            string[] get_list_tag = new string[5];
            foreach (floders_type f_list in floders_list)
            {
                get_list_tag = f_list.get_tags();
                for(int i = 0; i < 5; i++)
                {
                    if(set_taget_tag == get_list_tag[i])
                    {
                        dataGridView1.Rows.Add(new object[] { f_list.folder_name, get_list_tag[0], get_list_tag[1], get_list_tag[2], get_list_tag[3], get_list_tag[4] });
                        break;
                    }
                }
            }
        }

        private void initialization_tag_kind()//初始化標籤種類 (done?
        {
            for (int i = 0; i < 100; i++)
            {
                tag_kind[i] = " ";
            }
        }

        public Form1() //(done?
        {

            InitializeComponent();
            set_empty_tags();
            comboBox1.Items.Add(" ");
            initialization_tag_kind();

            if (!File.Exists(@".\settings.txt"))//初次啟動，建立儲存book所在地和tag種類的檔案settings.txt
            {
                start_adress = @".\book\";
                if (!Directory.Exists(@".\book"))//建立book資料夾存放檔案
                {
                    Directory.CreateDirectory(@".\book");
                }
                FileStream fileStream = new FileStream(@".\settings.txt", FileMode.Create);
                fileStream.Close(); 
                using (StreamWriter sw = new StreamWriter(@".\settings.txt"))//寫入book所在地和tag種類
                {
                    sw.Write(@".\book\"+"\n");
                    for (int i = 0; i < 100; i++)
                    {
                        sw.Write(tag_kind[i] + "\n");
                    }
                }                
            }
            else
            {                
                StreamReader sr = new StreamReader(@".\settings.txt");
                start_adress = sr.ReadLine();// or @+sr.ReadLine()
                start_adress = @start_adress;
                tag_kind[0] = sr.ReadLine();
                if (tag_kind[0] != " ")
                {
                    int i = 1;
                    tag_kind_size++;
                    while (tag_kind[i] != " " && i < 100)
                    {
                        tag_kind[i] = sr.ReadLine();
                        i++;
                        tag_kind_size++;
                    }
                }
            }

            if(!File.Exists(@".\save.txt"))//初次啟動，建立儲存資料夾名稱，位置，tag 1~5的檔案save.txt
            {
                FileStream fileStream = new FileStream(@".\save.txt", FileMode.Create);
                fileStream.Close();

                string dir_name;
                string dir_adress;
                string[] dir_tags = new string[5];
                for(int i = 0; i < 5; i++)
                {
                    dir_tags[i] = " ";
                }

                //讀取start_adress下資料夾名稱並初始化並寫入save.txt
                DirectoryInfo di = new DirectoryInfo(start_adress);
                DirectoryInfo[] dirsInDir = di.GetDirectories("*.*");

                StreamWriter sw = new StreamWriter(@".\save.txt");
                foreach (DirectoryInfo foundDir in dirsInDir)
                {
                    dir_name = foundDir.Name;
                    dir_adress = foundDir.FullName;
                    floders_list.Add(new floders_type() { folder_name = dir_name,folder_adress = dir_adress ,  tags = dir_tags});
                    try
                    {
                        //Pass the filepath and filename to the StreamWriter Constructor
                        //Write a line of text
                        sw.WriteLine(dir_name);
                        sw.WriteLine(dir_adress);
                        for(int i = 0; i < 5; i++)
                        {
                            sw.WriteLine(dir_tags[i]);
                        }
                        //Write a second line of text
                        //Close the file                        
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                    }                    
                }
                sw.Close();
            }
            else
            {
                //read save.txt
                StreamReader sr = new StreamReader(@".\save.txt");
                
                string f_name = sr.ReadLine();
                string f_adress;
                string[] f_tags = new string[5];
                while(f_name != null)
                {
                    f_adress = sr.ReadLine();
                    for(int i = 0; i < 5; i++)
                    {
                        f_tags[i] = sr.ReadLine();
                    }
                    floders_list.Add(new floders_type() { folder_adress = f_adress, folder_name = f_name , tags = f_tags});
                    f_name = sr.ReadLine();
                }
                sr.Close();

                //讀取start_adress下的資料夾與save.txt比較有無新增資料夾
                string dir_adress;
                string dir_name;
                DirectoryInfo di = new DirectoryInfo(@start_adress);
                DirectoryInfo[] dirsInDir = di.GetDirectories("*.*");
                
                foreach (DirectoryInfo foundDir in dirsInDir)
                {
                    dir_name = foundDir.Name;
                    dir_adress = foundDir.FullName;
                    //floders_list.Contains(new floders_type { folder_name = dir_name }).ToString()
                    //MessageBox.Show(floders_list.Exists(floders_list => floders_list.get_folder_name() == dir_name).ToString());
                    //if (!floders_list.Contains(new floders_type { folder_name = dir_name }))
                    if (!floders_list.Exists(floders_list => floders_list.get_folder_name() == dir_name))//有的話寫入save.txt
                    {
                        floders_list.Add(new floders_type() { folder_adress = dir_adress, folder_name = dir_name, tags = empty_tags });
                        StreamWriter sw = new StreamWriter(@".\save.txt",true);
                        try
                        {
                            sw.WriteLine(dir_name);
                            sw.WriteLine(dir_adress);
                            
                            for (int i = 0; i < 5; i++)
                            {
                                sw.WriteLine(empty_tags[i]);
                            }           
                        }
                        catch (Exception e)
                        {
                        }
                        finally
                        {
                        }
                        sw.Close();
                    }
                }
            }

            show_all_datagird();
            if(tag_kind_size > 0)
            {
                comboBox1.Items.AddRange(tag_kind);
            }
            //MessageBox.Show(start_adress);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            dataGridView1.Rows.Add(new object[] { "a", "資工", "台大", "大二", null, null });
            dataGridView1.Rows.Add(new object[] { "b", "資工", "淡江", "大三", null, null });
            dataGridView1.Rows.Add(new object[] { "c", "地質", "淡江", "大二", null, null });
            */
        }

        private void pictureBox1_Click(object sender, EventArgs e)// (done
        {
            //HK416 MY WIFE
            MessageBox.Show("416我老婆","看!是我老婆ㄟ");
        }

        private void button1_Click(object sender, EventArgs e)//搜尋
        {
            /*
            MessageBox.Show(start_adress);
            System.Diagnostics.Process.Start("Explorer.exe", $"/e, {start_adress}");
            */
            bool all_space = true;
            bool tag_exist = false;
            for(int i = 0 ; i < input_search.Length ; i++)
            {
                if(input_search[i] != ' ')
                {
                    all_space = false;
                }
                if (input_search[i] == '#')
                {
                    tag_exist = true;
                }
            }
            clear_datagrid();
            if(input_search == null || input_search == "")
            {
                show_all_datagird();
                //MessageBox.Show("null or \"\"");
            }
            else if(all_space)
            {
                show_all_datagird();
                //MessageBox.Show("space");
            }
            else
            {
                if(!tag_exist)
                {
                    //List<floders_type> floders_list = new List<floders_type>();
                    foreach (floders_type target_floder in floders_list)
                    {
                        if (input_search == target_floder.get_folder_name())
                        {
                            string[] the_searched_tags = target_floder.get_tags();
                            dataGridView1.Rows.Add(new object[] { target_floder.get_folder_name(), the_searched_tags[0], the_searched_tags[1], the_searched_tags[2], the_searched_tags[3], the_searched_tags[4] });
                        }
                    }
                }
                else
                {
                    show_all_datagird();
                    MessageBox.Show("作者還沒做出來直接搜尋標籤\n等之後的版本吧~");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)//輸入搜尋 (done?
        {
            input_search = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)//設定 (done?
        {
            Settings form = new Settings(this);
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)//開啟 (done?
        {
            /*
             得知datagridview row使用者所在列數
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            MessageBox.Show(rowIndex.ToString());
            */

            /*
             * 開啟目標目錄資料夾
            string filename = @"";
            filename += "C:\Users\iron0\OneDrive\桌面\novel\轉生未來AI";
            System.Diagnostics.Process.Start("Explorer.exe", $"/e, {filename}");
            */
            
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            /*
            string target = @"";
            target += floders_list[rowIndex].get_folder_adress();
            System.Diagnostics.Process.Start("Explorer.exe", $"/e, {target}");
            */
            System.Diagnostics.Process.Start("Explorer.exe", $"/e, {floders_list[rowIndex].get_folder_adress()}");

        }

        private void button3_Click(object sender, EventArgs e)//remove tag (error
        {
            bool all_space = true;
            for (int i = 0; i < input_tag_name.Length; i++)
            {
                if (input_tag_name[i] != ' ')
                {
                    all_space = false;
                }
            }
            clear_datagrid();
            if (input_tag_name == null || input_tag_name == "")
            {
                show_all_datagird();
                MessageBox.Show("null or \"\"");
            }
            else if (all_space)
            {
                show_all_datagird();
                MessageBox.Show("space");
            }
            else
            {
                //得知datagridview row使用者所在列數
                int rowIndex = dataGridView1.CurrentCell.RowIndex;//error return null
                string[] change_tags = new string[5];
                change_tags = floders_list[rowIndex].get_tags();
                for (int i = 0; i < 5; i++)
                {
                    if (change_tags[i] == input_tag_name)
                    {
                        change_tags[i] = " ";
                        dataGridView1.Rows[rowIndex].Cells[i + 2].Value = change_tags[i];
                        floders_list[rowIndex].set_tags(change_tags);
                        break;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//標籤種類 (done?
        {
            selectedIndex = comboBox1.SelectedIndex;
            clear_datagrid();
            if (selectedIndex == 0)
            {
                show_all_datagird();
            }
            else
            {
                show_target_tag_datagrid(selectedIndex);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)//tag name (done?
        {
            input_tag_name = textBox2.Text;
        }

        private void button5_Click(object sender, EventArgs e)//add tag (error
        {
            bool all_space = true;
            for (int i = 0; i < input_tag_name.Length; i++)
            {
                if (input_tag_name[i] != ' ')
                {
                    all_space = false;
                }
            }
            clear_datagrid();
            if (input_tag_name == null || input_tag_name == "")
            {
                show_all_datagird();
                MessageBox.Show("null or \"\"");
            }
            else if (all_space)
            {
                show_all_datagird();
                MessageBox.Show("space");
            }
            else
            {
                //得知datagridview row使用者所在列數
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                string[] change_tags = new string[5];
                change_tags = floders_list[rowIndex].get_tags();
                for (int i = 0; i < 5; i++)
                {
                    if (change_tags[i] == " ")
                    {
                        change_tags[i] = input_tag_name;
                        dataGridView1.Rows[rowIndex].Cells[i + 2].Value = change_tags[i];
                        floders_list[rowIndex].set_tags(change_tags);
                        break;
                    }
                }
            }                
        }
    }
}
