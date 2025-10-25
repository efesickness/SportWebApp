# Sport Web App
Bu uygulama; basketbol, yüzme, voleybol gibi çoğu spor dalı için yönetim ve kayıt tutma ihtiyaçlarını karşılayan bir staj projesidir.
B.U.Ü Spor Müdürlüğü ve Spor Kulübü adına geliştirilmiştir.
## Özellikler
- Öğrenci ve antrenör kayıtları alınabiliyor.
- Antrenörler kendi alanlarında kurslar, ve bu kurslar için gruplar açabiliyor.
- Öğrenciler açıktaki kurslara başvurabiliyor ve kayıtları onaylanırsa antrenörleri tarafından gruba atanabiliyor.
- Antrenörler antrenman sistemi ile gruplara antrenman atayabiliyor, bunları takvimde rahatça gözlemleyebiliyor.
- Tekrarlayacak antrenmanlar için çoklu antrenman oluşturma özelliğimiz bulunuyor("İki tarih arası haftada hangi günler" gibi).
- Aidat takip sistemi ile öğrencilerin ödemelerine ne kadar kaldığı görülebiliyor ve aidat yenileme işlemi yapılabiliyor.
- Yetkililer, sistemde tanımlı 30’dan fazla yetkiden istediklerini kullanıcıya verebiliyor veya geri alabiliyor.
## Kullanılan Teknolojiler
- ASP.Net Core MVC
- Entity Framework Core
- SQL Server (MSSQL)
- ASP.Net Identity
## Kurulum
```bash
# Repoyu klonla
git clone https://github.com/efesickness/SportWebApp.git

# Proje dizinine gir
cd SportWebApp/SporWebDeneme1

# Migration çalıştır
dotnet ef database update
```

Proje artık çalıştırmaya hazır:
```bash
dotnet run
```
