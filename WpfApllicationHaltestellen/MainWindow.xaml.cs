using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApllicationHaltestellen
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Haltestelle zuletztAngeklicktHaltestelle;
        Haltestelle[] haltestellen;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                string dateiname = ofd.FileName;
                string[] zeilen = File.ReadAllLines(dateiname);
                haltestellen = new Haltestelle[zeilen.Length - 1];
                //for (int i = 1; i < 40; i++)
                for (int i = 1; i < zeilen.Length; i++)
                {
                    string[] teile = zeilen[i].Split(';');
                    double länge = double.Parse(teile[5]);
                    double breite = double.Parse(teile[6]);
                    haltestellen[i - 1] = new Haltestelle(teile[3], new GeoPunkt(länge, breite));
                }

                double minLänge = haltestellen.Min(x => x.Position.Lon);
                double maxLänge = haltestellen.Max(x => x.Position.Lon);
                double minBreite = haltestellen.Min(x => x.Position.Lat);
                double maxBreite = haltestellen.Max(x => x.Position.Lat);

                for (int d = 0; d < 2; d++)
                {
                    for (int i = 0; i < haltestellen.Length; i++)
                    {
                        if (d == 0 && !haltestellen[i].IstHbf || d == 1 && haltestellen[i].IstHbf)
                        {
                            Haltestelle h = haltestellen[i];
                            Ellipse elli = new Ellipse();
                            elli.Width = 5.0;
                            elli.Height = 5.0;
                            elli.Fill = h.IstHbf ? Brushes.Red : Brushes.DarkBlue;
                            elli.ToolTip = h.Ort;
                            elli.Tag = h; //Mekt sich alles vom h
                            elli.MouseDown += ellipseGeklickt;

                            zeichenfläche.Children.Add(elli);

                            Canvas.SetLeft(elli, zeichenfläche.ActualWidth / (maxLänge - minLänge) * (h.Position.Lon - minLänge));
                            Canvas.SetBottom(elli, zeichenfläche.ActualHeight / (maxBreite - minBreite) * (h.Position.Lat - minBreite));
                        }
                    }
                }
                //for (int i = 0; i < haltestellen.Length; i++)
                //{
                //    if (haltestellen[i].IstHbf)
                //    {
                //        Ellipse elli = new Ellipse();
                //        elli.Width = 5.0;
                //        elli.Height = 5.0;
                //        elli.Fill = Brushes.Red;
                //        elli.ToolTip = haltestellen[i].Ort;

                //        zeichenfläche.Children.Add(elli);

                //        Canvas.SetLeft(elli, zeichenfläche.ActualWidth / (maxLänge - minLänge) * (haltestellen[i].Position.Lon - minLänge));
                //        Canvas.SetBottom(elli, zeichenfläche.ActualHeight / (maxBreite - minBreite) * (haltestellen[i].Position.Lat - minBreite));
                //    }
                //}
            }
        }

        private void ellipseGeklickt(object sender, MouseButtonEventArgs e)
        {

            Haltestelle soebenAngeklickteHaltestelle = (Haltestelle)((Ellipse)sender).Tag;
            //MessageBox.Show(((Haltestelle)((Ellipse)sender).Tag).Ort);
            if (zuletztAngeklicktHaltestelle != null)
            {

                MessageBox.Show(GeoPunkt.BestimmeAbstand(soebenAngeklickteHaltestelle.Position,zuletztAngeklicktHaltestelle.Position).ToString());
            }
            zuletztAngeklicktHaltestelle = soebenAngeklickteHaltestelle;
        }
    }

    class Haltestelle
    {
        private string ort;
        GeoPunkt position;

        public Haltestelle(string ort, GeoPunkt position)
        {
            this.ort = ort;
            this.position = position;
        }

        public string Ort { get => ort; set => ort = value; }
        internal GeoPunkt Position { get => position; }

        public bool IstHbf
        {
            get
            {
                return ort.EndsWith("Hbf");
            }
        }

    }
}
