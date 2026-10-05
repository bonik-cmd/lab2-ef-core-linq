using Microsoft.EntityFrameworkCore;
using Lab2.Models;   // ← уточните namespace по Patient.cs

var options = new DbContextOptionsBuilder<ClinicalLaboratoryDbContext>()
    .UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=ClinicalLaboratoryDB;Trusted_Connection=True;TrustServerCertificate=True")
    .Options;

using var db = new ClinicalLaboratoryDbContext(options);
Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    Console.Clear();
    Console.WriteLine("========== ЛАБОРАТОРНАЯ РАБОТА №2 ==========");
    Console.WriteLine("2.1.  Все пациенты");
    Console.WriteLine("2.2.  Врачи с фильтром");
    Console.WriteLine("2.3.  Статистика по показателям");
    Console.WriteLine("2.4.  Заказы + имена пациентов");
    Console.WriteLine("2.5.  Заказы с фильтром");
    Console.WriteLine("2.6.  Добавить пациента");
    Console.WriteLine("2.7.  Добавить заказ");
    Console.WriteLine("2.8.  Удалить пациента");
    Console.WriteLine("2.9.  Удалить последний заказ");
    Console.WriteLine("2.10. Обновить статусы заказов");
    Console.WriteLine("0.    Выход");
    Console.WriteLine("============================================");
    Console.Write("Выберите пункт: ");

    string choice = Console.ReadLine()?.Trim() ?? "";

    try
    {
        switch (choice)
        {
            case "2.1": Task21(db); break;
            case "2.2": Task22(db); break;
            case "2.3": Task23(db); break;
            case "2.4": Task24(db); break;
            case "2.5": Task25(db); break;
            case "2.6": Task26(db); break;
            case "2.7": Task27(db); break;
            case "2.8": Task28(db); break;
            case "2.9": Task29(db); break;
            case "2.10": Task210(db); break;
            case "0": return;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[Ошибка] {ex.Message}");
    }

    Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
    Console.ReadLine();
}

// ============================================================
// 2.1. Выборка ВСЕХ данных из таблицы на стороне "один"
// ============================================================
static void Task21(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.1. Все пациенты (первые 20) ===");
    var rows = db.Patients.Take(20).ToList();
    foreach (var p in rows)
        Console.WriteLine($"{p.PatientId} | {p.FullName} | {p.BirthDate} | {p.Gender}");
    Console.WriteLine($"\nВсего в таблице PATIENTS: {db.Patients.Count()}");
}

// ============================================================
// 2.2. Выборка с фильтром
// ============================================================
static void Task22(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.2. Врачи-кардиологи из поликлиники №5 (первые 20) ===");

    var query = db.Doctors
        .Where(d => d.Specialty == "Кардиолог"
                 && d.MedicalOrgName.Contains("поликлиника №5"));

    var rows = query.Take(20).ToList();
    foreach (var d in rows)
        Console.WriteLine($"{d.FullName} | {d.Specialty} | {d.MedicalOrgName}");
    Console.WriteLine($"\nВсего найдено: {query.Count()}");
}

// ============================================================
// 2.3. Группировка по полю "многие" с агрегатом
// ============================================================
static void Task23(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.3. Статистика по показателям (первые 20) ===");
    var query = db.TestResults
        .GroupBy(r => r.IndicatorId)
        .Select(g => new
        {
            IndicatorId = g.Key,
            Total = g.Count(),
            Deviated = g.Count(r => r.DeviationStatus != "В норме")
        });

    var rows = query.Take(20).ToList();
    foreach (var s in rows)
        Console.WriteLine($"Показатель {s.IndicatorId}: всего={s.Total}, с отклонением={s.Deviated}");
    Console.WriteLine($"\nВсего групп (показателей): {query.Count()}");
}

// ============================================================
// 2.4. Два поля из двух таблиц, связанных 1:M
// ============================================================
static void Task24(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.4. Заказ + имя пациента (первые 20) ===");
    var query = db.Orders
        .Select(o => new
        {
            o.OrderId,
            PatientName = o.Patient.FullName,
            o.CreatedAt
        });

    var rows = query.Take(20).ToList();
    foreach (var r in rows)
        Console.WriteLine($"Заказ {r.OrderId} | {r.PatientName} | {r.CreatedAt:dd.MM.yyyy}");
    Console.WriteLine($"\nВсего записей: {query.Count()}");
}

// ============================================================
// 2.5. Соединение 1:M с фильтром
// ============================================================
static void Task25(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.5. Заказы 'В обработке' у пациентов с 'Пациентская' (первые 20) ===");
    var query = db.Orders
        .Where(o => o.OverallStatus == "В обработке"
                 && o.Patient.Address.Contains("Пациентская"))
        .Select(o => new
        {
            o.OrderId,
            Patient = o.Patient.FullName,
            o.PaymentStatus,
            o.OverallStatus
        });

    var rows = query.Take(20).ToList();
    foreach (var r in rows)
        Console.WriteLine($"Заказ {r.OrderId} | {r.Patient} | {r.PaymentStatus} | {r.OverallStatus}");
    Console.WriteLine($"\nВсего найдено: {query.Count()}");
}

// ============================================================
// 2.6. Вставка в таблицу "один"
// ============================================================
static void Task26(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.6. Добавление пациента ===");

    Console.Write("ФИО: ");
    string fullName = Console.ReadLine() ?? "";

    Console.Write("Дата рождения (гггг-мм-дд): ");
    DateOnly birthDate = DateOnly.Parse(Console.ReadLine() ?? "");

    Console.Write("Пол (Мужской/Женский): ");
    string gender = Console.ReadLine() ?? "";

    Console.Write("Телефон: ");
    string phone = Console.ReadLine() ?? "";

    Console.Write("Номер паспорта: ");
    string passport = Console.ReadLine() ?? "";

    Console.Write("Адрес: ");
    string address = Console.ReadLine() ?? "";

    var patient = new Patient
    {
        FullName = fullName,
        BirthDate = birthDate,
        Gender = gender,
        Phone = phone,
        PassportNumber = passport,
        Address = address
    };

    db.Patients.Add(patient);
    db.SaveChanges();
    Console.WriteLine($"\nПациент добавлен. Id = {patient.PatientId}");
    Console.WriteLine($"Всего пациентов в таблице: {db.Patients.Count()}");
}

// ============================================================
// 2.7. Вставка в таблицу "многие"
// ============================================================
static void Task27(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.7. Добавление заказа ===");

    Console.Write("Id пациента: ");
    int patientId = int.Parse(Console.ReadLine() ?? "0");

    Console.Write("Id врача: ");
    int doctorId = int.Parse(Console.ReadLine() ?? "0");

    Console.Write("Статус оплаты (Ожидает оплаты/Оплачено/Отменено): ");
    string payment = Console.ReadLine() ?? "Ожидает оплаты";

    var order = new Order
    {
        PatientId = patientId,
        DoctorId = doctorId,
        CreatedAt = DateTime.Now,
        PaymentStatus = payment,
        OverallStatus = "В обработке"
    };

    db.Orders.Add(order);
    db.SaveChanges();
    Console.WriteLine($"\nЗаказ добавлен. Id = {order.OrderId}");
    Console.WriteLine($"Всего заказов в таблице: {db.Orders.Count()}");
}

// ============================================================
// 2.8. Удаление из таблицы "один"
// ============================================================
static void Task28(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.8. Удаление пациента по Id ===");

    Console.Write("Id пациента: ");
    int id = int.Parse(Console.ReadLine() ?? "0");

    var patient = db.Patients
        .Include(p => p.Orders)
        .FirstOrDefault(p => p.PatientId == id);

    if (patient == null)
    {
        Console.WriteLine("Пациент не найден.");
        return;
    }

    db.Orders.RemoveRange(patient.Orders);
    db.Patients.Remove(patient);
    db.SaveChanges();
    Console.WriteLine($"Пациент {id} и его заказы удалены.");
    Console.WriteLine($"Всего пациентов осталось: {db.Patients.Count()}");
}

// ============================================================
// 2.9. Удаление из таблицы "многие"
// ============================================================
static void Task29(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.9. Удаление заказа по Id ===");

    Console.Write("Id заказа: ");
    int id = int.Parse(Console.ReadLine() ?? "0");

    var order = db.Orders
        .Include(o => o.Samples)
        .FirstOrDefault(o => o.OrderId == id);

    if (order == null)
    {
        Console.WriteLine("Заказ не найден.");
        return;
    }

    db.Samples.RemoveRange(order.Samples);
    db.Orders.Remove(order);
    db.SaveChanges();
    Console.WriteLine($"Заказ {id} и связанные пробы удалены.");
    Console.WriteLine($"Всего заказов осталось: {db.Orders.Count()}");
}

// ============================================================
// 2.10. Обновление по условию
// ============================================================
static void Task210(ClinicalLaboratoryDbContext db)
{
    Console.WriteLine("\n=== 2.10. Обновление статусов ===");
    Console.WriteLine("Всем заказам с payment_status='Отменено'");
    Console.WriteLine("проставить overall_status='Отменён'.");

    var list = db.Orders
        .Where(o => o.PaymentStatus == "Отменено" && o.OverallStatus != "Отменён")
        .ToList();

    foreach (var o in list)
        o.OverallStatus = "Отменён";

    db.SaveChanges();
    Console.WriteLine($"\nОбновлено заказов: {list.Count}");
    Console.WriteLine($"Всего заказов в таблице: {db.Orders.Count()}");
}