FROM node:20-alpine AS build
WORKDIR /app
COPY package.json ./
RUN npm install --omit=dev
COPY server.js ./
FROM nginx:1.25-alpine
COPY --from=build /app /usr/share/nginx/html