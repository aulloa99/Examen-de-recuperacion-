using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HARMONY.modelos;

namespace HARMONY
{
    public partial class Catalogo_Principal : Form
    {
        private DaoCanciones daoCanciones;
        private Usuario usuarioUser;
        public Catalogo_Principal(Usuario usuario)
        {

            InitializeComponent();
            usuarioUser = usuario;
            daoCanciones = new DaoCanciones();


            this.dataGridViewCatalogo.SelectionChanged += new EventHandler(this.cellselectedchanged);

            dataGridViewCatalogo.Columns.Add("Id", "ID");
            dataGridViewCatalogo.Columns.Add("Nombre", "Nombre");
            dataGridViewCatalogo.Columns.Add("Artista", "Artista");

            DataGridViewImageColumn imagenColumn = new DataGridViewImageColumn();
            imagenColumn.HeaderText = "Imagen";
            imagenColumn.Name = "Imagen";
            dataGridViewCatalogo.Columns.Add(imagenColumn);

            imagenColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

            dataGridViewCatalogo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Catalogo_Principal_Load(object sender, EventArgs e)
        {
            CargarDatosCanciones();
        }


        private void cellselectedchanged(object sender, EventArgs e)
        {
            if (dataGridViewCatalogo.SelectedRows.Count > 0)
            {
                DataGridViewRow fila = dataGridViewCatalogo.SelectedRows[0];
                int id = Convert.ToInt32(fila.Cells["Id"].Value);
                string nombre = Convert.ToString(fila.Cells["Nombre"].Value);
                string artista = Convert.ToString(fila.Cells["Artista"].Value);
                Image imagenPerfil = null;
                Bitmap imagenBitmap = fila.Cells["Imagen"].Value as Bitmap;

                txtNombre.Text = nombre;
                txtArtista.Text = artista;

                if (imagenBitmap != null)
                {
                    pictureBox2.Image = imagenBitmap;
                    pictureBox3.Image = imagenBitmap;
                }
                else
                {
                    MessageBox.Show("No se encontró una imagen válida para la canción seleccionada.");
                }

            }
        }

        private void CargarDatosCanciones()
        {
            List<Cancion> canciones = daoCanciones.ObtenerTodasLasCanciones();

            dataGridViewCatalogo.Rows.Clear();

            foreach (Cancion cancion in canciones)
            {
                Image imagenPerfil = ByteArrayToImage(cancion.Imagen);

                dataGridViewCatalogo.Rows.Add(cancion.Id, cancion.Nombre, cancion.Artista, imagenPerfil);
            }

            dataGridViewCatalogo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return null;

            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string artista = labelartista.Text;
            byte[] imagen_perfil = null;


            if (pictureBox3.Image != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    pictureBox3.Image.Save(stream, pictureBox1.Image.RawFormat);
                    imagen_perfil = stream.ToArray();
                    if (daoCanciones.InsertarCancion(nombre, artista, imagen_perfil))
                    {
                        MessageBox.Show("La cancion ha sido agregada");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo agregar la cancion");
                    }
                }
            }
            else
            {
                MessageBox.Show("No has ingresado ninguna foto, ingresala porfavor");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cargarExplorador();
        }

        private void cargarExplorador()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";



            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {

                string rutaImagen = openFileDialog.FileName;




                pictureBox3.Image = Image.FromFile(rutaImagen);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Playlists formplaylists = new Playlists(usuarioUser.id);
            this.Hide();
            formplaylists.Show();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCatalogo.SelectedRows.Count > 0)
            {
                DataGridViewRow fila = dataGridViewCatalogo.SelectedRows[0];
                int id = Convert.ToInt32(fila.Cells["Id"].Value);

                DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar esta canción?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    if (daoCanciones.EliminarCancion(id))
                    {
                        MessageBox.Show("La canción ha sido eliminada exitosamente.");
                        CargarDatosCanciones();
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error al intentar eliminar la canción.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una canción para eliminar.");
            }
        }


        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dataGridViewCatalogo.SelectedRows.Count > 0)
            {
                DataGridViewRow fila = dataGridViewCatalogo.SelectedRows[0];
                int id = Convert.ToInt32(fila.Cells["Id"].Value);
                string nombre = txtNombre.Text;
                string artista = txtArtista.Text;
                byte[] imagenPerfil = null;

                if (pictureBox3.Image != null)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            pictureBox3.Image.Save(stream, pictureBox3.Image.RawFormat);
                            imagenPerfil = stream.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar la imagen: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    if (fila.Cells["Imagen"].Value != null)
                    {
                        imagenPerfil = imagenalista(fila.Cells["Imagen"].Value as Image);
                    }
                }

                if (daoCanciones.ActualizarCancion(id, nombre, artista, imagenPerfil))
                {
                    MessageBox.Show("La canción ha sido actualizada exitosamente.");
                    CargarDatosCanciones(); 
                }
                else
                {
                    MessageBox.Show("Hubo un error al intentar actualizar la canción.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una canción para actualizar.");
            }
        }


        private byte[] imagenalista(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

    }
}
