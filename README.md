# DirTree

**Консольна утиліта для відображення структури каталогу** з розмірами файлів і папок, налаштовуваними кольорами та зручним меню.

- Платформи: **Windows · Linux · macOS**
- .NET 8 · Spectre.Console
- Ліцензія: **MIT**

---

## Встановлення

### Windows (зі збірки)
```
1. Завантажте dirtree.exe з розділу Releases на GitHub
2. Покладіть у будь-яку папку, наприклад C:\Tools\
3. Додайте C:\Tools\ до змінної PATH (одноразово):
   [Система] → [Змінні середовища] → PATH → [Змінити] → [Створити] → C:\Tools\
4. Відкрийте новий термінал і запустіть: dirtree
```

### Linux / macOS (зі збірки або GitHub)
```bash
# Клонуйте репозиторій
git clone https://github.com/yourusername/dirtree
cd dirtree

# Збірка та встановлення
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
sudo cp ./publish/dirtree /usr/local/bin/
sudo chmod +x /usr/local/bin/dirtree
```

---

## Збірка з коду

```bash
# Windows — один exe-файл
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/win-x64

# Linux
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/linux-x64

# macOS
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/osx-x64
```

---

## Використання

```
dirtree [шлях] [ключ]
```

Якщо `шлях` не вказано — використовується поточний каталог.
Якщо `ключ` не вказано — запускається інтерактивний режим з меню.

### Ключі

| Ключ | Дія |
|------|-----|
| `--save` | Зберегти дерево у `directory_tree.txt` (з розмірами) |
| `--save-clean` | Зберегти дерево у `directory_tree_clean.txt` (без розмірів) |
| `--copy` | Скопіювати дерево в буфер обміну (з розмірами) |
| `--copy-clean` | Скопіювати дерево в буфер обміну (без розмірів) |
| `--about` | Інформація про програму та автора |
| `--help`, `-h` | Показати довідку |

### Приклади

```bash
# Інтерактивний режим — поточний каталог
dirtree

# Інтерактивний режим — вказаний каталог
dirtree C:\Projects\MyApp

# Зберегти і повернутись до консолі (без меню)
dirtree --save
dirtree C:\Projects\MyApp --save

# Зберегти без розмірів
dirtree --save-clean

# Скопіювати в буфер
dirtree --copy
dirtree C:\Projects\MyApp --copy-clean

# Довідка
dirtree --help

# Про програму
dirtree --about
```

---

## Інтерактивне меню

Без ключів після виведення дерева з'являється меню, яке залишається активним після кожної дії:

```
💾  Зберегти у файл          (з розмірами)
💾  Зберегти у файл          (без розмірів)
📋  Скопіювати в буфер       (з розмірами)
📋  Скопіювати в буфер       (без розмірів)
ℹ️  Про програму та автора
🚪  Вийти з утиліти
🔴  Вийти з утиліти та закрити термінал
```

---

## Налаштування кольорів — dirtree.config

При першому запуску поруч з `dirtree.exe` автоматично створюється `dirtree.config`.
Відкрийте у будь-якому редакторі та змінюйте кольори у форматі `#RRGGBB`.

### Параметри

| Параметр | Що фарбує |
|----------|-----------|
| `TreeLines` | символи `├──` `└──` `│` |
| `DirectoryName` | назва папки |
| `FileName` | назва файлу (невідоме розширення) |
| `SizeLabel` | підпис `[розмір]` |
| `RootPath` | шлях-заголовок |
| `ExtCode` | `.cs` `.py` `.js` `.ts` `.go` `.rs` `.java` `.cpp` |
| `ExtConfig` | `.json` `.xml` `.yaml` `.toml` `.ini` `.cfg` `.env` |
| `ExtText` | `.md` `.txt` `.log` |
| `ExtImage` | `.png` `.jpg` `.gif` `.svg` `.webp` |
| `ExtDocument` | `.pdf` `.doc` `.docx` `.xls` `.xlsx` `.pptx` |
| `ExtArchive` | `.zip` `.tar` `.gz` `.rar` `.7z` |
| `ExtBinary` | `.exe` `.dll` `.so` `.dylib` |
| `ExtScript` | `.sh` `.bat` `.cmd` `.ps1` |
| `ExtWeb` | `.html` `.css` `.scss` `.php` |
| `SummaryPath` | шлях у панелі підсумку |
| `SummarySize` | розмір у панелі підсумку |

### Готові теми

**Світла тема (для терміналів зі світлим фоном)**
```ini
TreeLines     = #999999
DirectoryName = #0055cc
FileName      = #111111
SizeLabel     = #999999
RootPath      = #cc7700
ExtCode       = #007700
ExtConfig     = #886600
ExtText       = #333333
ExtImage      = #880088
ExtDocument   = #cc0000
ExtArchive    = #cc5500
ExtBinary     = #aa0000
ExtScript     = #447700
ExtWeb        = #0077cc
SummaryPath   = #cc7700
SummarySize   = #007700
```

**Монохромна тема**
```ini
TreeLines     = #444444
DirectoryName = #dddddd
FileName      = #aaaaaa
SizeLabel     = #444444
RootPath      = #ffffff
ExtCode       = #aaaaaa
ExtConfig     = #aaaaaa
ExtText       = #aaaaaa
ExtImage      = #aaaaaa
ExtDocument   = #aaaaaa
ExtArchive    = #aaaaaa
ExtBinary     = #aaaaaa
ExtScript     = #aaaaaa
ExtWeb        = #aaaaaa
SummaryPath   = #ffffff
SummarySize   = #aaaaaa
```

---

## Підтримка буфера обміну

| ОС | Інструмент |
|----|-----------|
| Windows | `clip.exe` (вбудований) |
| macOS | `pbcopy` (вбудований) |
| Linux | `xclip`, `xsel` або `wl-copy` |

```bash
# Приклад команди встановлення xclip в Linux
sudo apt install xclip
```

## Примітка:
``xclip`` працює тільки з ``X11`` та не запускається на ``Wayland``. Якщо ви працюєте на ``Wayland``, використовуйте замість нього ``wl-clipboard`` (``wl-copy`` та ``wl-paste``).


## Важлива інформація по запуску

- При подвійному кліку утиліта виводить попередження і завершується
- Папки сортуються першими, потім файли (алфавітно)
- Недоступні каталоги позначаються як `<немає доступу>`
- `dirtree.config` створюється автоматично поруч з ``exe`` при першому запуску
- Некоректний колір в конфігу ігнорується — використовується значення за замовчуванням
