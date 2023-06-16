# Library Management System

Before starting with the installation process, ensure to modify the following section in the `appsettings.json` file which is located in the `LMS.Web` solution:
```json
"EmailSettings": {
  "Host": "Your_SMTP_Host_Here",
  "Port": Your_Port_Number_Here,
  "Password": "Your_Password_Here",
  "Email": "Your_Email_Here"
}
```

## Installation Steps

1. Ensure you have the required software installed:
    * Docker
    * Git

2. Clone the repository:
    ```bash
    git clone https://github.com/recepgunes1/LibraryManagementSystem-WebBased.git
    ```

3. Navigate into the project directory:
    ```bash
    cd LibraryManagementSystem-WebBased
    ```

4. Use Docker to build and run the project:
    ```bash
    docker-compose up -d --build
    ```

5. Verify the installation:
    ```bash
    docker ps
    ```

If everything went well, you should see your new Docker container running!

## Default User Details
Upon successful installation, the application creates a default user with administrative privileges. The details are as follows:

- **Username:** 111111111
- **Email:** admin@admin.com
- **Password:** Admin123.
- **Role:** admin
