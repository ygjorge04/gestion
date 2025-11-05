# Stage 1: build Angular
FROM node:20 AS build

WORKDIR /app

# Copiamos package.json y package-lock.json
COPY package*.json ./

# Instalamos dependencias
RUN npm install

# Copiamos todo el proyecto
COPY . .

# Build en modo producción
RUN npm run build -- --output-path=dist/my-app

# Stage 2: servir con Nginx
FROM nginx:alpine

# Copiamos el build desde el stage anterior
COPY --from=build /app/dist/my-app /usr/share/nginx/html

# Exponemos puerto 80
EXPOSE 80

# Comando por defecto
CMD ["nginx", "-g", "daemon off;"]
