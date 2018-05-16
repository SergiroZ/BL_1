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
    public partial class Form1 : Form
    {
        private BindingSource bookBindingSource = new BindingSource();
        private IList<String> books = new List<String>();
        public DataGridView dataGridView1;

        public Form1()
        {
            InitializeComponent();
            GetAllBooks1();
        }

        public void AddPublisher(Publisher publisher)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                Publisher a = db.Publishers.Where((x) =>
                x.PublisherName == publisher.
                PublisherName).FirstOrDefault();
                if (a == null)
                {
                    db.Publishers.Add(publisher);
                    db.SaveChanges();
                    MessageBox.Show("New publisher added: " + publisher.PublisherName);
                }
            }
        }

        public bool AddBook(Book book)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                Book a = db.Books.Where((x) => x.Title ==
                book.Title).FirstOrDefault();
                if (a == null)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    MessageBox.Show(" New book added:  " + book.Title);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void GetAllBooks1()
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var au = db.Books.OrderBy((x) => x.Title).ToList();

                int nom = 0;
                foreach (var a in au)
                {
                    string s = (++nom).ToString() + ". " + a.Title + " : " +
                    a.Author.FirstName + " " + a.Author.LastName + "  [" + a.Publisher.PublisherName +
                    " " + a.Publisher.Address + "]; Price: " + a.Price + ", pages: " + a.Pages;
                    books.Add(s);
                }

                //dataGridView1.DataSource = books.Select(selector: x => new { Books = x }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Books");
                foreach (var item in books)
                {
                    var row = dt.NewRow();
                    row["Books"] = item;
                    dt.Rows.Add(row);
                }

                // Set the DataSource to the DataSet
                // Set DataSource of BindingSource to table
                bookBindingNavigator.BindingSource = bookBindingSource;
                bookBindingSource.DataSource = dt;
                dataGridView1.DataSource = bookBindingSource;

                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.Columns["Books"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void GetAllBooks()
        {
            using (var context = new LibraryEntities())
            {
                var query = from books in context.Books
                            join author in context.Authors on books.IdAuthor equals author.Id
                            join publisher in context.Publishers on books.IdPublisher equals publisher.Id
                            select new
                            {
                                books.Id,
                                aFName = author.FirstName,
                                aLName = author.LastName,
                                books.Title,
                                books.Pages,
                                books.Price,
                                publisher.PublisherName,
                                publisher.Address
                            };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.Columns["aFName"].HeaderText = "First name";
                dataGridView1.Columns["aLName"].HeaderText = "Second name";
                dataGridView1.Columns["PublisherName"].HeaderText = "Publisher name";
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //try
            //{
            //}
            //catch (Exception)
            //{
            //    //throw;
            //}
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
        }

        private void bindingBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            EditBook newBook = new EditBook
            {
                Owner = this
            };

            newBook.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void bookBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
        }
    }
}