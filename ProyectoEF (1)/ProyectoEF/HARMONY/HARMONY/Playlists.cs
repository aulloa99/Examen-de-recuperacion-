using HARMONY.modelos;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace HARMONY
{
    public partial class Playlists : Form
    {
        private int idelUsuario;

        public Playlists(int idUsuario)
        {
            this.idelUsuario = idUsuario;
            InitializeComponent();
            CargarPlaylistsEnComboBox();
            CargarCancionesEnComboBox();
            playListcombo.SelectedIndexChanged += PlayListcombo_SelectedIndexChanged;
        }

        private void MainForm_Load()
        {
            CargarPlaylistsEnComboBox();
        }

        private void CargarPlaylistsEnComboBox()
        {
            DaoPlaylist playlistDao = new DaoPlaylist();
            List<Playlist> playlists = playlistDao.ObtenerPlaylistsPorUsuario(idelUsuario);

            playListcombo.DataSource = playlists;
            playListcombo.DisplayMember = "Nombre";
            playListcombo.ValueMember = "Id";
        }

        private void CargarCancionesEnComboBox()
        {
            DaoCanciones cancionDao = new DaoCanciones();
            List<Cancion> canciones = cancionDao.ObtenerTodasLasCanciones();

            comboCanciones.DataSource = canciones;
            comboCanciones.DisplayMember = "Nombre";
            comboCanciones.ValueMember = "Id";
        }

        private void PlayListcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playListcombo.SelectedValue is int idPlaylist)
            {
                CargarCancionesDePlaylist(idPlaylist);
            }
        }

        private void CargarCancionesDePlaylist(int idPlaylist)
        {
            DaoPlaylist playlistDao = new DaoPlaylist();
            List<Cancion> canciones = playlistDao.ObtenerCancionesDePlaylist(idPlaylist);

            dataGridView1.DataSource = canciones;
        }

        private void btnAgregarCancion_Click(object sender, EventArgs e)
        {
            int idPlaylist = (int)playListcombo.SelectedValue;
            int idCancion = (int)comboCanciones.SelectedValue;

            DaoPlaylist playlistDao = new DaoPlaylist();
            playlistDao.AgregarCancionAPlaylist(idPlaylist, idCancion);

            MessageBox.Show("Canción agregada a la playlist exitosamente.");

            CargarCancionesDePlaylist(idPlaylist);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idPlaylist = (int)playListcombo.SelectedValue;

            int idCancion = (int)comboCanciones.SelectedValue;

            DaoPlaylist playlistDao = new DaoPlaylist();
            playlistDao.AgregarCancionAPlaylist(idPlaylist, idCancion);

            CargarCancionesDePlaylist(idPlaylist);

            MessageBox.Show("Canción agregada a la playlist exitosamente.");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DaoPlaylist playlistdao = new DaoPlaylist();

            string nombrPlaylist = nombrePlaylist.Text;

            

            playlistdao.InsertarPlaylist(nombrPlaylist, idelUsuario);
            CargarPlaylistsEnComboBox();
        }
    }
}
