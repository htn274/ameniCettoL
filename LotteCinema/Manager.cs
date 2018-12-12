﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LotteCinema
{
    public partial class fManager : Form
    {
        public fManager()
        {
            InitializeComponent();
        }

        private void loadTabFilm(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("sp_LietKePhim", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_film.DataSource = dt;
            }
        }

        private void loadTabShowtimes(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("sp_LietKeSuatChieu", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_showtime.DataSource = dt;
            }
        }

        private string Date(DateTime dt)
        {
            return dt.Day.ToString() + "/" + dt.Month.ToString() + "/" + dt.Year.ToString();
        }

        private void loadTabStatistic(SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("sp_LietKeRap", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cb_cinema.DataSource = dt;
                cb_cinema.ValueMember = "idrap";
                cb_cinema.DisplayMember = "tenrap";
            }

            using (SqlCommand cmd = new SqlCommand("sp_LietKePhim", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cb_film.DataSource = dt;
                cb_film.ValueMember = "idphim";
                cb_film.DisplayMember = "tuaphim";
            }

            dtpk_dateFrom.Value = DateTime.Today;
            tb_dateFrom.Text = Date(dtpk_dateFrom.Value);
            dtpk_dateTo.Value = DateTime.Today.AddDays(1);
            tb_dateTo.Text = Date(dtpk_dateTo.Value);
        }

        private void fManager_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(SQLConnection.connectionString()))
            {
                conn.Open();
                loadTabFilm(conn);
                loadTabShowtimes(conn);
                loadTabStatistic(conn);
                conn.Close();
            }
        }

        private void btn_statistic_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(SQLConnection.connectionString()))
            using (SqlCommand cmd = new SqlCommand("sp_ThongKe", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@rap", SqlDbType.Int);
                cmd.Parameters.Add("@phim", SqlDbType.Int);
                cmd.Parameters.Add("@ngayBD", SqlDbType.Date);
                cmd.Parameters.Add("@ngayKT", SqlDbType.Date);
                cmd.Parameters["@rap"].Value = int.Parse(cb_cinema.SelectedValue.ToString());
                cmd.Parameters["@phim"].Value = int.Parse(cb_film.SelectedValue.ToString());
                cmd.Parameters["@ngayBD"].Value = dtpk_dateFrom.Value;
                cmd.Parameters["@ngayKT"].Value = dtpk_dateTo.Value;

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cb_film.DataSource = dt;

                conn.Close();
            }
        }

        private void dtpk_dateFrom_ValueChanged(object sender, EventArgs e)
        {
            tb_dateFrom.Text = Date(dtpk_dateFrom.Value);
        }

        private void dtpk_dateTo_ValueChanged(object sender, EventArgs e)
        {
            tb_dateTo.Text = Date(dtpk_dateTo.Value);
        }
    }
}
