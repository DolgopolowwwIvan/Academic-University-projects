# PriceList Manager - Прайс-лист фирмы

Веб-приложение для управления каталогом товаров компании.

## Технологический стек

- **Бэкенд:** C#, .NET 8 (ASP.NET Core MVC)
- **Фронтенд:** React 18, Tailwind CSS, Vite
- **База данных:** PostgreSQL 15 (Docker)
- **Аутентификация:** JWT токены

## Структура проекта

```
PriceListManager/
├── PriceListManager.API/       # Бэкенд (ASP.NET Core)
│   ├── Controllers/            # API контроллеры
│   ├── Models/                 # Модели данных
│   ├── Services/               # Бизнес-логика
│   ├── Data/                   # Контекст БД и миграции
│   └── wwwroot/                # Статические файлы (SPA)
├── PriceListManager.Client/    # React клиент
│   └── src/
│       ├── components/         # React компоненты
│       ├── pages/              # Страницы
│       └── services/           # API сервисы
├── docker-compose.yml          # Docker Compose для БД
└── PriceListManager.sln        # Solution файл
```

## Требования

- .NET 8 SDK
- Node.js 18+ и npm
- Docker (для PostgreSQL)

## Запуск

### 1. Запуск PostgreSQL

```bash
docker compose up -d
```

### 2. Запуск бэкенда

```bash
cd PriceListManager.API
dotnet restore
dotnet run
```

Бэкенд будет доступен по адресу: http://localhost:5000

### 3. Запуск фронтенда (в отдельном терминале)

```bash
cd PriceListManager.Client
npm install
npm run dev
```

Фронтенд будет доступен по адресу: http://localhost:3000

## Тестовые пользователи

| Логин    | Пароль     | Роль        |
|----------|------------|-------------|
| admin    | Admin123!  | SuperAdmin  |
| manager  | Admin123!  | Admin       |
| user     | Admin123!  | User        |

## Функциональность

### Авторизация и аутентификация
- Вход по логину/email и паролю
- JWT токены с истечением через 30 минут
- Разграничение прав доступа по ролям

### Товары
- Просмотр списка товаров с фильтрацией
- Поиск по названию и артикулу
- Расширенный поиск (по категории, цене, наличию)
- Добавление/редактирование/удаление товаров (для администраторов)
- Перемещение товаров между категориями
- Массовое перемещение товаров

### Категории
- Управление категориями (для администраторов)
- Просмотр количества товаров в каждой категории

### Пользователи (только SuperAdmin)
- Просмотр списка пользователей
- Добавление новых администраторов
- Изменение ролей пользователей

## API Endpoints

### Аутентификация
- `POST /api/auth/login` - Вход
- `POST /api/auth/logout` - Выход

### Категории
- `GET /api/categories` - Список категорий
- `POST /api/categories` - Создание категории
- `PUT /api/categories/{id}` - Обновление категории
- `DELETE /api/categories/{id}` - Удаление категории

### Товары
- `GET /api/products` - Список товаров
- `GET /api/products/search` - Расширенный поиск
- `POST /api/products` - Создание товара
- `PUT /api/products/{id}` - Обновление товара
- `DELETE /api/products/{id}` - Удаление товара
- `POST /api/products/{id}/move` - Перемещение товара
- `POST /api/products/move-bulk` - Массовое перемещение

### Пользователи
- `GET /api/users` - Список пользователей
- `POST /api/users/admin` - Создание администратора
- `PUT /api/users/{id}/role` - Изменение роли
