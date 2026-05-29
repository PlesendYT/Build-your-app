# Use the official Node.js LTS image as the base
FROM node:20-alpine

# Set the working directory inside the container
WORKDIR /app

# Copy package files and install dependencies
COPY package.json .
RUN npm install --production

# Copy the application source code
COPY index.js .

# Run the app when the container starts
CMD ["node", "index.js"]
