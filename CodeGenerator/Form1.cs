using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeGenerator_BusinessLayer;


namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void _SetCredetials()
        {

            clsGenerator.SetCredentials(tbDataBase.Text.Trim(), tbUserID.Text.Trim(), tbPassword.Text.Trim());

        }



        private void _StartGenerate()
        {

           

            clsGenerator gen = new clsGenerator();

            gen.DataAccessProjectPath = Util.CreateConsoleProject(tbProjectName.Text.Trim() + "_DataAccessLayer");
            gen.BusinessProjectPath = Util.CreateConsoleProject(tbProjectName.Text.Trim() + "_BusinessLayer");



            gen.Start();


            List<clsTable> tables = clsTable.FindTables();


            foreach (clsTable _table in tables)
            {
                gen.table = _table;

                gen.Generate();


            }

           



            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbProjectName.Focus();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this .ValidateChildren ())
            {

                _SetCredetials();

                
                if (!clsGenerator.IsCredetialsCorrect())
                {
                    MessageBox.Show("Incorrect Credentials Or Database Doesn't Existe.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }




                MessageBox.Show("Operation Started be Patient  Until it Completed...",
                    "Operation Started", MessageBoxButtons .OK, MessageBoxIcon.Information);

                
         
                Task.Run(() => _StartGenerate()).ContinueWith(t =>
                {


                    MessageBox.Show("Operation Completed Go to " + Util.solutionDirectory + " To see The Results",
    "Operation Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                 

                }

                );






            }



        }

        private void tbDataBase_Validating(object sender, CancelEventArgs e)
        {
            if (string .IsNullOrEmpty(tbDataBase .Text .Trim ()))
            {
                EpDbInfo.SetError(tbDataBase, "This Field is Required!");

            }
            else
                EpDbInfo.SetError(tbDataBase,"");

        }

        private void tbUserID_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbUserID_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(tbUserID.Text.Trim()))
            {
                EpDbInfo.SetError(tbUserID, "This Field is Required!");

            }
            else
                EpDbInfo.SetError(tbUserID, "");

        }

        private void tbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbPassword.Text.Trim()))
            {
                EpDbInfo.SetError(tbPassword, "This Field is Required!");

            }
            else
                EpDbInfo.SetError(tbPassword, "");

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

      

    }
}
