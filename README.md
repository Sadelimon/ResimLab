# ResimLab

**ResimLab**, C# (WPF) ve **OpenCvSharp** kütüphanesi kullanılarak geliştirilmiş; matematiksel matris işlemleri ve makine öğrenimi (ML) tekniklerini birleştiren bir masaüstü resim düzenleme aracıdır.

##  Özellikler

Bu proje, görüntü işlemeyi matematiksel ve algoritmik temellerle uygular:

*  Yapay Zeka (ML): Haar Cascade algoritması kullanarak fotoğraflardaki yüzleri tespit eder ve işaretler.
*  Piksel Matematiği: Lineer dönüşümlerle Parlaklık ve Kontrast ayarı.
*  Konvolüsyon Filtreleri: Matris çekirdekleri (Kernel) ile Keskinleştirme (Sharpen), Bulanıklaştırma (Blur) ve Gri Tonlama.
*  Geometrik Dönüşümler: Döndürme (Rotation) ve Aynalama (Flip).

##  Kullanılan Teknolojiler

* C# & .NET 6.0/7.0 (WPF)
* OpenCvSharp4 (OpenCV Wrapper)
* Haar Cascade Classifiers (Model)

##  Kurulum ve Çalıştırma

1.  Projeyi klonlayın veya indirin.
2.  `ResimLab.sln` dosyasını **Visual Studio 2022** ile açın.
3.  Projeyi derleyin (Build).
4.  *Önemli: Yüz tanıma özelliğinin çalışması için `haarcascade.xml` dosyasının derleme klasöründe (`bin/Debug/net6.0-windows/`) olduğundan emin olun. (Proje ayarlarında otomatik kopyalama aktiftir).

##  Lisans 

Bu proje eğitim amaçlı geliştirilmiştir.
* Görüntü işleme altyapısı için [OpenCV](https://opencv.org/) kullanılmıştır.
* Yüz tanıma modeli Intel'in açık kaynaklı Haar Cascade modelini kullanır (Lisans metni model dosyası içindedir).
