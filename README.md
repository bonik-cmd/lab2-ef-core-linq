# Лабораторная работа №2. Entity Framework Core и LINQ

**Вариант 8** — «Клинико-диагностическая лаборатория»

## Описание

Консольное приложение .NET, использующее подход **Database First** и Scaffolding
EF Core для работы с БД `ClinicalLaboratoryDB`, созданной в лабораторной работе №1.

## Стек

- .NET 10
- Entity Framework Core
- MS SQL Server Express
- LINQ

## Строка подключения (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ClinicalLaboratoryDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}