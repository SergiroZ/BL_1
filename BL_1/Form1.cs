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

        private Dictionary<String, int> dBook = new Dictionary<String, int>();

        public DataGridView dataGridView1;
        public String setNewBook = "";
        private DataTable dt = new DataTable();
        public bool isAdd = false;
        public int idBookSender;

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
                    MessageBox.Show(" Cancel the transaction.\n" +
                        " A book with this name:\n" + book.Title +
                        " already exists in the library.");
                    return false;
                }
            }
        }

        public bool EdBook(Book book)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                int cnt = db.Books.Where((x) => x.Title ==
                            book.Title).Count();
                if (cnt <= 1)
                {
                    db.Books.Where((x) =>
                        x.Id == idBookSender).Single().Title = book.Title;
                    db.Books.Where((x) =>
                        x.Id == idBookSender).Single().IdAuthor = book.IdAuthor;
                    db.Books.Where((x) =>
                        x.Id == idBookSender).Single().IdPublisher = book.IdPublisher;
                    db.Books.Where((x) =>
                        x.Id == idBookSender).Single().Pages = book.Pages;
                    db.Books.Where((x) =>
                        x.Id == idBookSender).Single().Price = book.Price;

                    db.SaveChanges();
                    MessageBox.Show(
                        " The book with the name\n" + book.Title +
                        "\n was successfully edited.");
                    return true;
                }
                else
                {
                    MessageBox.Show(" Cancel the transaction.\n" +
                        " A book with this name:\n" + book.Title +
                        " already exists in the library.");
                    return false;
                }
            }
        }

        private void GetAllBooks1()
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var au = db.Books.OrderBy((x) => x.Title).ToList();
                idBookSender = au.First().Id;

                foreach (var a in au)
                {
                    string s = a.Title + " : " + a.Author.FirstName + " " +
                        a.Author.LastName + "  [" + a.Publisher.PublisherName +
                        " " + a.Publisher.Address + "]; Price: " + a.Price +
                        ", pages: " + a.Pages;
                    dBook.Add(s, a.Id);
                }

                //dataGridView1.DataSource = books.Select(selector: x => new { Books = x }).ToList();

                dt.Columns.Add("Books");
                foreach (var item in dBook.Keys)
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
            }
        }

        // another way
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

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            EditBook newBook = new EditBook
            {
                Owner = this
            };

            isAdd = true;

            newBook.ShowDialog();
            // removes an empty string at the end of the datagridview
            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            if (isAdd)
            {
                DataRow newRow = dt.NewRow();
                newRow["Books"] = setNewBook;
                dt.Rows.Add(newRow);
            }
        }

        private void bindingNavigatorEditItem_Click(object sender, EventArgs e)
        {
            EditBook newBook = new EditBook
            {
                Owner = this
            };

            isAdd = false;

            newBook.ShowDialog();

            dt.Rows[dataGridView1.CurrentRow.Index]["Books"] = setNewBook;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns["Books"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            setNewBook = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            idBookSender = dBook[dataGridView1.CurrentRow.Cells[0].Value.ToString()];
            setNewBook = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }
    }
}