const express = require('express');
const path = require('path');
const { Pool } = require('pg');

const app = express();
const PORT = process.env.PORT || 3000;

app.use(express.json());
app.use(express.static(path.join(__dirname, 'public')));

// Подключение к PostgreSQL
const pool = new Pool({
    host: process.env.DB_HOST || 'localhost',
    user: process.env.DB_USER || 'postgres',
    password: process.env.DB_PASSWORD || 'postgres',
    database: process.env.DB_NAME || 'comments',
    port: 5432
});

// Проверка подключения к БД с повторными попытками
async function waitForDatabase(maxRetries = 30, delay = 1000) {
    for (let i = 1; i <= maxRetries; i++) {
        try {
            await pool.query('SELECT 1');
            console.log('Подключение к базе данных успешно');
            return true;
        } catch (error) {
            console.log(`Попытка подключения к БД ${i}/${maxRetries} не удалась, ждём...`);
            if (i === maxRetries) {
                throw new Error('Не удалось подключиться к базе данных после ' + maxRetries + ' попыток');
            }
            await new Promise(resolve => setTimeout(resolve, delay));
        }
    }
}

// Создание таблицы при старте сервера
async function initDatabase() {
    try {
        // Ждём готовности БД
        await waitForDatabase();
        
        // Используем кавычки для "user" — это зарезервированное слово в PostgreSQL
        await pool.query(`
            CREATE TABLE IF NOT EXISTS comments (
                id BIGINT PRIMARY KEY,
                text TEXT NOT NULL,
                date TEXT NOT NULL,
                "user" TEXT NOT NULL,
                author TEXT NOT NULL
            )
        `);
        console.log('Таблица comments создана/проверена');
    } catch (error) {
        console.error('Ошибка инициализации базы данных:', error);
        throw error;
    }
}

// Получить все комментарии
app.get('/api/comments', async (req, res) => {
    try {
        const result = await pool.query('SELECT * FROM comments ORDER BY id DESC');
        res.json(result.rows);
    } catch (error) {
        console.error('Ошибка получения комментариев:', error);
        res.status(500).json({ error: 'Ошибка получения комментариев' });
    }
});

// Получить комментарии пользователя
app.get('/api/comments/:user', async (req, res) => {
    try {
        const { user } = req.params;
        const result = await pool.query(
            'SELECT * FROM comments WHERE "user" = $1 ORDER BY id DESC',
            [user]
        );
        res.json(result.rows);
    } catch (error) {
        console.error('Ошибка получения комментариев:', error);
        res.status(500).json({ error: 'Ошибка получения комментариев' });
    }
});

// Добавить комментарий
app.post('/api/comments', async (req, res) => {
    try {
        const { text, user } = req.body;
        
        if (!text || !user) {
            return res.status(400).json({ error: 'Текст комментария и пользователь обязательны' });
        }
        
        const newComment = {
            id: Date.now(),
            text: text.trim(),
            date: new Date().toLocaleString('ru-RU'),
            user: user,
            author: 'Анонимный пользователь'
        };
        
        await pool.query(
            'INSERT INTO comments (id, text, date, "user", author) VALUES ($1, $2, $3, $4, $5)',
            [newComment.id, newComment.text, newComment.date, newComment.user, newComment.author]
        );
        
        res.status(201).json(newComment);
    } catch (error) {
        console.error('Ошибка добавления комментария:', error);
        res.status(500).json({ error: 'Ошибка добавления комментария' });
    }
});

// Удалить комментарий
app.delete('/api/comments/:id', async (req, res) => {
    try {
        const { id } = req.params;
        const result = await pool.query('DELETE FROM comments WHERE id = $1', [id]);
        
        if (result.rowCount === 0) {
            return res.status(404).json({ error: 'Комментарий не найден' });
        }
        
        res.json({ message: 'Комментарий удален' });
    } catch (error) {
        console.error('Ошибка удаления комментария:', error);
        res.status(500).json({ error: 'Ошибка удаления комментария' });
    }
});

// Удалить все комментарии пользователя
app.delete('/api/comments/user/:user', async (req, res) => {
    try {
        const { user } = req.params;
        const result = await pool.query('DELETE FROM comments WHERE "user" = $1', [user]);
        
        if (result.rowCount === 0) {
            return res.status(404).json({ error: 'Комментарии не найдены' });
        }
        
        res.json({ message: `Все комментарии для ${user} удалены` });
    } catch (error) {
        console.error('Ошибка удаления комментариев:', error);
        res.status(500).json({ error: 'Ошибка удаления комментариев' });
    }
});

app.get('/health', (req, res) => {
    res.status(200).json({ status: 'OK', timestamp: new Date().toISOString() });
});

app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'cards.html'));
});

app.get('/cards.html', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'cards.html'));
});

// Инициализация БД и запуск сервера
async function startServer() {
    try {
        // Ждём создания таблицы перед запуском сервера
        await initDatabase();
        
        app.listen(PORT, '0.0.0.0', () => {
            console.log(`Сервер запущен на порту ${PORT}`);
            console.log(`Доступно по адресу: http://localhost:${PORT}`);
        });
    } catch (error) {
        console.error('Ошибка запуска сервера:', error);
        process.exit(1);
    }
}

startServer();