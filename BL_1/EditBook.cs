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
        public Dictionary<String, int> dAuthor = new Dictionary<String, int>();
        private Dictionary<String, int> dPublisher = new Dictionary<String, int>();
        private string keyAuthor, keyPublisher;
        private int valueAuthor, valuePublisher;
        private bool localIsAdd = true;
        private int senderIdBook;
        private string sTransmitter;

        public EditBook()
        {
            InitializeComponent();
        }

        private void EditBook_Load(object sender, EventArgs e)
        {
            if (Owner is Form1 main)
            {
                localIsAdd = main.isAdd;
                senderIdBook = main.idBookSender;
            }

            using (LibraryEntities db = new LibraryEntities())
            {
                // initializing fields with values from the selected
                // row of the parent datagridview

                // for textBoxTitle:
                textBoxTitle.Text = (from bk in db.Books.ToList()
                                     where bk.Id == senderIdBook
                                     select bk.Title).Single();

                // for comboBoxAuthor:
                var au = db.Authors.OrderBy((x) => x.FirstName).ToList();
                foreach (var a in au)
                {
                    string s = a.FirstName + " " + a.LastName;
                    dAuthor.Add(s, a.Id);
                }
                comboBoxAuthor.DataSource = new BindingSource(dAuthor, null);
                comboBoxAuthor.DisplayMember = "Key";
                comboBoxAuthor.ValueMember = "Value";
                sTransmitter = (from bk in db.Books.ToList()
                                where bk.Id == senderIdBook
                                select bk.Author.FirstName + " " + bk.Author.LastName).Single();
                comboBoxAuthor.Text = sTransmitter;
                // the same thing in another way
                // comboBoxAuthor.SelectedIndex = comboBoxAuthor.FindString(sAut);

                // for comboBoxPublisher:
                var pb = db.Publishers.OrderBy((x) => x.PublisherName).ToList();
                foreach (var a in pb)
                {
                    string s = a.PublisherName + " :: " + a.Address;
                    dPublisher.Add(s, a.Id);
                }
                comboBoxPublisher.DataSource = new BindingSource(dPublisher, null);
                comboBoxPublisher.DisplayMember = "Key";
                comboBoxPublisher.ValueMember = "Value";
                sTransmitter = (from bk in db.Books.ToList()
                                where bk.Id == senderIdBook
                                select bk.Publisher.PublisherName + " :: " + bk.Publisher.Address).Single();
                comboBoxPublisher.Text = sTransmitter;

                // for textBoxPrice
                textBoxPrice.Text = (from bk in db.Books.ToList()
                                     where bk.Id == senderIdBook
                                     select bk.Price).Single().ToString();

                // for textBoxPages
                textBoxPages.Text = (from bk in db.Books.ToList()
                                     where bk.Id == senderIdBook
                                     select bk.Pages).Single().ToString();

                if (localIsAdd)
                {
                    textBoxTitle.Text = "unknown..";
                    textBoxPrice.Text = "0";
                    textBoxPages.Text = "0";
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (Owner is Form1 main)
            {
                main.isAdd = false;
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
                if (localIsAdd)
                {
                    // if add row
                    if (!main.AddBook(newBook))
                    {
                        main.isAdd = false;
                    }
                    else
                    {
                        main.setNewBook = newBook.Title + " : " + keyAuthor +
                        "  [" + keyPublisher + "]; Price: " + newBook.Price +
                        ", pages: " + newBook.Pages;
                    }
                }
                else
                {
                    // if edit row
                    if (!main.EdBook(newBook))
                    {
                        main.isAdd = false;
                    }
                    else
                    {
                        main.setNewBook = newBook.Title + " : " + keyAuthor +
                        "  [" + keyPublisher + "]; Price: " + newBook.Price +
                        ", pages: " + newBook.Pages;
                    }
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