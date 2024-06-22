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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DaoAutenticacion daoautenticacion = new DaoAutenticacion();
            
            string usuario = textboxusuario.Text;
            string contrasena = textboxcontrasena.Text;
            Usuario respuesta = daoautenticacion.Login(usuario, contrasena);
            if (respuesta !=null)
            {
                MessageBox.Show("sesion iniciada correctamente");

                Catalogo_Principal catalogo_Principal = new Catalogo_Principal(respuesta);

                this.Hide();

                catalogo_Principal.Show();

            }
            else
            {
                MessageBox.Show("Las credenciales insertadas son invalidas");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registrarse registroform = new Registrarse();
            registroform.Show();
        }
    }
}
