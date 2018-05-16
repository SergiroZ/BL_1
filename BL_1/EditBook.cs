using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BL_1
{
    public partial class EditBook : Form
    {
        private Dictionary<String, int> dAuthor = new Dictionary<String, int>();
        private Dictionary<String, int> dPublisher = new Dictionary<String, int>();
        private string keyAuthor, keyPublisher;
        private int valueAuthor, valuePublisher;

        public EditBook()
        {
            InitializeComponent();
        }

        private void EditBook_Load(object sender, EventArgs e)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var au = db.Authors.OrderBy((x) => x.FirstName).ToList();
                //IList<String> authors = new List<String>();
                foreach (var a in au)
                {
                    string s = a.FirstName + " " + a.LastName;
                    //authors.Add(s);
                    dAuthor.Add(s, a.Id);
                }

                comboBoxAuthor.DataSource = new BindingSource(dAuthor, null);
                comboBoxAuthor.DisplayMember = "Key";
                comboBoxAuthor.ValueMember = "Value";

                var pb = db.Publishers.OrderBy((x) => x.PublisherName).ToList();
                //IList<String> publishers = new List<String>();
                foreach (var a in pb)
                {
                    string s = a.PublisherName + " :: " + a.Address;
                    //publishers.Add(s);
                    dPublisher.Add(s, a.Id);
                }
                comboBoxPublisher.DataSource = new BindingSource(dPublisher, null);
                comboBoxPublisher.DisplayMember = "Key";
                comboBoxPublisher.ValueMember = "Value";

                textBoxTitle.Text = "unknown..";
                textBoxPrice.Text = "0";
                textBoxPages.Text = "0";
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (Owner is Form1 main)
            {
                main.dataGridView1.Rows.RemoveAt(main.dataGridView1.Rows.Count - 1);
            }
            Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            Book newBook = new Book
            {
                Title = textBoxTitle.Text.Trim(),
                IdAuthor = valueAuthor,
                IdPublisher = valuePublisher,
                Pages = Convert.ToInt32(textBoxPages.Text),
                Price = Convert.ToInt32(textBoxPrice.Text),
            };
            if (Owner is Form1 main)
            {
                if (!main.AddBook(newBook))
                {
                    main.dataGridView1.Rows.RemoveAt(main.dataGridView1.Rows.Count - 1);
                }
            }
            Close();
        }

        private void comboBoxAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            keyAuthor = ((KeyValuePair<string, int>)comboBoxAuthor.SelectedItem).Key;
            valueAuthor = ((KeyValuePair<string, int>)comboBoxAuthor.SelectedItem).Value;
        }

        private void comboBoxPublisher_SelectedIndexChanged(object sender, EventArgs e)
        {
            keyPublisher = ((KeyValuePair<string, int>)comboBoxPublisher.SelectedItem).Key;
            valuePublisher = ((KeyValuePair<string, int>)comboBoxPublisher.SelectedItem).Value;
        }
    }
}