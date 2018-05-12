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
        public Form1()
        {
            InitializeComponent();

            GetAllBooks();
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
    }
}