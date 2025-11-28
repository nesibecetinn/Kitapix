# 📚 Kitapix

## 📌 Proje Hakkında

Kitapix, Microsoft Visual Studio üzerinde geliştirilen çok katmanlı bir
.NET çözümüdür. Bu proje; içerik yönetimi, kitap işleme, veri katmanları
veya API yapıları gibi modülleri içerebilecek esnek bir çözüm mimarisi
sunar.

------------------------------------------------------------------------


## 🗂 Çözüm Yapısı


`Kitapix.sln` içerisinde birden fazla proje modülü bulunmaktadır. Yapı
Visual Studio'nun standart solution formatına uygun olarak
düzenlenmiştir.


```
 Kitapix.sln
└── src/
    ├── Kitapix.Domain
    ├── Kitapix.Application
    ├── Kitapix.Infrastructure
    └── Kitapix.WebAPI
```


---

### **1. Kitapix.Domain**

- İş kurallarının ve çekirdek modellerin bulunduğu katmandır.  
- `Entity`  tanımları burada yer alır.

---

### **2. Kitapix.Application**

- Kullanım senaryolarının (**Use Cases**) yer aldığı katmandır.  
- CQRS, MediatR veya Validator yapılarını içerir.  
- Domain ile Infrastructure arasındaki **mantıksal köprü** görevini görür.

---

### **3. Kitapix.Infrastructure**

- Veritabanı erişimi, üçüncü parti servis bağlantıları ve dış bağımlılıklar bu katmandadır.  
- Repository implementasyonları bu katmanda bulunur.

---

### **4. Kitapix.WebAPI**

- Uygulamaya HTTP üzerinden erişilen giriş noktasıdır.  
- Controller'lar ve API endpoint’leri burada yer alır.

------------------------------------------------------------------------

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler

-   Visual Studio 2019/2022
-   .NET Framework veya .NET 9 
-   NuGet paket yöneticisi

### Klonlama

``` bash
git clone https://github.com/kullanici/Kitapix.git
cd Kitapix
```

### Açılış

`Kitapix.sln` dosyasını Visual Studio ile açın.

### Bağımlılıkların Yüklenmesi

Visual Studio → **Build** → **Restore NuGet Packages**

### Çalıştırma

- Ana projeyi seçin ve **F5** ile debug modunda başlatın.
- `https://localhost:44390/scalar/v1` adresinden endpointleri test edebilirsiniz 

------------------------------------------------------------------------

## 📬 İletişim

E‑posta:nesibe3cetin@gmail.com
