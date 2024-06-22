using HARMONY.modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARMONY
{
    public partial class Registrarse : Form
    {
        public Registrarse()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DaoAutenticacion daoautenticacion = new DaoAutenticacion();

            if( daoautenticacion.RegistrarUsuario(txtNombre.Text, txtContrasena.Text))
            {
                MessageBox.Show("Puede loguearse ahora con el boton de abajo");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 formularioLogin = new Form1();

            formularioLogin.Show();
        }
    }
}
