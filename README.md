WebApp
1. Открыть `Solution.sln` в Visual Studio 2022.
2. Запустить WebApp (F5).
3. Доступные страницы:
   - `Students.aspx` – список студентов (paging, sorting, edit/delete)
   - `StudentsAdd.aspx` – добавление нового студента
   - `Courses.aspx` – выбор кафедры и отображение связанных курсов
4. **Особенности**: для EntityDataSource заменены строковые настройки подключения на `ContextTypeName` → уменьшен overhead (быстрее и надёжнее).

ConsoleApp
1. Входной файл: `ConsoleApp/deposits.xml`
2. Запуск: через Visual Studio или команду `dotnet run` в папке ConsoleApp
3. Что делает:
   - Читает депозиты из XML
   - Вычисляет APR и EAR для разных периодов капитализации: 12, 4, 1, ∞
   - Пишет отчёт в `apr_ear.txt`:
     ```
     Депозит | Номинальная ставка | APR | EAR
     ```
   - Обрабатывает граничные случаи: нулевая или отрицательная ставка → выбрасывает исключение и логирует в файл
4. Использует: ООП с базовым классом, наследником, extension method и делегаты.

Тестовые данные
- Примеры XML: `ConsoleApp/deposits.xml`
- Список студентов и курсов — в базе `School.mdf` (Database First, контекст `SchoolEntities`)

Проверка
- WebApp: добавление/редактирование/удаление студентов, выбор кафедры корректно работает
- ConsoleApp: отчёт APR/EAR совпадает с 2–3 известными формулами

## Особенности реализации
- Web: `ContextTypeName` вместо строковых настроек в EntityDataSource → меньше overhead
- Console: LINQ to XML для чтения/модификации данных, LINQ to Objects для отчётов, делегаты для обработки событий
