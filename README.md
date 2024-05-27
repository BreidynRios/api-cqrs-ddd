# WebApi with CQRS and DDD

### **Docker**
1. Download the file **docker-compose.yml**
2. Download the folder **InitDB**
3. In the console, write **docker-compose up -d** and press **Enter**
3. In the browser write **http://localhost:5000/swagger** and press **Enter**

### **Local**
Open the project and select **Persistence** layer as Startup Project
1. Open Tools > NuGet Package Manager > Package Manager Console
2. In Default Project, select **Persistence**
3. Write **Update-Database**, then press **Enter**
4. Select **WebApi** layer as Startup Project, then **run** the project.
5. In the browser write **http://localhost:5000/swagger** and press **Enter**

Open SQL Server 
1. Select **ManageEmployees**
2. Open new query
3. Run this query
```sql
USE ManageEmployees;
GO
IF NOT EXISTS(SELECT * FROM Employee)
BEGIN
	INSERT INTO Employee
	VALUES
	 ('Alan', 'Rios', '47544850', null, 1, GETDATE(), null, null)
	,('Juan', 'Perez', '12345678', null, 1, GETDATE(), null, null)
END
GO
IF NOT EXISTS(SELECT * FROM PermissionType)
BEGIN
	INSERT INTO PermissionType
	VALUES 
	 ('Super Administrador')
	,('Administrador')
	,('Gestor')
	,('Consultor')
	,('Secretaria')
END
```
