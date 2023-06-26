using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System.Reflection;

namespace users
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            LoadUserList();
            SetButtonIcons();

        }

        private List<User> userList;
        private List<User> filteredList;
        private const int PageSize = 100;
        private int currentPage = 1;
        private int totalPageCount;


        class User
        {
            public string ID { get; set; }
            public string JobTitle { get; set; }
            public string EmailAddress { get; set; }
            public string FirstNameLastName { get; set; }

            public User(string id, string jobTitle, string emailAddress, string firstNameLastName)
            {
                ID = id;
                JobTitle = jobTitle;
                EmailAddress = emailAddress;
                FirstNameLastName = firstNameLastName;
            }
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        }

        private void LoadUserList()
        {
            string jsonFilePath = "C:\\Users\\Lenovo\\Desktop\\user\\ExportJson.json";

            try
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                userList = JsonConvert.DeserializeObject<List<User>>(jsonContent);
                filteredList = userList.ToList();
                dataGridView1.DataSource = userList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }

        private void Search(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredList = userList.FindAll(u =>
                    (u.ID?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (u.JobTitle?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (u.EmailAddress?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (u.FirstNameLastName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }
            else
            {
                filteredList = userList.ToList();
            }

            //
            totalPageCount = (int)Math.Ceiling((double)filteredList.Count / PageSize);
            currentPage = Math.Min(currentPage, totalPageCount);

            UpdateDataGridView();
            UpdatePaginationButtons();
        }

        private void UpdateDataGridView()
        {
            if (filteredList.Count > 0)
            {
                int startIndex = (currentPage - 1) * PageSize;
                int count = Math.Min(PageSize, filteredList.Count - startIndex);
                List<User> pagedData = filteredList.GetRange(startIndex, count);
                dataGridView1.DataSource = pagedData;
            }
            else
            {
                dataGridView1.DataSource = null; // Verileri temizle
            }
        }

        private void UpdatePaginationButtons()
        {
            if (filteredList.Count > 0)
            {
                int startIndex = (currentPage - 1) * PageSize;
                int count = Math.Min(PageSize, filteredList.Count - startIndex);
                List<User> pagedData = filteredList.Skip(startIndex).Take(count).ToList();
                dataGridView1.DataSource = pagedData;
            }
            else
            {
                dataGridView1.DataSource = null; // Verileri temizle
            }


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            Search(string.Empty);

        }


        private void SetButtonIcons()
        {
            string rightImagePath = "img/right.png";
            string leftImagePath = "img/left.png";

            if (File.Exists(rightImagePath))
            {
                button1.Image = Image.FromFile(rightImagePath);
                button1.Image = new Bitmap(button1.Image, new Size(20, 20));
                button1.ImageAlign = ContentAlignment.MiddleCenter;

            }

            if (File.Exists(leftImagePath))
            {
                button2.Image = Image.FromFile(leftImagePath);
                button2.Image = new Bitmap(button2.Image, new Size(20, 20));
                button2.ImageAlign = ContentAlignment.MiddleCenter;

            }

            button1.Text = "";
            button2.Text = "";

            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;


        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click_1(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            currentPage = 1;
            Search(searchText);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (currentPage < totalPageCount)
            {
                currentPage++;
                Search(textBox1.Text);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Search(textBox1.Text);
            }
        }
    }
}