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