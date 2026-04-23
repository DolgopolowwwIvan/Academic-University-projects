FROM node:18-alpine

# Установим рабочую директорию 
WORKDIR /app

# Копируем нужные файлы и скачиваем зависимости из package.json
COPY package.json ./

RUN npm install --production

COPY server.js ./
COPY public/ ./public/

# Открываем порт
EXPOSE 3000

# Выполняем команду запуска "npm" + "start": npm start
CMD ["npm", "start"]

