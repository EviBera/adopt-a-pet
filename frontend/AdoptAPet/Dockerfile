# Step 1: Build the Angular app
FROM node:18 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire Angular project into the container
COPY . .

# Build the Angular project
RUN npm run build

# Step 2: Serve the Angular app using Nginx
FROM nginx:alpine

# Copy the built Angular app from the previous step
COPY --from=build /app/dist/adopt-apet/browser /usr/share/nginx/html

# Copy the custom Nginx configuration (optional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Check if the backend is available
COPY wait-for-backend.sh /usr/local/bin/wait-for-backend.sh

RUN chmod +x /usr/local/bin/wait-for-backend.sh

# Expose the port Nginx is running on
EXPOSE 80

# Start Nginx server
CMD ["wait-for-backend.sh", "nginx", "-g", "daemon off;"]
