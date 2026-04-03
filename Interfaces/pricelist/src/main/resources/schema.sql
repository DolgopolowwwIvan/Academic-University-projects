-- Создание базы данных (выполнить вручную в PostgreSQL)
-- CREATE DATABASE pricelist;

-- Скрипт инициализации данных
-- Создание первого администратора (логин: admin, пароль: 12345)
-- Пароль хеширован с BCrypt

INSERT INTO users (login, email, password_hash, role) 
VALUES ('admin', 'admin@example.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZRGdjGj/n3.aS3ZZ5NBqK0lV0k7W6', 1)
ON CONFLICT (login) DO NOTHING;

-- Пример категорий
INSERT INTO categories (name, description) VALUES 
('Электроника', 'Телефоны, компьютеры, планшеты'),
('Бытовая техника', 'Холодильники, стиральные машины, пылесосы'),
('Одежда', 'Мужская, женская, детская одежда')
ON CONFLICT (name) DO NOTHING;