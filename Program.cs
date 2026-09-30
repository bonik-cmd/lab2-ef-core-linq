using Microsoft.EntityFrameworkCore;
using Lab2.Models;   // ← уточните namespace по первой строке Patient.cs

var options = new DbContextOptionsBuilder<ClinicalLaboratoryDbContext>()
    .UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=ClinicalLaboratoryDB;Trusted_Connection=True;TrustServerCertificate=True")
    .Options;

using var db = new ClinicalLaboratoryDbContext(options);
Console.OutputEncoding = System.Text.Encoding.UTF8;

// 2.1 — все данные из таблицы на стороне "один" (PATIENTS)
Console.WriteLine("=== 2.1. Все пациенты (первые 10) ===");
foreach (var p in db.Patients.Take(10))
    Console.WriteLine($"{p.PatientId} | {p.FullName} | {p.BirthDate} | {p.Gender}");
Console.WriteLine($"Всего: {db.Patients.Count()}\n");

// 2.2 — фильтр по нескольким полям (DOCTORS)
Console.WriteLine("=== 2.2. Врачи с фильтром ===");
var doctors = db.Doctors
    .Where(d => d.Specialty == "Кардиолог" && d.MedicalOrgName.Contains("поликлиника №3"))
    .ToList();
foreach (var d in doctors)
    Console.WriteLine($"{d.FullName} | {d.Specialty} | {d.MedicalOrgName}");
Console.WriteLine($"Найдено: {doctors.Count}\n");

// 2.3 — группировка по полю таблицы "многие" (TEST_RESULTS) с агрегатом
Console.WriteLine("=== 2.3. Статистика по показателям ===");
var stats = db.TestResults
    .GroupBy(r => r.IndicatorId)
    .Select(g => new
    {
        IndicatorId = g.Key,
        Total = g.Count(),
        Deviated = g.Count(r => r.DeviationStatus != "В норме")
    })
    .Take(10)
    .ToList();
foreach (var s in stats)
    Console.WriteLine($"Показатель {s.IndicatorId}: всего={s.Total}, с отклонением={s.Deviated}");
Console.WriteLine();

// 2.4 — два поля из двух таблиц, связанных 1:M (ORDER + PATIENT)
Console.WriteLine("=== 2.4. Заказ + имя пациента ===");
var orders = db.Orders
    .Select(o => new
    {
        o.OrderId,
        PatientName = o.Patient.FullName,
        o.CreatedAt
    })
    .Take(10)
    .ToList();
foreach (var r in orders)
    Console.WriteLine($"Заказ {r.OrderId} | {r.PatientName} | {r.CreatedAt:dd.MM.yyyy}");
Console.WriteLine();

// 2.5 — то же + фильтр
Console.WriteLine("=== 2.5. Заказы с фильтром ===");
var filteredOrders = db.Orders
    .Where(o => o.OverallStatus == "В обработке"
             && o.Patient.Address.Contains("Пациентская"))
    .Select(o => new
    {
        o.OrderId,
        Patient = o.Patient.FullName,
        o.PaymentStatus,
        o.OverallStatus
    })
    .Take(10)
    .ToList();
foreach (var r in filteredOrders)
    Console.WriteLine($"Заказ {r.OrderId} | {r.Patient} | {r.PaymentStatus} | {r.OverallStatus}");
Console.WriteLine();

// 2.6 — вставка в таблицу "один" (PATIENT)
Console.WriteLine("=== 2.6. Вставка пациента ===");
var newPatient = new Patient
{
    FullName = "Тестов Тест Тестович",
    BirthDate = new DateOnly(1990, 5, 15),   // если ошибка — замените на new DateTime(1990,5,15)
    Gender = "Мужской",
    Phone = "+375-29-000-00-00",
    PassportNumber = "MP9999999",
    Address = "г. Гомель, ул. Тестовая, д. 1"
};
db.Patients.Add(newPatient);
db.SaveChanges();
Console.WriteLine($"Добавлен пациент Id = {newPatient.PatientId}\n");

// 2.7 — вставка в таблицу "многие" (ORDER)
Console.WriteLine("=== 2.7. Вставка заказа ===");
var newOrder = new Order
{
    PatientId = newPatient.PatientId,
    DoctorId = 1,
    CreatedAt = DateTime.Now,
    PaymentStatus = "Ожидает оплаты",
    OverallStatus = "В обработке"
};
db.Orders.Add(newOrder);
db.SaveChanges();
Console.WriteLine($"Добавлен заказ Id = {newOrder.OrderId}\n");

// 2.8 — удаление из таблицы "один" (PATIENT) вместе с зависимыми ORDERS
Console.WriteLine("=== 2.8. Удаление пациента ===");
var pToDel = db.Patients
    .Include(p => p.Orders)
    .FirstOrDefault(p => p.PassportNumber == "MP9999999");
if (pToDel != null)
{
    db.Orders.RemoveRange(pToDel.Orders);
    db.Patients.Remove(pToDel);
    db.SaveChanges();
    Console.WriteLine("Пациент удалён вместе с заказами\n");
}

// 2.9 — удаление из таблицы "многие" (ORDER) вместе с SAMPLES
Console.WriteLine("=== 2.9. Удаление заказа ===");
var oToDel = db.Orders
    .Include(o => o.Samples)
    .OrderByDescending(o => o.OrderId)
    .FirstOrDefault();
if (oToDel != null)
{
    db.Samples.RemoveRange(oToDel.Samples);
    db.Orders.Remove(oToDel);
    db.SaveChanges();
    Console.WriteLine($"Заказ {oToDel.OrderId} удалён\n");
}

// 2.10 — обновление по условию
Console.WriteLine("=== 2.10. Обновление статусов ===");
var toUpd = db.Orders
    .Where(o => o.PaymentStatus == "Отменено" && o.OverallStatus != "Отменён")
    .ToList();
foreach (var o in toUpd) o.OverallStatus = "Отменён";
db.SaveChanges();
Console.WriteLine($"Обновлено: {toUpd.Count}\n");

Console.WriteLine("=== Готово! ===");