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
        private BindingSource authorBindingSource = new BindingSource();
        private BindingSource publisherBindingSource = new BindingSource();

        private Dictionary<String, int> dBook = new Dictionary<String, int>();

        public DataGridView dataGridViewBook;
        public String setNewBook = "";
        private DataTable dt = new DataTable();
        public bool isAdd = false;
        public int idBookSender;

        public Form1()
        {
            InitializeComponent();
            GetAllBooks();
            GetAllAuthors();
            GetAllPublishers();
        }

        public bool AddAuthor(Author author)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                Author a = db.Authors.Where((x) =>
                x.FirstName + " " + x.LastName == author.
                FirstName + " " + author.LastName).FirstOrDefault();
                if (a == null)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    MessageBox.Show("New aythor added: "
                        + author.LastName + " " + author.LastName);
                    return true;
                }
                else
                {
                    MessageBox.Show(" Cancel the transaction.\n" +
                        " A author with this name:\n" +
                        author.LastName + " " + author.LastName +
                        "/n already exists in the library.");
                    return false;
                }
            }
        }

        public bool AddPublisher(Publisher publisher)
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
                    MessageBox.Show("New publisher added: " +
                        publisher.PublisherName);
                    return true;
                }
                else
                {
                    MessageBox.Show(" Cancel the transaction.\n" +
                        " A publisher with this name:\n" +
                        publisher.PublisherName +
                        "/n already exists in the library.");
                    return false;
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
                    idBookSender = book.Id;
                    MessageBox.Show(" New book added:  " + book.Title);
                    return true;
                }
                else
                {
                    MessageBox.Show(" Cancel the transaction.\n" +
                        " A book with this name:\n" + book.Title +
                        "\n already exists in the library.");
                    return false;
                }
            }
        }

        public bool EdBook(Book book)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                Book b = db.Books.Where((x) => x.Title ==
                            book.Title).FirstOrDefault();
                if (b == null)
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
                        "\n already exists in the library.");
                    return false;
                }
            }
        }

        private void GetAllBooks()
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
                dataGridViewBook.DataSource = bookBindingSource;
            }
        }

        // another way
        //private void GetAllBooks1()
        //{
        //    using (var context = new LibraryEntities())
        //    {
        //        var query = from books in context.Books
        //                    join author in context.Authors on books.IdAuthor equals author.Id
        //                    join publisher in context.Publishers on books.IdPublisher equals publisher.Id
        //                    select new
        //                    {
        //                        books.Id,
        //                        aFName = author.FirstName,
        //                        aLName = author.LastName,
        //                        books.Title,
        //                        books.Pages,
        //                        books.Price,
        //                        publisher.PublisherName,
        //                        publisher.Address
        //                    };
        //        dataGridViewBook.DataSource = query.ToList();
        //        dataGridViewBook.Columns["aFName"].HeaderText = "First name";
        //        dataGridViewBook.Columns["aLName"].HeaderText = "Second name";
        //        dataGridViewBook.Columns["PublisherName"].HeaderText = "Publisher name";
        //    }
        //}

        private void GetAllAuthors()
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var dtAuthor = db.Authors.Select(selector:
                    x => new { x.FirstName, x.LastName }).ToList();

                dataGridViewAuthors.DataSource = dtAuthor;
                dataGridViewAuthors.Columns[0].HeaderText = "First Name";
                dataGridViewAuthors.Columns[1].HeaderText = "Last Name";

                // Set the DataSource to the DataSet
                // Set DataSource of BindingSource to table
                authorBindingNavigator.BindingSource = authorBindingSource;
                authorBindingSource.DataSource = dtAuthor;
                dataGridViewAuthors.DataSource = authorBindingSource;
                dataGridViewAuthors.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridViewAuthors.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void GetAllPublishers()
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var dtPublisher = db.Publishers.Select(selector:
                    x => new { x.PublisherName, x.Address }).ToList();
                dataGridViewPublishers.DataSource = dtPublisher;

                dataGridViewPublishers.Columns[0].HeaderText = "Name";
                dataGridViewPublishers.Columns[1].HeaderText = "Address";

                // Set the DataSource to the DataSet
                // Set DataSource of BindingSource to table
                publisherBindingNavigator.BindingSource = publisherBindingSource;
                publisherBindingSource.DataSource = dtPublisher;
                dataGridViewPublishers.DataSource = publisherBindingSource;
                dataGridViewPublishers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridViewPublishers.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            dataGridViewBook.Rows.RemoveAt(dataGridViewBook.Rows.Count - 1);
            if (isAdd)
            {
                DataRow newRow = dt.NewRow();
                newRow["Books"] = setNewBook;
                dt.Rows.Add(newRow);
                dBook.Add(setNewBook, idBookSender);
            }
        }

        private void bindingNavigatorEditItem_Click(object sender, EventArgs e)
        {
            EditBook newBook = new EditBook
            {
                Owner = this
            };

            isAdd = false;
            dBook.Remove(setNewBook);
            newBook.ShowDialog();
            dBook.Add(setNewBook, idBookSender);

            dt.Rows[dataGridViewBook.CurrentRow.Index]["Books"] = setNewBook;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewBook.AllowUserToAddRows = false;
            dataGridViewBook.Columns["Books"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewBook.MultiSelect = false;
            dataGridViewBook.ReadOnly = true;
            setNewBook = dataGridViewBook.CurrentRow.Cells[0].Value.ToString();

            authorBindingNavigator.Visible = false;
            publisherBindingNavigator.Visible = false;
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            int caseSwitch = tabControl1.SelectedIndex;
            switch (caseSwitch)
            {
                case 0:
                    bookBindingNavigator.Visible = true;
                    authorBindingNavigator.Visible = false;
                    publisherBindingNavigator.Visible = false;
                    break;

                case 1:
                    bookBindingNavigator.Visible = false;
                    authorBindingNavigator.Visible = true;
                    publisherBindingNavigator.Visible = false;
                    break;

                case 2:
                    bookBindingNavigator.Visible = false;
                    authorBindingNavigator.Visible = false;
                    publisherBindingNavigator.Visible = true;
                    break;

                default:
                    break;
            }
        }

        private void dataGridViewBook_MouseClick(object sender, MouseEventArgs e)
        {
            idBookSender = dBook[dataGridViewBook.CurrentRow.Cells[0].Value.ToString()];
            setNewBook = dataGridViewBook.CurrentRow.Cells[0].Value.ToString();
        }
    }
}