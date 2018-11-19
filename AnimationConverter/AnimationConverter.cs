﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnimationLibrary;

namespace AnimationConverter
{
    public partial class AnimationConverter : Form
    {
        public AnimationConverter()
        {
            InitializeComponent();
        }

        DataSet dataset = null;
        AnimationBundleReader bundles = null;
        static string file = @"D:\GitLab\OxygenNotIncludedMOD\OxygenNotIncludedDev\AnimationConverter\test\rockrefinery_0.png";
        static string path = file.Remove(file.LastIndexOf('_'));
        static string name = file.Substring(file.LastIndexOf("\\") + 1, (file.LastIndexOf(".") - file.LastIndexOf("\\") - 1));

        /// <summary>
        /// 导出图片
        /// </summary>
        private void ExportTexture()
        {
            Directory.CreateDirectory(path);
            for (int frame = 0; frame < dataset.Tables["bildTable"].Rows.Count; frame++)
            {
                var row = dataset.Tables["bildTable"].Rows[frame];
                var img = AnimationTextureExporter.TextureSlicing(
                    file,
                    Convert.ToInt32(row["x1"]), Convert.ToInt32(row["y2"]), Convert.ToInt32(row["x2"]), Convert.ToInt32(row["y1"])
                    );
                img.Save(path + "\\" + row["name"] + "_" + row["index"] + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Image source = Image.FromFile(file);

            bundles = new AnimationBundleReader();
            dataset = bundles.ReadFile(path, source.Width, source.Height);
            dataGridView1.DataSource = dataset.Tables["animTable"];

            ExportTexture();
        }

        private void button2_Click(object sender, EventArgs e)

        {
            AnimationConfigExporter animationConfigExporter = new AnimationConfigExporter();
            animationConfigExporter.InitData(dataset, bundles, path, name);
        }
    }
}