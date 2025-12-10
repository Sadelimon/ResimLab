using System;
using System.Windows;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace ResimLab
{
    
    public partial class MainWindow : System.Windows.Window
    {
        private Mat _src;
        private Mat _working;

        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void BtnYukle_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == true)
            {
                _src = Cv2.ImRead(dlg.FileName);
                _working = _src.Clone();

                SliderParlaklik.Value = 0;
                SliderKontrast.Value = 1.0;

                EkraniGuncelle();
            }
        }

        
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_src == null || _working == null) return;

            double alpha = SliderKontrast.Value;
            double beta = SliderParlaklik.Value;

            Mat sonuc = new Mat();
            _working.ConvertTo(sonuc, -1, alpha, beta);

            ImgAnaEkran.Source = sonuc.ToBitmapSource();
        }

        
        private void BtnSolaDon_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;
            Cv2.Rotate(_working, _working, RotateFlags.Rotate90Counterclockwise);
            EkraniGuncelle();
        }

        private void BtnSagaDon_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;
            Cv2.Rotate(_working, _working, RotateFlags.Rotate90Clockwise);
            EkraniGuncelle();
        }

        private void BtnAyna_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;
            Cv2.Flip(_working, _working, FlipMode.Y);
            EkraniGuncelle();
        }

       
        private void BtnGri_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;

            Mat gri = new Mat();
            Cv2.CvtColor(_working, gri, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(gri, _working, ColorConversionCodes.GRAY2BGR);

            EkraniGuncelle();
        }

        private void BtnBlur_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;
            Cv2.GaussianBlur(_working, _working, new OpenCvSharp.Size(15, 15), 0);
            EkraniGuncelle();
        }

        private void BtnKeskin_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;

            
            float[] data = { 0, -1, 0, -1, 5, -1, 0, -1, 0 };

            Mat kernel = new Mat(3, 3, MatType.CV_32FC1);

           
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    kernel.Set<float>(i, j, data[i * 3 + j]);
                }
            }

            Cv2.Filter2D(_working, _working, -1, kernel);
            EkraniGuncelle();
        }
       
        private void BtnYuzBul_Click(object sender, RoutedEventArgs e)
        {
            
            if (_src == null)
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
                return;
            }

           
            string modelDosyasi = "haarcascade.xml";

            if (!System.IO.File.Exists(modelDosyasi))
            {
                MessageBox.Show($"HATA: '{modelDosyasi}' dosyası bulunamadı!\n\n" +
                                $"Bu dosyayı şu klasöre attığından emin ol:\n" +
                                $"{System.AppDomain.CurrentDomain.BaseDirectory}",
                                "Dosya Hatası", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
               
                var faceCascade = new CascadeClassifier(modelDosyasi);

               
                Mat gri = new Mat();
                Cv2.CvtColor(_working, gri, ColorConversionCodes.BGR2GRAY);

               
                var yuzler = faceCascade.DetectMultiScale(
                    gri,
                    scaleFactor: 1.1,
                    minNeighbors: 4,
                    minSize: new OpenCvSharp.Size(30, 30));

              
                if (yuzler.Length == 0)
                {
                    MessageBox.Show("Resimde yüz bulunamadı. Daha net veya aydınlık bir resim deneyin.", "Sonuç");
                    return;
                }

                
                foreach (var yuz in yuzler)
                {
                    
                    Cv2.Rectangle(_working, yuz, Scalar.Red, 3);

                   
                    Cv2.PutText(_working, "Yuz", new OpenCvSharp.Point(yuz.X, yuz.Y - 10),
                        HersheyFonts.HersheySimplex, 0.7, Scalar.Red, 2);
                }

               
                ImgAnaEkran.Source = _working.ToBitmapSource();

                MessageBox.Show($"{yuzler.Length} adet yüz bulundu ve işaretlendi!", "Başarılı");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Kritik Hata");
            }
        }

        
        private void BtnSifirla_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;
            _working = _src.Clone();
            SliderParlaklik.Value = 0;
            SliderKontrast.Value = 1.0;
            EkraniGuncelle();
        }

        private void BtnKaydet_Click(object sender, RoutedEventArgs e)
        {
            if (_src == null) return;

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "JPG Resmi|*.jpg|PNG Resmi|*.png";

            if (saveDlg.ShowDialog() == true)
            {
                Mat kaydedilecek = new Mat();
                _working.ConvertTo(kaydedilecek, -1, SliderKontrast.Value, SliderParlaklik.Value);
                Cv2.ImWrite(saveDlg.FileName, kaydedilecek);
                MessageBox.Show("Resim kaydedildi!");
            }
        }

        private void EkraniGuncelle()
        {
            Slider_ValueChanged(null, null);
        }
    }
}