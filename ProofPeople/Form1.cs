using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace ProofPeople
{
    public partial class Form1 : Form
    {
        private OleDbConnection conn;
        private OleDbCommand _oleCmd;
        //private static String _Arquivo = @"C:\dados\Excel\Empregados.xlsx";
        //private String connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;ReadOnly=False;C:\temp\empleados.xlsx'");
        //private String sql;

        OleDbConnection objConn;
        String connection = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\temp\empleados.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES;'");

        public Form1()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            //private String archivo = @"C:\temp\empleados.xlsx";
            //private String connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;ReadOnly=False';"+ archivo);
            //string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";");
            //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\temp\empleados.xls"
            // Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\myFolder\myExcel2007file.xlsx;
            //string connectionString = string.Format('@"Driver={Microsoft Excel Driver (*.xlsx)};Data Source=c:\temp\empleados.xlsx;"');
            //@"Driver={Microsoft Excel Driver (*.xls)};DBQ=C:\temp\empleados.xls";

            //OdbcConnection conn = new OdbcConnection(connectionString);
            //conn = new OleDbConnection(connectionString);
            //sql = "Select * FROM [empleadostbl$]";

            conn.Open();
            //OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);

            DataSet ds = new DataSet();

            //da.Fill(ds, "Empleados");
        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void limpiarFormulario()
        {
            try
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtEmail.Text = "";
                txtPhone.Text = "";
                cbxCasado.SelectedIndex = -1;
                cbxChilLess16.SelectedIndex = -1;
                cbxChilGre16.SelectedIndex = -1;
            }catch(Exception ex)
            {
                MessageBox.Show("Cannot clean the form , Error:" + ex.ToString());
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                if (guardarRegistro())
                {
                    limpiarFormulario();
                    llenarFormulario();
                    MessageBox.Show("Register saved.");
                }
            }
            else
            {
                MessageBox.Show("You must enter the data in the correct fields.", "Validation");
            }
        }

        private bool guardarRegistro()
        {
            bool flag = true;
            string sql = null;

            try
            {
                OleDbCommand objCmdInsert = new OleDbCommand();
                objConn = new OleDbConnection(connection);

                string casado = "";
                string menores = null;
                string mayores = null;

                if (cbxCasado.SelectedIndex != -1)
                {
                    casado = cbxCasado.SelectedItem.ToString();
                }
                else
                {
                    casado = "null";
                }

                if (cbxChilLess16.SelectedIndex != -1)
                {
                    menores = cbxChilLess16.SelectedItem.ToString();
                }
                else
                {
                    menores = "null";
                }

                if(cbxChilGre16.SelectedIndex != -1)
                {
                    mayores = cbxChilGre16.SelectedItem.ToString();
                }else
                {
                    mayores = "null";
                }


                objConn.Open();
                objCmdInsert.Connection = objConn;
                sql = "INSERT into [empleadostbl$] (nombre, " +
                                                   "apellido, " +
                                                   "casado," +
                                                   "celular," +
                                                   "email," +
                                                   "menores_16," +
                                                   "mayores_16) " +
                                           "values('" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFirstName.Text) + "', " +
                                                   "'" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLastName.Text) + "'," +
                                                   "" + casado + ", " +
                                                   "'" + txtPhone.Text + "'," +
                                                   "'" + txtEmail.Text + "'," +
                                                   "" + menores + "," +
                                                   "" + mayores + " )";

                objCmdInsert.CommandText = sql;
                objCmdInsert.ExecuteNonQuery();
                objConn.Close();                

            }catch(Exception e)
            {
                flag = false;
                MessageBox.Show("Cannot insert new person, " + e.ToString(), "Error");
            }

            return flag;
        }

        /**
         * Method to validate the empty fields. 
         * 
         * @author Diego.Perez.
         * @date 01/04/2019.
         **/
        private bool validarCampos()
        {
            bool flag = true;

            if(txtFirstName.Text == null || txtFirstName.Text == "")
            {
                flag = false;
            }

            if(txtLastName.Text == null || txtLastName.Text == "")
            {
                flag = false;
            }

            if(txtEmail.Text == null || txtEmail.Text == "")
            {
                flag = false;
            }

            if(txtPhone.Text == null || txtPhone.Text == "")
            {
                flag = false;
            }

            return flag;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            llenarFormulario();
        }

        private void llenarFormulario()
        {
            objConn = new OleDbConnection(connection);
            //nombre	apellido					
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT nombre, " +
                                                                "apellido, " +
                                                                "casado, " +
                                                                "celular, " +
                                                                "email, " +
                                                                "menores_16, " +
                                                                "mayores_16 " +
                                                          "FROM [empleadostbl$]", objConn);
            //objCmdSelect.CommandText = "";
            objConn.Open();

            objCmdSelect.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
            DataTable clientes = new DataTable();
            da.Fill(clientes); //name of the datagridviewer
            dgvFornec.DataSource = clientes;
        }
    }
}
