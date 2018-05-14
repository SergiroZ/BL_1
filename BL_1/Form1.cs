﻿using System;
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

            GetAllBooks1();
        }

        private void AddPublisher(Publisher publisher)
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

        private void AddBook(Book book)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                Book a = db.Books.Where((x) => x.Title ==
                book.Title).FirstOrDefault();
                if (a == null)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    MessageBox.Show("New book added:" + book.Title);
                }
            }
        }

        //private DataGridView songsDataGridView = new DataGridView();

        private void GetAllBooks1()
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var au = db.Books.OrderBy((x) => x.Title).ToList();
                IList<String> books = new List<String>();
                int nom = 0;
                foreach (var a in au)
                {
                    string s = (++nom).ToString() + ". " + a.Title + " : " +
                    a.Author.FirstName + " " + a.Author.LastName + "  [" + a.Publisher.PublisherName +
                    " " + a.Publisher.Address + "]; Price: " + a.Price + ", pages: " + a.Pages;
                    books.Add(s);
                }
                dataGridView1.DataSource = books.Select(selector: x => new { Books = x }).ToList();
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
    }
}