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
        public EditBook()
        {
            InitializeComponent();
        }

        private void EditBook_Load(object sender, EventArgs e)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var au = db.Authors.OrderBy((x) => x.FirstName).ToList();
                IList<String> authors = new List<String>();
                foreach (var a in au)
                {
                    string s = a.FirstName + " " + a.LastName;
                    authors.Add(s);
                }
                comboBoxAuthor.DataSource = authors;

                var pb = db.Publishers.OrderBy((x) => x.PublisherName).ToList();
                IList<String> publishers = new List<String>();
                foreach (var a in pb)
                {
                    string s = a.PublisherName + " :: " + a.Address;
                    publishers.Add(s);
                }
                comboBoxPublisher.DataSource = publishers;
            }
        }
    }
}